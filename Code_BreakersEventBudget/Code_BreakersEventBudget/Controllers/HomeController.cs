﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_BreakersEventBudget.Models;
using Code_BreakersEventBudget.Utility;

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
            //else
            //{
            //    return View("DisplayView");
            //}
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
            var row = dbContext.PersonalInfoes.Where(m=>m.UserID ==id).ToList();
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
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            List removeRecord = dbContext.Lists.Find(id);
            var uId = removeRecord.UserID;
            dbContext.Lists.Remove(removeRecord);
            dbContext.SaveChanges();
             List<List> existingLists = dbContext.Lists.Where(m => m.UserID == uId).ToList();
            return RedirectToAction("DisplayView", new { id = uId });
            // return View("DisplayView", existingLists);
        }
        //[HttpPost]
        //public ActionResult Delete(int id, List l1)
        //{
        //    List removeRecord = dbContext.Lists.Find(id);
        //    dbContext.Lists.Remove(removeRecord);
        //    dbContext.SaveChanges();

        //    return View("DisplayView");
        //}

        //public ActionResult ContinueShopping(int id)
        //{
        //    //List<ListItem> existingList = new List<ListItem>();
        //    //if (id > 0)
        //    //{
        //    //    existingList = dbContext.ListItems.Where(m => m.ListId == id).ToList();
        //    //    if (existingList == null) { existingList = new List<ListItem>(); }
        //    //}
        //    var uId = dbContext.Lists.Where(m => m.ListID == id).Single().UserID;
        //    ViewBag.UserID = uId;
        //    ViewBag.ListID = id;
        //    ViewBag.UserName = dbContext.PersonalInfoes.Where(m => m.UserID == uId).Single().Name;
        //     ViewBag.Budget = dbContext.Lists.Where(m => m.ListID == id).Single().Budget;
        //    ViewBag.Event = dbContext.Lists.Where(m => m.ListID == id).Single().Description;
        //    return View("ContinueShopping");
        //}


        public ActionResult ContinueShopping(int id)
        {
            //List<ListItem> existingList = new List<ListItem>();
            //    //if (id > 0)
            //    //{
            //    //    existingList = dbContext.ListItems.Where(m => m.ListId == id).ToList();
            //    //    if (existingList == null) { existingList = new List<ListItem>(); }
            //    //}
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
                int UserId = int.Parse(collection["UserId"]);
                int ListId = int.Parse(collection["ListId"]);
                Helper helper = new Helper();
                List<ListItem> listItems = helper.CallWalmartAPI(strSearch,UserId,ListId);
                ViewBag.listItems = listItems;
                ViewBag.ListID = ListId;
                return View("DisplayAPIData");
            }
            catch
            {
                return View();
            }
        }


        //public ActionResult Add()
        //{

        //    return RedirectToAction("ContinueShopping");
        //    //return View("ContinueShopping");
        //    //return View();
        //}

        //     [HttpPost]
        //public ActionResult Add(ListItem listItem)
        //{

        //    listItem = dbContext.ListItems.Add(listItem);
        //        dbContext.SaveChanges();
        //        //return View("ContinueShopping");

        //    return RedirectToAction("ContinueShopping");

        //}

        

      

        [HttpPost]
        public ActionResult AddAPIValueToList(string Product, decimal Price, int ListID, int UserID, string ThumbnailUrl)
        {
            ListItem listdetail = new ListItem();
            listdetail.ProductName = Product.Substring(0, Math.Min(Product.Length, 50));
            listdetail.Price = Price;
            listdetail.ListID = ListID;
            listdetail.UserID = UserID;
            listdetail.ThumbnailUrl = ThumbnailUrl;
            dbContext.ListItems.Add(listdetail);
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {

            }
          
            return RedirectToAction("ContinueShopping" ,new { id = ListID});

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
    }
}