using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_BreakersEventBudget.Models;
using Code_BreakersEventBudget.Utility;
using System.Net.Mail;

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
        public ActionResult Index(PersonalInfo newUser)
        {
            int id = 0;

            if (ModelState.IsValid)
            {
                var emailRecord = dbContext.PersonalInfoes.Where(m => m.Email.ToLower() == newUser.Email.ToLower());
                if (emailRecord == null || emailRecord.ToList().Count == 0)
                {
                    PersonalInfo personalInfo = dbContext.PersonalInfoes.Add(newUser);

                    dbContext.SaveChanges();

                }
                else
                {

                }

                var rows = dbContext.PersonalInfoes.Where(m => m.Email.ToLower() == newUser.Email.ToLower()).ToList();

                id = rows[0].UserID;
                // ViewBag.PersonalInfo = personalInfo;
                ViewBag.UserID = id;
                ViewBag.UserName = newUser.Name;
                //return RedirectToAction("Index");

            }
            else
            {
                return View("Index");
            }
            return RedirectToAction("DisplayView", new { id = id });
            // return View("DisplayView");
        }

        [HttpGet]
        public ViewResult DisplayView(int id)
        {
            List<List> existingList = new List<List>();
            if (id > 0)
            {
                existingList = dbContext.Lists.Where(m => m.UserID == id).ToList();
                if (existingList == null) { existingList = new List<List>(); }
            }
            ViewBag.UserID = id;
            var row = dbContext.PersonalInfoes.Where(m => m.UserID == id).ToList();
            ViewBag.UserName = row[0].Name;
            return View(existingList);

        }

        [HttpPost]
        public ViewResult DisplayView(FormCollection form, List usersNewList)
        {
            if (usersNewList == null)
            {
                usersNewList = new List();
            }

            usersNewList.Description = form["Event"].ToString();
            usersNewList.Budget = decimal.Parse(form["Budget"]);
            usersNewList.EventDate = DateTime.Parse(form["DDate"]);
            int id = int.Parse(form["UserID"].ToString());
            ViewBag.UserID = id;
            if (ModelState.IsValid)
            {
                List L1 = dbContext.Lists.Add(usersNewList);
                dbContext.SaveChanges();

                //return RedirectToAction("Index");
                List<List> existingLists = dbContext.Lists.Where(m => m.UserID == id).ToList();

                return View("DisplayView", existingLists);
            }
            else
            {
                return View("DisplayView");
            }
        }

        public ActionResult Delete(int id)
        {
            List removeRecord = dbContext.Lists.Find(id);
            var uId = removeRecord.UserID;

            var itemList = dbContext.ListItems.Where(m => m.ListID ==id);
            dbContext.ListItems.RemoveRange(itemList.ToList());
            
            dbContext.Lists.Remove(removeRecord);
            dbContext.SaveChanges();
            List<List> existingLists = dbContext.Lists.Where(m => m.UserID == uId).ToList();
            return RedirectToAction("DisplayView", new { id = uId });
            // return View("DisplayView", existingLists);
        }


        public ActionResult ContinueShopping(int id)
        {

            List<ListItem> existingList = new List<ListItem>();
            if (id > 0)
            {
                existingList = dbContext.ListItems.Where(m => m.ListID == id).ToList();
                if (existingList == null) { existingList = new List<ListItem>(); }
            }

            var uId = dbContext.Lists.Where(m => m.ListID == id).Single().UserID;
            ViewBag.UserID = uId;
            ViewBag.ListID = id;
            ViewBag.UserName = dbContext.PersonalInfoes.Where(m => m.UserID == uId).Single().Name;
            var budget = dbContext.Lists.Where(m => m.ListID == id).Single().Budget;
            ViewBag.Budget = budget;
            ViewBag.Event = dbContext.Lists.Where(m => m.ListID == id).Single().Description;
            var recCount = dbContext.ListItems.Where(m => m.ListID == id).Count();
            var totalPrice = 0.0m;
            if (recCount > 0)
                totalPrice = dbContext.ListItems.Where(m => m.ListID == id).Sum(m => m.Price);

            DateTime eventDate = dbContext.Lists.Where(m => m.ListID == id).Single().EventDate.Value;
            TimeSpan timespan = eventDate - DateTime.Now;
            ViewBag.DaysLeft = timespan.Days;
            ViewBag.TotalPrice = totalPrice;
            if (totalPrice > budget)
            {
                ViewBag.Message = "You have exceeded your budget!";
            }
            else
            {
                var balanceRemain = budget - totalPrice;
                ViewBag.Message = "Remaining budget balance: " + balanceRemain.ToString();
            }
            return View("ContinueShopping", existingList);
        }


        public ActionResult Search()
        {
            return View("ContinueShopping");
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            try
            {
                string strSearch = collection["search"];
                string giftFor = collection["GiftFor"];
                int UserId = int.Parse(collection["UserId"]);
                int ListId = int.Parse(collection["ListId"]);
                Helper helper = new Helper();
                List<ListItem> listItems = helper.CallWalmartAPI(strSearch, giftFor, UserId, ListId);
                ViewBag.listItems = listItems;
                ViewBag.ListID = ListId;
                return View("DisplayAPIData");
            }
            catch
            {
                return View();
            }
        }



        [HttpPost]
        public ActionResult AddAPIValueToList(string Product, decimal Price, int ListID, int UserID, string ThumbnailUrl, string GiftFor)
        {
            ListItem listdetail = new ListItem();
            listdetail.ProductName = Product.Substring(0, Math.Min(Product.Length, 50));
            listdetail.Price = Price;
            listdetail.ListID = ListID;
            listdetail.UserID = UserID;
            listdetail.ThumbnailUrl = ThumbnailUrl;
            listdetail.GiftFor = GiftFor;
            dbContext.ListItems.Add(listdetail);
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("ContinueShopping", new { id = ListID });

        }

        public ActionResult DeleteFromListItem(int id)
        {
            ListItem removeRecord = dbContext.ListItems.Find(id);
            var lId = removeRecord.ListID;
            dbContext.ListItems.Remove(removeRecord);
            dbContext.SaveChanges();
            List<List> existingLists = dbContext.Lists.Where(m => m.ListID == lId).ToList();
            return RedirectToAction("ContinueShopping", new { id = lId });
            // return View("DisplayView", existingLists);
        }



        public ActionResult SendEmail(FormCollection collection)
        {
            string strUrl= collection["urlField"];
            int uId = int.Parse(collection["UserID"]);
            int lId = int.Parse(collection["ListID"]);
            var emailAddress = dbContext.PersonalInfoes.Where(m => m.UserID == uId).Single().Email;
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //mail.From = new MailAddress("ritesh.chakravarty@gmail.com", "Testing Email");
            mail.From = new MailAddress("teamcodebreaker@gmail.com", "BudgetBuddy");
            mail.To.Add(emailAddress);
            mail.IsBodyHtml = true;
            mail.Subject = "List of the Items for your event from Budget Buddy";
            //string url = "https://blogs.msdn.microsoft.com/";

            Helper oHelper = new Helper();
            mail.Body = oHelper.HttpContent(strUrl);
            mail.Priority = System.Net.Mail.MailPriority.High;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential("teamcodebreaker@gmail.com", "GrandCircus");
            smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Send(mail);
            ViewBag.lId = lId;
            return View("SendEmail");
        }

    }
}