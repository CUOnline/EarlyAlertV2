using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSS.Clients.Canvas;

namespace EarlyAlertV2.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ICanvasClient canvasClient;

        public ReportsController(ICanvasClient canvasClient)
        {
            this.canvasClient = canvasClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RiskIndex()
        {
            

            return View();
        }
    }
}