using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class ReportSettingsBLL : IReportSettingsBLL
    {
        private readonly IReportSettingsRepository reportSettingsRepository;

        public ReportSettingsBLL(IReportSettingsRepository reportSettingsRepository)
        {
            this.reportSettingsRepository = reportSettingsRepository;
        }

        public ReportSettings Add(ReportSettings model)
        {
            return reportSettingsRepository.Add(model);
        }

        public IEnumerable<ReportSettings> GetAll()
        {
            return reportSettingsRepository.GetAll();
        }

        public ReportSettings Get(int modelId)
        {
            return reportSettingsRepository.Get(modelId);
        }

        public ReportSettings Update(ReportSettings model)
        {
            model.LastUpdated = DateTime.Now;
            return reportSettingsRepository.Update(model);
        }
    }
}