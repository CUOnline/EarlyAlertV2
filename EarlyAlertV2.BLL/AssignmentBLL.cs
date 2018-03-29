using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class AssignmentBLL : IAssignmentBLL
    {
        private readonly IAssignmentRepository assignmentRepository;

        public AssignmentBLL(IAssignmentRepository assignmentRepository)
        {
            this.assignmentRepository = assignmentRepository;
        }

        public Assignment Add(Assignment model)
        {
            model.DateCreated = DateTime.Now;
            return assignmentRepository.Add(model);
        }

        public IEnumerable<Assignment> GetAll()
        {
            return assignmentRepository.GetAll();
        }

        public Assignment Get(int modelId)
        {
            return assignmentRepository.Get(modelId);
        }

        public Assignment GetByCanvasId(int canvasId)
        {
            return assignmentRepository.GetByCanvasId(canvasId);
        }

        public Assignment Update(Assignment model)
        {
            model.LastUpdated = DateTime.Now;
            return assignmentRepository.Update(model);
        }
    }
}