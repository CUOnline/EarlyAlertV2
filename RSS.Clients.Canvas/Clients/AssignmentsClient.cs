using RSS.Clients.Canvas.Interfaces.Client;
using RSS.Clients.Canvas.Interfaces.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Clients.Canvas.Clients
{
    public class AssignmentsClient : ApiClient, IAssignmentsClient
    {
        public AssignmentsClient(IApiConnection apiConnection)
            : base(apiConnection)
        {

        }
    }
}
