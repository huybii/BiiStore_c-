using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThat.Models;

namespace WebNoiThat.Controllers
{
    public class LoginController : Controller
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
        // GET: Login
        public ActionResult Index()
        {
            
                if (Request.Cookies["email"] != null)
                {
                    ViewBag.txtEmail = Request.Cookies["email"].Value;
                }
                if (Request.Cookies["pass"] != null)
                {
                    ViewBag.txtPass = Request.Cookies["pass"].Value;
                }
            
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            /*string message = string.Empty;*/
            try
            {
                _context = new WebNoiThatDbContext();
                string acc = collection["txtAccount"].ToString();
                string pass = collection["txtPass"].ToString();
                var item = _context.tbldangnhap.Where(x => x.Email == acc && x.MatKhau == pass).FirstOrDefault();
                if (item != null)
                {
                    if (item.TrangThai == "hoạt động")
                    {
                        if (!string.IsNullOrEmpty(collection["NhoMk"]) && collection["NhoMk"].ToString() == "yes")
                        {
                            Response.Cookies["email"].Value = acc;
                            Response.Cookies["pass"].Value = pass;
                            Response.Cookies["email"].Expires = DateTime.Now.AddDays(15);
                            Response.Cookies["pass"].Expires = DateTime.Now.AddDays(15);
                        }
                        else
                        {
                            Response.Cookies["email"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["pass"].Expires = DateTime.Now.AddDays(-1);
                        }
                        if (item.Email == "admin")
                        {
                            Session["account"] = acc;
                            Session["pass"] = pass;
                            SetAlert("success", "Đăng nhập thành công");
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        else
                        {
                            Session["account"] = acc;
                            Session["pass"] = pass;
                            SetAlert("success", "Đăng nhập thành công");
                            return RedirectToAction("Index", "Home");
                        }
                        
                    }
                    else
                    {
                        SetAlert("error", "Tài khoản đã bị chặn");
                        return View();
                    }
                }
                else
                {
                    SetAlert("error", "Đăng nhập thất bại");
                    return RedirectToAction("Index", "Login");

                }

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public ActionResult LogOut()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(FormCollection collection)
        {
            if (Session["account"] != null)
            {
                string EmailUser = Session["account"].ToString();
                _context = new WebNoiThatDbContext();
                var acc = _context.tbldangnhap.Where(x => x.Email == EmailUser).FirstOrDefault();
                if (collection["txtMatKhauHienTai"].ToString() == acc.MatKhau)
                {
                    if (collection["txtMatKhauMoi"].ToString() == collection["txtXacNhanMatKhauMoi"].ToString())
                    {
                        acc.MatKhau = collection["txtXacNhanMatKhauMoi"].ToString();
                        _context.SaveChanges();
                        SetAlert("success", "Đổi mật khẩu thành công");
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        SetAlert("error", "Mật khẩu xác nhận chưa khớp");
                        return View();
                    }
                }
                else
                {
                    SetAlert("error", "Mật khẩu hiện tại không đúng");
                    return View();
                }

            }
            else
            {
                return null;
            }
            
        }
        
    }
}