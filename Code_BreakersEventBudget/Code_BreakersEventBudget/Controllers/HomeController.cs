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
        UserRecordEntities1 dbContext = new UserRecordEntities1();
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(PersonalInfo newUser)
        {
            int id = 0;
            if (ModelState.IsValid)
            {

                PersonalInfo personalInfo = dbContext.PersonalInfoes.Add(newUser);
                dbContext.SaveChanges();


                var rows = dbContext.PersonalInfoes.Where(m => m.Name == newUser.Name && m.Email == newUser.Email).ToList();
              
             id=   rows[0].UserID;
                ViewBag.PersonalInfo = personalInfo;
               // ViewBag.UserID = id;
                //return RedirectToAction("Index");

            }
            //else
            //{
            //    return View("DisplayView");
            //}
            return View("DisplayView", id);
        }

        [HttpGet]
        public ViewResult DisplayView(int id)
        {
            List<List> existingList = new List<List>();
            if (id > 0)
            {
                existingList = dbContext.Lists.Where(m => m.UserID == id).ToList();
            }
            ViewBag.UserID = id;
            return View(existingList);

        }

        [HttpPost]
        public ViewResult DisplayView(List newList)
        {

            if (ModelState.IsValid)
            {
               
                List L1 = dbContext.Lists.Add(newList);
                dbContext.SaveChanges();
                //return RedirectToAction("Index");
                return View("DisplayView", newList);
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