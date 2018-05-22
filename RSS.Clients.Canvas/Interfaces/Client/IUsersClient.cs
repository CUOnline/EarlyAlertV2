using RSS.Clients.Canvas.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RSS.Clients.Canvas.Interfaces.Client
{
    public interface IUsersClient
    {
        Task<UserResult> Get(int id);

        Task<UserResult> Get(string sisUserId);

        Task<List<PageViewsResult>> GetLatestPageView(string sisUserId);

        ICoursesClient Courses { get; }
    }
}
