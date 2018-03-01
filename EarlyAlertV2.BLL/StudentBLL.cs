using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class StudentBLL : IStudentBLL
    {
        private readonly IStudentRepository studentRepository;

        public StudentBLL(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public Student Add(Student model)
        {
            return studentRepository.Add(model);
        }

        public IEnumerable<Student> GetAll()
        {
            return studentRepository.GetAll();
        }

        public Student Get(int modelId)
        {
            return studentRepository.Get(modelId);
        }

        public Student Update(Student model)
        {
            model.LastUpdated = DateTime.Now;
            return studentRepository.Update(model);
        }
    }
}