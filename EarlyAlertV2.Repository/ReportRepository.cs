using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EarlyAlertV2.Models;
using EarlyAlertV2.Interfaces.Repository;

namespace EarlyAlertV2.Repository
{
    public class ReportRepository : RepositoryBase, IReportRepository
    {
        public ReportRepository(EarlyAlertV2Context context) : base(context) { }

        public Report Add(Report model)
        {
            Context.Reports.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<Report> GetAll()
        {
            return Context.Reports;
        }

        public Report Get(int modelId)
        {
            return Context.Reports.Find(modelId);
        }

        public Report Update(Report model)
        {
            Context.Reports.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}