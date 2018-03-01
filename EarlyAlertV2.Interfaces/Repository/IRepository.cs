using System;
using System.Linq;
using System.Text;

namespace EarlyAlertV2.Interfaces.Repository
{
    public interface IRepository<T>
    {
        T Add(T model);
        IQueryable<T> GetAll();
        T Get(int modelId);
        T Update(T model);
    }
}
