﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.ViewModels.BootstrapModels
{
    public class ModalFooter
    {
        public string SubmitButtonText { get; set; } = "Submit";
        public string CancelButtonText { get; set; } = "Cancel";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }
    }
}
