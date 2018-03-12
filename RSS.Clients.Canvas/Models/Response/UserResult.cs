using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Clients.Canvas.Models.Response
{
    public class UserResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SortableName { get; set; }
        public string ShortName { get; set; }
        public string SisUserId { get; set; }
        public string IntegrationId { get; set; }
        public string SisImportId { get; set; }
        public string RootAccount { get; set; }
        public string LoginId { get; set; }
        public Uri AvatarUrl { get; set; }
        public string Locale { get; set; }
        public PermissionsResult Permissions { get; set; }
    }
}
