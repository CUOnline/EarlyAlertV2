using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Models;
using EarlyAlertV2.ViewModels.ReportViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSS.Clients.Canvas;
using RSS.Clients.Canvas.Models.Response;

namespace EarlyAlertV2.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ICanvasClient canvasClient;
        private readonly IReportBLL reportBll;

        public ReportsController(ICanvasClient canvasClient, IReportBLL reportBll)
        {
            this.canvasClient = canvasClient;
            this.reportBll = reportBll;
        }

        public IActionResult Index()
        {
            var reports = reportBll.GetAll();

            var model = new IndexViewModel();
            model.Reports = reports.ToList();
            model.AddEditReportViewModel = new AddEditReportViewModel();
            model.AddEditReportViewModel.ReportType = ReportType.RiskIndex;

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddEditReport(IndexViewModel model, IFormFile reportData)
        {
            if (ModelState.IsValid && reportData != null && reportData.Length > 0)
            {
                string fileContents;
                using (var reader = new StreamReader(reportData.OpenReadStream()))
                {
                    fileContents = await reader.ReadToEndAsync();
                }
                
                reportBll.Add(new Report()
                {
                    Name = model.AddEditReportViewModel.Name,
                    ReportType = model.AddEditReportViewModel.ReportType,
                    Status = "Pending Creation",
                    ReportData = fileContents
                });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RiskIndex(int reportId)
        {

            var model = new RiskIndexReportViewModel()
            {
                ReportId = reportId,
                Users = new List<UserResult>()
            };

            if (reportId > 0)
            {
                var report = reportBll.Get(reportId);

                foreach (var studentSidId in GetStudentSidIds(report.ReportData))
                {
                    try
                    {
                        model.Users.Add(await canvasClient.UsersClient.Get(studentSidId));

                    }
                    catch (Exception ex)
                    {
                        // 404 not found?
                    }
                }
            }

            return View(model);
        }

        private IEnumerable<string> GetStudentSidIds(string reportData)
        {
            var studentData = reportData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var data in studentData.Skip(1)) // First index contains headers
            {
                var student = data.Split(",", StringSplitOptions.RemoveEmptyEntries);
                yield return student[1];
            }
        }
    }
}