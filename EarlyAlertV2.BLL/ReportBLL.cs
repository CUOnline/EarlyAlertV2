using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class ReportBLL : IReportBLL
    {
        private readonly IReportRepository reportRepository;

        public ReportBLL(IReportRepository reportRepository)
        {
            this.reportRepository = reportRepository;
        }

        public Report Add(Report model)
        {
            model.DateCreated = DateTime.Now;
            return reportRepository.Add(model);
        }

        public IEnumerable<Report> GetAll()
        {
            return reportRepository.GetAll();
        }

        public Report Get(int modelId)
        {
            return reportRepository.Get(modelId);
        }

        public Report Update(Report model)
        {
            model.LastUpdated = DateTime.Now;
            return reportRepository.Update(model);
        }
    }
}