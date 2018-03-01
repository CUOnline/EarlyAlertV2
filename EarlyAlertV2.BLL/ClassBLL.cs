using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class ClassBLL : IClassBLL
    {
        private readonly IClassRepository classRepository;

        public ClassBLL(IClassRepository classRepository)
        {
            this.classRepository = classRepository;
        }

        public Class Add(Class model)
        {
            return classRepository.Add(model);
        }

        public IEnumerable<Class> GetAll()
        {
            return classRepository.GetAll();
        }

        public Class Get(int modelId)
        {
            return classRepository.Get(modelId);
        }

        public Class Update(Class model)
        {
            model.LastUpdated = DateTime.Now;
            return classRepository.Update(model);
        }
    }
}