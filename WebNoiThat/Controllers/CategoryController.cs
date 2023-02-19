using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThat.Models;

namespace WebNoiThat.Controllers
{
    public class CategoryController : Controller
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
        // GET: Category
        [HttpGet]
        public ActionResult Index(int id)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var Category = _context.tbldanhmuc.Where(x => x.ID == id).FirstOrDefault();
                var lstCategory = _context.tbldanhmuc.ToList();
                var lstProduct = _context.tblsanpham.Where(x => x.IdDanhMuc == id).ToList();
                ViewBag.lstProduct = lstProduct;
                ViewBag.lstCategory = lstCategory;
                ViewBag.Category = Category.TenDanhMuc;
                ViewBag.id = id;
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }


        [HttpPost]
        public ActionResult FilterProduct(FormCollection collection)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var checkedItem = collection["rdoDanhMuc"];


                return RedirectToAction("Index",new { id = checkedItem });
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }
    }
}