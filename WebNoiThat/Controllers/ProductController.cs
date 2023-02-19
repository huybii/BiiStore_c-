using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThat.Models;

namespace WebNoiThat.Controllers
{
    public class ProductController : Controller
    {
        WebNoiThatDbContext _context;
        protected void SetAlert(string type, string message)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "success";
            }
            else
            {
                TempData["AlertType"] = "error";
            }
        }
        // GET: Product
        [HttpGet]
        public ActionResult Index(int id)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var model = _context.tblsanpham.Where(x => x.ID == id).FirstOrDefault();

                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }
    }
}