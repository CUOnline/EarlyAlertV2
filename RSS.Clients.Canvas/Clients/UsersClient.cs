using RSS.Clients.Canvas.Helpers;
using RSS.Clients.Canvas.Interfaces.Client;
using RSS.Clients.Canvas.Interfaces.Http;
using RSS.Clients.Canvas.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace RSS.Clients.Canvas.Clients
{
    public class UsersClient : ApiClient, IUsersClient
    {
        public UsersClient(IApiConnection apiConnection)
            : base(apiConnection)
        {
            Courses = new CoursesClient(apiConnection);
        }

        public ICoursesClient Courses { get; private set; }

        public Task<UserResult> Get(int id)
        {
            return ApiConnection.Get<UserResult>(ApiUrls.User(id));
        }

        public Task<UserResult> Get(string sisUserId)
        {
            return ApiConnection.Get<UserResult>(ApiUrls.User(sisUserId));
        }

        public Task<List<PageViewsResult>> GetLatestPageView(string sisUserId)
        {
            return ApiConnection.Get<List<PageViewsResult>>(ApiUrls.UserLatestPageView(sisUserId));
        }
    }
}
