using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyAlertV2.Interfaces.BLL
{
    public interface IBLL<T>
    {
        T Add(T model);
        IEnumerable<T> GetAll();
        T Get(int modelId);
        T Update(T model);
    }
}
