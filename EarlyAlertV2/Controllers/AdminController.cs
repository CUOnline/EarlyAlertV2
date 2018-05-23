using EarlyAlertV2.Helpers;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.Controllers
{
    [AuthorizeRoles(UserRoles.AdminRole, UserRoles.OITAdminRole)]
    public class AdminController : Controller
    {
        private readonly IReportSettingsBLL reportSettingsBll;

        public AdminController(IReportSettingsBLL reportSettingsBll)
        {
            this.reportSettingsBll = reportSettingsBll;
        }

        public IActionResult ReportSettings()
        {
            var model = reportSettingsBll.GetAll().FirstOrDefault() ?? new ReportSettings();

            return View(model);
        }

        [HttpPost]
        public IActionResult ReportSettings(ReportSettings reportSettings)
        {
            var settingsSum = GetReportSettingsSum(reportSettings);
            if (settingsSum != 100.0)
            {
                ModelState.AddModelError("InvalidSum", $"Sum of Weights Must Equal 100 Percent. Current Value: {settingsSum}");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (reportSettings.Id != 0)
                    {
                        reportSettingsBll.Update(reportSettings);
                    }
                    else
                    {
                        reportSettingsBll.Add(reportSettings);
                    }

                    return RedirectToAction(nameof(ReportSettingsConfirmation));
                }
            }


            return View(reportSettings);
        }

        public IActionResult ReportSettingsConfirmation()
        {
            return View();
        }

        private double GetReportSettingsSum(ReportSettings reportSettings)
        {
            return reportSettings.ActivityWeight 
                + reportSettings.CommunicationWeight 
                + reportSettings.GradeWeight 
                + reportSettings.LateAssignmentsWeight 
                + reportSettings.MissedAssignmentsWeight 
                + reportSettings.NumberOfActiveCoursesWeight 
                + reportSettings.PageViewsWeight 
                + reportSettings.ParticipationWeight;
        }
    }
}
