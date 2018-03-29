using RSS.Clients.Canvas.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RSS.Clients.Canvas.Interfaces.Client
{
    public interface IAssignmentsClient
    {
        Task<AssignmentResult> Get(int assignmentId);
        Task<IReadOnlyList<AssignmentResult>> GetAll(int courseId);
    }
}
