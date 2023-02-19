using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThat.Models;

namespace WebNoiThat.Areas.Admin.Controllers
{
    public class ProductManagerController : Controller
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
        // GET: Admin/ProductManager
        /*public ActionResult Index()
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var lstProduct = _context.tblsanpham.ToList();
                var lstCategory = _context.tbldanhmuc.ToList();
                ViewBag.Products = lstProduct;
                ViewBag.Categorys = lstCategory;
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
           
        }*/
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                //string searchString = collection["txtSearch"].ToString();
                if (!string.IsNullOrEmpty(collection["txtSearch"]))
                {
                    _context = new WebNoiThatDbContext();
                    string searchString = collection["txtSearch"].ToString();
                    var lstProduct = _context.tblsanpham.Where(x => x.TenSanPham.Contains(searchString));
                    var lstCategory = _context.tbldanhmuc.ToList();
                    
                    ViewBag.Products = lstProduct.ToList();
                    ViewBag.Categorys = lstCategory;
                    ViewBag.keyWord = searchString;
                    return View();
                    
                    
                }
                else
                {
                    _context = new WebNoiThatDbContext();
                    var lstProduct = _context.tblsanpham.ToList();
                    var lstCategory = _context.tbldanhmuc.ToList();
                    ViewBag.Products = lstProduct;
                    ViewBag.Categorys = lstCategory;
                    return View();
                    
                }
                
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            _context = new WebNoiThatDbContext();
            var lstCategory = _context.tbldanhmuc.ToList();
            ViewBag.lstCategory = lstCategory;
            return View();
        }
        [HttpPost]
        public ActionResult CreateProduct(tblsanpham model, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/SanPham"), fileName);
                file.SaveAs(path);

                _context = new WebNoiThatDbContext();
                model.TenSanPham = collection["txtTenSanPham"];
                model.IdDanhMuc = Convert.ToInt32(collection["txtDanhMuc"]);
                model.MoTaSanPham = collection["txtMoTa"];
                model.GiaGoc = Convert.ToInt32(collection["txtGiaGoc"]);
                model.GiaKhuyenMai = Convert.ToInt32(collection["txtGiaKhuyenMai"]);
                model.DuongDanAnh = "/Content/images/SanPham/" + fileName;

                _context.tblsanpham.Add(model);
                _context.SaveChanges();
                SetAlert("success", "Ghi dữ liệu thành công");
                return RedirectToAction("Index", "ProductManager");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditProduct(int id, int idDanhMuc)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var model = _context.tblsanpham.Where(x => x.ID == id).FirstOrDefault();
                var lstCategory = _context.tbldanhmuc.ToList();
                var Category = _context.tbldanhmuc.Where(x => x.ID == idDanhMuc).FirstOrDefault();
                ViewBag.lstCategory = lstCategory;
                ViewBag.Category = Category;
                return View(model);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult EditProduct(int id, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var model = _context.tblsanpham.Where(x => x.ID == id).FirstOrDefault();
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/SanPham"), fileName);
                file.SaveAs(path);

                model.TenSanPham = collection["txtTenSanPham"];
                model.IdDanhMuc = Convert.ToInt32(collection["txtDanhMuc"]);
                model.MoTaSanPham = collection["txtMoTa"];
                model.GiaGoc = Convert.ToInt32(collection["txtGiaGoc"]);
                model.GiaKhuyenMai = Convert.ToInt32(collection["txtGiaKhuyenMai"]);
                model.DuongDanAnh = "/Content/images/SanPham/" + fileName;

                _context.SaveChanges();
                SetAlert("success", "Cập nhật dữ liệu thành công");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetAlert("error", "Cập nhật dữ liệu thất bại");

                return View();
            }
        }

        public ActionResult DeleteProduct(int id)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var model = _context.tblsanpham.Where(x => x.ID == id).FirstOrDefault();
                _context.tblsanpham.Remove(model);
                _context.SaveChanges();
                SetAlert("success", "Xóa sản phẩm thành công");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetAlert("error", "Lỗi");
                return RedirectToAction("Index");
            }
            
        }

        /*public ActionResult SearchProduct(FormCollection collection)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                string searchString = collection["txtSearch"].ToString();
                var searchItem = _context.tblsanpham.Where(x => x.TenSanPham == searchString).FirstOrDefault();
                ViewBag.Products = searchItem;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }*/
    }
}