﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.ViewModels
{
    public class CanvasOAuth
    {
      public string AuthorizationEndpoint { get; set; }
      public string TokenEndpoint { get; set; }
      public string ProfileEndpoint { get; set; }
      public string ClientId { get; set; }
      public string ClientSecret { get; set; }
    }
}
