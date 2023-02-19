using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThat.Models;
namespace WebNoiThat.Controllers
{
    public class CartController : Controller
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

        // GET: Cart
        public ActionResult Index()
        {
            if (Session["account"] != null)
            {
                string emailUser = Session["account"].ToString();
                _context = new WebNoiThatDbContext();
                var CartItems = _context.tblgiohang.Where(x => x.EmailUser == emailUser).ToList();
                var productInfo = _context.tblsanpham.ToList();
                ViewBag.productInfo = productInfo;

                SqlConnection conn = new SqlConnection(connectionString);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string query = "SELECT sum(SoLuong*DonGia) as 'TongTien' from tblGioHang where EmailUser = '"+ emailUser +"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                ViewBag.TotalBill = ds.Tables[0].Rows[0]["TongTien"].ToString();
                return View(CartItems);
            }
            else
            {
                return null;
            }
            
        }

        public ActionResult plusQuantity(string productName)
        {
            if (Session["account"] != null)
            {
                string emailUser = Session["account"].ToString();
                _context = new WebNoiThatDbContext();
                var quantity = _context.tblgiohang.Where(x => x.EmailUser == emailUser && x.TenSanPham == productName).FirstOrDefault();
                quantity.SoLuong += 1;
                _context.SaveChanges();
                
            }
            return RedirectToAction("Index");
        }

        public ActionResult subQuantity(string productName)
        {
            if (Session["account"] != null)
            {
                string emailUser = Session["account"].ToString();
                _context = new WebNoiThatDbContext();
                var quantity = _context.tblgiohang.Where(x => x.EmailUser == emailUser && x.TenSanPham == productName).FirstOrDefault();
                quantity.SoLuong = quantity.SoLuong - 1;
                _context.SaveChanges();


            }
            return RedirectToAction("Index");
        }

        public ActionResult Order(tblgiohang model, FormCollection collection)
        {
            if (Session["account"] != null)
            {
                string emailUser = Session["account"].ToString();
                _context = new WebNoiThatDbContext();

                tbldonhang modelDonHang = new tbldonhang();
                modelDonHang.EmailUser = emailUser;
                modelDonHang.TongTien = Convert.ToInt32(collection["txtTongBill"]);
                modelDonHang.TenNguoiNhan = collection["txtHoTen"];
                modelDonHang.SDT = Convert.ToInt32(collection["txtSDT"]);
                modelDonHang.ThanhPho = collection["txtThanhPho"];
                modelDonHang.Huyen = collection["txtHuyen"];
                modelDonHang.Xa = collection["txtXa"];
                modelDonHang.DiaChiChiTiet = collection["txtDiaChiChiTiet"];
                modelDonHang.NgayDat = DateTime.Now;
                modelDonHang.TrangThai = "chờ xác nhận";
                _context.tbldonhang.Add(modelDonHang);
                _context.SaveChanges();

                SqlConnection conn1 = new SqlConnection(connectionString);

                string query1 = "SELECT max(ID) as 'IdDonHang' from tblDonHang where EmailUser = '" + emailUser + "' ";
                SqlCommand cmd1 = new SqlCommand(query1, conn1);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);

                string query2 = "SELECT * from tblGioHang where EmailUser = '" + emailUser + "'";
                SqlCommand cmd2 = new SqlCommand(query2, conn1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                da2.Fill(ds2);
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        tbldathang model1 = new tbldathang();
                        model1.EmailUser = emailUser;
                        model1.TenSanPham = ds2.Tables[0].Rows[i]["TenSanPham"].ToString();
                        model1.DonGia = Convert.ToInt32(ds2.Tables[0].Rows[i]["DonGia"]);
                        model1.SoLuong = Convert.ToInt32(ds2.Tables[0].Rows[i]["SoLuong"]);
                        model1.IdDonHang = Convert.ToInt32(ds1.Tables[0].Rows[0]["IdDonHang"]);
                        _context.tbldathang.Add(model1);

                        
                        tblgiohang model2 = new tblgiohang();
                        
                        string TenSP = ds2.Tables[0].Rows[i]["TenSanPham"].ToString();
                        model2 = _context.tblgiohang.Where(x => x.TenSanPham == TenSP && x.EmailUser == emailUser).FirstOrDefault();
                        _context.tblgiohang.Remove(model2);
                        
                        _context.SaveChanges();
                       
                    }
                }
                SetAlert("success", "Đặt hàng thành công");

                
            }
            return RedirectToAction("Index");
        }

        public ActionResult OrderLayout()
        {
            if (Session["account"] != null)
            {
                string emailUser = Session["account"].ToString();
                _context = new WebNoiThatDbContext();
                var lstOrder = _context.tbldonhang.Where(x => x.EmailUser == emailUser).ToList();

                List<tbldonhang> Order = new List<tbldonhang>(lstOrder);

                List<tbldathang> LstOrderItem = new List<tbldathang>();
                foreach (var item in Order)
                {
                    SqlConnection conn1 = new SqlConnection(connectionString);
                    //cmd1 = new SqlCommand();

                    string query1 = "SELECT * from tbldathang where EmailUser = '" + emailUser + "' AND IdDonHang = '" + item.ID + "' ";
                    SqlCommand cmd1 = new SqlCommand(query1, conn1);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1);
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            tbldathang OrderItem = new tbldathang();
                            OrderItem.TenSanPham = ds1.Tables[0].Rows[i]["TenSanPham"].ToString();
                            OrderItem.DonGia = Convert.ToInt32(ds1.Tables[0].Rows[i]["DonGia"]);
                            OrderItem.SoLuong = Convert.ToInt32(ds1.Tables[0].Rows[i]["SoLuong"]);
                            OrderItem.IdDonHang = Convert.ToInt32(ds1.Tables[0].Rows[i]["IdDonHang"]);
                            LstOrderItem.Add(OrderItem);

                        }
                    }
                }
                _context = new WebNoiThatDbContext();
                var lstProduct = _context.tblsanpham.ToList();
                ViewBag.lstProduct = lstProduct;
                ViewBag.lstOrderItem = LstOrderItem;
                ViewBag.lstOder = lstOrder;
                return PartialView(lstOrder);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult CancelOrder(int id, string returnUrl)
        {
            _context = new WebNoiThatDbContext();
            var model = _context.tbldonhang.Where(x => x.ID == id).FirstOrDefault();
            model.TrangThai = "đã hủy";
            _context.SaveChanges();
            SetAlert("success", "Hủy đơn hàng thành công");
            return Redirect(returnUrl);
        }
    }
}