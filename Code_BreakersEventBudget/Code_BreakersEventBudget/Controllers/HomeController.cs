using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_BreakersEventBudget.Models;

namespace Code_BreakersEventBudget.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(NewUser newUser)
        {
            if (ModelState.IsValid)
            {
                return View("DisplayList", newUser);
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}