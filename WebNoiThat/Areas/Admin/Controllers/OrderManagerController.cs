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
    public class OrderManagerController : Controller
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
        // GET: Admin/OrderManager
        public ActionResult Index(string OrderStatus, FormCollection collection)
        {
            if (Session["account"] != null)
            {
                string emailUser = Session["account"].ToString();
                _context = new WebNoiThatDbContext();
                var lstOrder = new List<tbldonhang>();
                if (OrderStatus == "tất cả")
                {
                    lstOrder = _context.tbldonhang.OrderByDescending(x => x.NgayDat).ToList();

                }
                else if(OrderStatus == "thất bại")
                {
                    lstOrder = _context.tbldonhang.Where(x => x.TrangThai == "đã hủy" || x.TrangThai == "giao thất bại").OrderByDescending(x => x.NgayDat).ToList();
                    
                }
                else
                {
                    lstOrder = _context.tbldonhang.Where(x => x.TrangThai == OrderStatus).OrderByDescending(x => x.NgayDat).ToList();
                    if (OrderStatus == "chờ xác nhận")
                    {
                        ViewBag.OrderBtn = "ChuyenDangGiao";
                    }else if(OrderStatus == "đang giao")
                    {
                        ViewBag.OrderBtn = "ChuyenDaGiao";
                    }
                }

                if (collection["txtSearch"] != null)
                {
                    string keyWord = collection["txtSearch"].ToString();
                    ViewBag.keyword = keyWord;
                    lstOrder = _context.tbldonhang.Where(x=>x.TenNguoiNhan.Contains(keyWord)).OrderByDescending(x => x.NgayDat).ToList();
                }
                List<tbldonhang> Order = new List<tbldonhang>(lstOrder);

                List<tbldathang> LstOrderItem = new List<tbldathang>();
                foreach (var item in Order)
                {
                    SqlConnection conn1 = new SqlConnection(connectionString);
                    //cmd1 = new SqlCommand();

                    string query1 = "SELECT * from tbldathang where EmailUser = '" + item.EmailUser + "' AND IdDonHang = '" + item.ID + "' ";
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
                return View(lstOrder);
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
        public ActionResult ChuyenSanSangGiao(FormCollection collection, string returnUrl)
        {
            
            try
            {
                if (collection["txtAction"].ToString() == "đang giao")
                {
                    if (collection["ckb"] != null)
                    {
                        foreach (var item in collection["ckb"])
                        {
                            var ckb1 = item.ToString();
                            if (ckb1 != ",")
                            {
                                int ckb2 = Convert.ToInt32(ckb1);
                                //tbldonhang order = new tbldonhang();
                                _context = new WebNoiThatDbContext();
                                var model = _context.tbldonhang.Where(x => x.ID == ckb2).FirstOrDefault();
                                model.TrangThai = collection["txtAction"].ToString();
                                _context.SaveChanges();
                            }



                        }
                        SetAlert("success", "Thao tác thành công");
                    }
                }else if(collection["txtAction"].ToString() == "đã giao")
                {
                    if (collection["ckb"] != null)
                    {
                        foreach (var item in collection["ckb"])
                        {
                            var ckb1 = item.ToString();
                            if (ckb1 != ",")
                            {
                                int ckb2 = Convert.ToInt32(ckb1);
                                //tbldonhang order = new tbldonhang();
                                _context = new WebNoiThatDbContext();
                                var model = _context.tbldonhang.Where(x => x.ID == ckb2).FirstOrDefault();
                                model.TrangThai = collection["txtAction"].ToString();
                                _context.SaveChanges();
                            }



                        }
                        SetAlert("success", "Thao tác thành công");
                    }
                }
                else if(collection["txtAction"].ToString() == "giao thất bại")
                {
                    if (collection["ckb"] != null)
                    {
                        foreach (var item in collection["ckb"])
                        {
                            var ckb1 = item.ToString();
                            if (ckb1 != ",")
                            {
                                int ckb2 = Convert.ToInt32(ckb1);
                                //tbldonhang order = new tbldonhang();
                                _context = new WebNoiThatDbContext();
                                var model = _context.tbldonhang.Where(x => x.ID == ckb2).FirstOrDefault();
                                model.TrangThai = collection["txtAction"].ToString();
                                _context.SaveChanges();
                            }



                        }
                        SetAlert("success", "Thao tác thành công");
                    }
                }
                /*if (collection["ckb"] != null)
                {
                    foreach (var item in collection["ckb"])
                    {
                        var ckb1 = item.ToString();
                        if (ckb1 != ",")
                        {
                            int ckb2 = Convert.ToInt32(ckb1);
                            //tbldonhang order = new tbldonhang();
                            _context = new WebNoiThatDbContext();
                            var model = _context.tbldonhang.Where(x => x.ID == ckb2).FirstOrDefault();
                            model.TrangThai = collection["txtAction"].ToString();
                            _context.SaveChanges();
                        }
                        
                        
                        
                    }
                    SetAlert("success", "Thao tác thành công");
                }*/
                //return RedirectToAction("Index", "OrderManager");
                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "OrderManager");
            }
        }
    }
}