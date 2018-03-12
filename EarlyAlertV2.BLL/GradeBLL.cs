using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class GradeBLL : IGradeBLL
    {
        private readonly IGradeRepository gradeRepository;

        public GradeBLL(IGradeRepository gradeRepository)
        {
            this.gradeRepository = gradeRepository;
        }

        public Grade Add(Grade model)
        {
            model.DateCreated = DateTime.Now;
            return gradeRepository.Add(model);
        }

        public IEnumerable<Grade> GetAll()
        {
            return gradeRepository.GetAll();
        }

        public Grade Get(int modelId)
        {
            return gradeRepository.Get(modelId);
        }

        public Grade Update(Grade model)
        {
            model.LastUpdated = DateTime.Now;
            return gradeRepository.Update(model);
        }
    }
}