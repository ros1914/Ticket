using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RTSTicket.Web.Filters;

namespace RTSTicket.Web.Controllers
{
    [MyAuthorizationFilter(Role = "Administrator")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}