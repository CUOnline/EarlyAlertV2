using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class StudentAssignmentSubmissionBLL : IStudentAssignmentSubmissionBLL
    {
        private readonly IStudentAssignmentSubmissionRepository studentAssignmentSubmissionRepository;

        public StudentAssignmentSubmissionBLL(IStudentAssignmentSubmissionRepository studentAssignmentSubmissionRepository)
        {
            this.studentAssignmentSubmissionRepository = studentAssignmentSubmissionRepository;
        }

        public StudentAssignmentSubmission Add(StudentAssignmentSubmission model)
        {
            return studentAssignmentSubmissionRepository.Add(model);
        }

        public IEnumerable<StudentAssignmentSubmission> GetAll()
        {
            return studentAssignmentSubmissionRepository.GetAll();
        }

        public StudentAssignmentSubmission Get(int modelId)
        {
            return studentAssignmentSubmissionRepository.Get(modelId);
        }

        public StudentAssignmentSubmission GetByCanvasId(int canvasId)
        {
            return studentAssignmentSubmissionRepository.GetByCanvasId(canvasId);
        }

        public StudentAssignmentSubmission Update(StudentAssignmentSubmission model)
        {
            model.LastUpdated = DateTime.Now;
            return studentAssignmentSubmissionRepository.Update(model);
        }
    }
}