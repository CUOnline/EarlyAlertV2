using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class AssignmentGroupBLL : IAssignmentGroupBLL
    {
        private readonly IAssignmentGroupRepository assignmentGroupRepository;

        public AssignmentGroupBLL(IAssignmentGroupRepository assignmentGroupRepository)
        {
            this.assignmentGroupRepository = assignmentGroupRepository;
        }

        public AssignmentGroup Add(AssignmentGroup model)
        {
            return assignmentGroupRepository.Add(model);
        }

        public IEnumerable<AssignmentGroup> GetAll()
        {
            return assignmentGroupRepository.GetAll();
        }

        public IQueryable<AssignmentGroup> GetAllByCourseId(int courseId)
        {
            return assignmentGroupRepository.GetAllByCourseId(courseId);
        }

        public AssignmentGroup Get(int modelId)
        {
            return assignmentGroupRepository.Get(modelId);
        }

        public AssignmentGroup GetByCanvasId(int canvasId)
        {
            return assignmentGroupRepository.GetByCanvasId(canvasId);
        }

        public AssignmentGroup Update(AssignmentGroup model)
        {
            model.LastUpdated = DateTime.Now;
            return assignmentGroupRepository.Update(model);
        }
    }
}