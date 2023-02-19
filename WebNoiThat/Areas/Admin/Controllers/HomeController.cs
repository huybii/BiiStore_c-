using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebNoiThat.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}