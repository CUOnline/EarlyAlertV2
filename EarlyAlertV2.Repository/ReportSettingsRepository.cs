using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EarlyAlertV2.Models;
using EarlyAlertV2.Interfaces.Repository;

namespace EarlyAlertV2.Repository
{
    public class ReportSettingsRepository : RepositoryBase, IReportSettingsRepository
    {
        public ReportSettingsRepository(EarlyAlertV2Context context) : base(context) { }

        public ReportSettings Add(ReportSettings model)
        {
            Context.ReportSettings.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<ReportSettings> GetAll()
        {
            return Context.ReportSettings;
        }

        public ReportSettings Get(int modelId)
        {
            return Context.ReportSettings.Find(modelId);
        }

        public ReportSettings Update(ReportSettings model)
        {
            Context.ReportSettings.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}