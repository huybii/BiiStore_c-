using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThat.Models;

namespace WebNoiThat.Areas.Admin.Controllers
{
    public class UserManagerController : Controller
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
        string connectionString = @"Data Source=LAPTOP-FLL0C1VV\SQLEXPRESS;Initial Catalog=webnoithat;Integrated Security=True";
        SqlDataAdapter da;
        SqlConnection conn;
        DataSet ds;
        public static string State = "";
        // GET: Admin/UserManager
        public ActionResult Index(FormCollection collection)
        {
            _context = new WebNoiThatDbContext();
            var lstUser = _context.tbldangnhap.Where(x => x.Email != "admin").ToList();
            if (collection["txtSearch"] != null)
            {
                string keyWord = collection["txtSearch"].ToString();
                ViewBag.Keyword = keyWord;
                lstUser = _context.tbldangnhap.Where(x => x.Email.Contains(keyWord) || x.TenHienThi.Contains(keyWord)).ToList();
            }
            //string ListOrderFail = "";
            List<UserItem> ListOrderFail = new List<UserItem>();
            foreach (var item in lstUser)
            {
                SqlConnection conn1 = new SqlConnection(connectionString);
                //cmd1 = new SqlCommand();

                string query1 = "SELECT count(*) as 'bom' from tbldonhang t where EmailUser = '" + item.Email + "' and TrangThai = N'giao thất bại'";
                SqlCommand cmd1 = new SqlCommand(query1, conn1);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    
                        UserItem OrderFailItem = new UserItem();
                        OrderFailItem.id = item.ID;
                        OrderFailItem.SoLanBom = Convert.ToInt32(ds1.Tables[0].Rows[0]["bom"]);
                        ListOrderFail.Add(OrderFailItem);
                    
                }
            }
            ViewBag.SoLanBom = ListOrderFail;
            ViewBag.lstUser = lstUser;
            return View();
        }
        public class UserItem
        {
            public int id { get; set; }
            public int SoLanBom { get; set; }
           
        }

        public ActionResult BlockUser(int id)
        {
            _context = new WebNoiThatDbContext();
            var User = _context.tbldangnhap.Where(x => x.ID == id).FirstOrDefault();
            User.TrangThai = "chặn";
            _context.SaveChanges();
            SetAlert("success", "Chặn người dùng thành công");
            return RedirectToAction("Index", "UserManager");
        }

        public ActionResult UnBlockUser(int id)
        {
            _context = new WebNoiThatDbContext();
            var User = _context.tbldangnhap.Where(x => x.ID == id).FirstOrDefault();
            User.TrangThai = "hoạt động";
            _context.SaveChanges();
            SetAlert("success", "Bỏ chặn người dùng thành công");
            return RedirectToAction("Index", "UserManager");
        }
    }
}