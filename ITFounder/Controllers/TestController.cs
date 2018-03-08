using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITFounder.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Home()
        {
            return View("HomeView");
        }
    }
}