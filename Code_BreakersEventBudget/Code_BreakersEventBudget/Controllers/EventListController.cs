using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Code_BreakersEventBudget.Controllers
{
    public class EventListController : Controller
    {
        // GET: EventList
        public ActionResult Index()
        {
            return View();
        }
      public ActionResult  ListEvents(string listid)
        {
            FinalProj_ListEntitiesLocal dbcontext = new FinalProj_ListEntitiesLocal();
            List lookupList = dbcontext.Lists.Find(listid);
            if (lookupList == null)
            {
                lookupList = new List();
                lookupList.Discription = "Dummy";
                lookupList.Budget = 2000;
                lookupList.ListID = "zero";
              //todo redirect after page is create  return Redirect(controller name and action);

            }


            ViewBag.ExampleList = lookupList;
            return View(lookupList);
        }
    }
}