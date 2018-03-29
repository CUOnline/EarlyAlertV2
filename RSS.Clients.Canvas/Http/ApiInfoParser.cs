﻿using RSS.Clients.Canvas.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RSS.Clients.Canvas.Http
{
    internal static class ApiInfoParser
    {
        const RegexOptions regexOptions = RegexOptions.IgnoreCase;

        static readonly Regex _linkRelRegex = new Regex("rel=\"(current|next|prev|first|last)\"", regexOptions);
        static readonly Regex _linkUriRegex = new Regex("<(.+)>", regexOptions);

        public static ApiInfo ParseResponseHeaders(IDictionary<string, string> responseHeaders)
        {
            Ensure.ArgumentNotNull(responseHeaders, "responseHeaders");

            var httpLinks = new Dictionary<string, Uri>();
            var oauthScopes = new List<string>();
            var acceptedOauthScopes = new List<string>();
            string etag = null;

            if (responseHeaders.ContainsKey("X-Accepted-OAuth-Scopes"))
            {
                acceptedOauthScopes.AddRange(responseHeaders["X-Accepted-OAuth-Scopes"]
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()));
            }

            if (responseHeaders.ContainsKey("X-OAuth-Scopes"))
            {
                oauthScopes.AddRange(responseHeaders["X-OAuth-Scopes"]
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()));
            }

            if (responseHeaders.ContainsKey("ETag"))
            {
                etag = responseHeaders["ETag"];
            }

            if (responseHeaders.ContainsKey("Link"))
            {
                var links = responseHeaders["Link"].Split(',');
                foreach (var link in links)
                {
                    var relMatch = _linkRelRegex.Match(link);
                    if (!relMatch.Success || relMatch.Groups.Count != 2) break;

                    var uriMatch = _linkUriRegex.Match(link);
                    if (!uriMatch.Success || uriMatch.Groups.Count != 2) break;

                    httpLinks.Add(relMatch.Groups[1].Value, new Uri(uriMatch.Groups[1].Value));
                }
            }

            return new ApiInfo(httpLinks, oauthScopes, acceptedOauthScopes, etag, new RateLimit(responseHeaders));
        }
    }
}
