using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WebNoiThat.Models;


namespace WebNoiThat.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        WebNoiThatDbContext _context;
        string error;
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
        // GET: Admin/Category
        public ActionResult Index(FormCollection collection)
        {
            if (!string.IsNullOrEmpty(collection["txtSearch"]))
            {
                string searchString = collection["txtSearch"].ToString();

                
                ViewBag.txtSearch = searchString;

                WebService1.WebService1 webService = new WebService1.WebService1();
                ViewBag.lstCategory = webService.GetCategoryByName(searchString);
                return View();
            }
            else
            {
               
                try
                {
                    WebService1.WebService1 webService = new WebService1.WebService1();
                    ViewBag.lstCategory = webService.GetCategory();

                }
                catch (Exception ex)
                {
                    var Error = ex.Message;

                }

                
                return View();
            }

        }
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(tbldanhmuc model, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {

                _context = new WebNoiThatDbContext();

                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/DanhMuc"), fileName);
                file.SaveAs(path);

                

                WebService1.WebService1 webService = new WebService1.WebService1();
                WebNoiThat.WebService1.tbldanhmuc item = new WebNoiThat.WebService1.tbldanhmuc();
                item.TenDanhMuc = collection["txtTenDanhMuc"];
                item.DuongDanAnh = "/Content/images/DanhMuc/" + fileName;
                var result = webService.AddCategory(item, ref error);
                if (result == true)
                {
                    SetAlert("success", "ghi dữ liệu thành công ");
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    SetAlert("error", "Lỗi");
                    return View();
                }
                SetAlert("success", "ghi dữ liệu thành công ");
                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var model = _context.tbldanhmuc.Where(x => x.ID == id).FirstOrDefault();
                return View(model);

            }
            catch (Exception ex)
            {

                return View();
            }
        }
        [HttpPost]
        public ActionResult EditCategory(int id, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var model = _context.tbldanhmuc.Where(x => x.ID == id).FirstOrDefault();
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/DanhMuc"), fileName);
                file.SaveAs(path);

                model.TenDanhMuc = collection["txtTenDanhMuc"];
                model.DuongDanAnh = "/Content/images/DanhMuc/" + fileName;

                _context.SaveChanges();
                SetAlert("success", "Cập nhật dữ liệu thành công");
                return RedirectToAction("Index");
                WebService1.WebService1 webService = new WebService1.WebService1();
                //WebNoiThat.WebService1.tbldanhmuc item = new WebNoiThat.WebService1.tbldanhmuc();

               /* item.TenDanhMuc = collection["txtTenDanhMuc"];
                item.DuongDanAnh = "/Content/images/DanhMuc/" + fileName;*/
                string TenDanhMuc = collection["txtTenDanhMuc"].ToString();
                string DuongDanAnh = "/Content/images/DanhMuc/" + fileName;
                var result = webService.EditCategory(id, TenDanhMuc, DuongDanAnh, ref error);
                if (result == true)
                {
                    SetAlert("success", "sửa dữ liệu thành công ");
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    SetAlert("error", "Lỗi");
                    return View();
                }
            }
            catch (Exception ex)
            {

                return View();
            }
        }

        public ActionResult DeleteCategory(int id)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var model = _context.tbldanhmuc.Where(x => x.ID == id).FirstOrDefault();
                _context.tbldanhmuc.Remove(model);
                _context.SaveChanges();
                SetAlert("success", "Xóa danh mục thành công");
                return RedirectToAction("Index");

                WebService1.WebService1 webService = new WebService1.WebService1();
                WebNoiThat.WebService1.tbldanhmuc item = new WebNoiThat.WebService1.tbldanhmuc();


                var result = webService.RemoveCategory(id, ref error);
                if (result == true)
                {
                    SetAlert("success", "xóa danh mục thành công ");
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    SetAlert("error", "Lỗi");
                    return RedirectToAction("Index", "Category");
                }
            }
            catch (Exception ex)
            {
                SetAlert("error", "Lỗi");
                return RedirectToAction("Index");
            }
        }


    }
}