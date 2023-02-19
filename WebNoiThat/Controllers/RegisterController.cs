using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThat.Models;

namespace WebNoiThat.Controllers
{
    public class RegisterController : Controller
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
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(tbldangnhap model, FormCollection collection)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                model.Email = collection["txtEmail"];
                model.MatKhau = collection["txtPass"];
                model.SDT = collection["txtSDT"];
                model.PhanQuyen = "customer";
                model.NgayTao = DateTime.Now;
                model.TrangThai = "hoạt động";
                model.TenHienThi = collection["txtHoTen"];
                string _email = collection["txtEmail"];
                var modelCheck = _context.tbldangnhap.Where(x => x.Email == _email).FirstOrDefault();
                if (modelCheck != null)
                {
                    SetAlert("error", "Email đã tồn tại!");
                    return View();
                }
                else
                {
                    if (collection["txtPass"] != collection["txtRePass"])
                    {
                        SetAlert("error", "Mật khẩu xác nhận không trùng khớp");
                        return View();
                    }
                    else
                    {
                        _context.tbldangnhap.Add(model);
                        _context.SaveChanges();
                        SetAlert("success", "Đăng kí thành công");
                        return RedirectToAction("Index", "Login");
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}