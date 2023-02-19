using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNoiThat.Models;

namespace WebNoiThat.Controllers
{
    public class HomeController : Controller
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
        public ActionResult Index()
        {
            /*_context = new WebNoiThatDbContext();
            var lstCategory = _context.tbldanhmuc.ToList();
            var lstProduct = _context.tblsanpham.ToList();
            ViewBag.lstCategory = lstCategory;
            ViewBag.lstProduct = lstProduct;*/

            _context = new WebNoiThatDbContext();
            var lstCategory = _context.tbldanhmuc.ToList();
            ViewBag.lstCategory = lstCategory;

            //var lstProduct = _context.tblsanpham.ToList();
            // ViewBag.lstProduct = lstProduct;
            List<tblsanpham> lstProduct = new List<tblsanpham>();
            foreach (var item in lstCategory)
            {
                SqlConnection conn1 = new SqlConnection(connectionString);
                //cmd1 = new SqlCommand();

                string query1 = "SELECT TOP(4) * from tblSanPham where IdDanhMuc = '"+ item.ID +"'";
                SqlCommand cmd1 = new SqlCommand(query1, conn1);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        tblsanpham Product = new tblsanpham();
                        Product.TenSanPham = ds1.Tables[0].Rows[i]["TenSanPham"].ToString();
                        Product.IdDanhMuc = Convert.ToInt32(ds1.Tables[0].Rows[i]["IdDanhMuc"]);
                        Product.DuongDanAnh = ds1.Tables[0].Rows[i]["DuongDanAnh"].ToString();
                        Product.GiaGoc = Convert.ToInt32(ds1.Tables[0].Rows[i]["GiaGoc"]);
                        Product.GiaKhuyenMai = Convert.ToInt32(ds1.Tables[0].Rows[i]["GiaKhuyenMai"]);
                        Product.MoTaSanPham = ds1.Tables[0].Rows[i]["MoTaSanPham"].ToString();
                        lstProduct.Add(Product);
                    }
                }
            }
            ViewBag.lstProduct = lstProduct;
            return View();
        }
        public ActionResult AddToCart(string ProductName, int price, string returnUrl)
        {
            try
            {
                _context = new WebNoiThatDbContext();

                if (Session["account"] != null)
                {

                    string emailUser = Session["account"].ToString();
                    var checkItem = _context.tblgiohang.Where(x => x.EmailUser == emailUser && x.TenSanPham == ProductName).FirstOrDefault();
                    if (checkItem != null)
                    {
                        checkItem.SoLuong += 1;
                        _context.SaveChanges();
                        SetAlert("success", "Thêm vào giỏ hàng thành công!");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        tblgiohang model = new tblgiohang();
                        model.EmailUser = emailUser;
                        model.TenSanPham = ProductName;
                        model.SoLuong = 1;
                        model.DonGia = price;
                        _context.tblgiohang.Add(model);
                        _context.SaveChanges();
                        SetAlert("success", "Thêm mới vào giỏ hàng thành công!");
                        //return RedirectToAction("Index");
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    SetAlert("error", "Vui lòng đăng nhập để mua sắm!");
                    return RedirectToAction("Index","Login");
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public ActionResult lblCart()
        {
            _context = new WebNoiThatDbContext();
            if (Session["account"] != null)
            {
                string emailUser = Session["account"].ToString();
                var model = _context.tblgiohang.Where(x => x.EmailUser == emailUser).ToList();
                ViewBag.lblCart = model.Count();
            }
            return PartialView();
        }

        //[HttpPost]
        public ActionResult CartSideBar()
        {
            if (Session["account"] != null)
            {
                string emailUser = Session["account"].ToString();
                _context = new WebNoiThatDbContext();
                var cartItem = _context.tblgiohang.Where(x => x.EmailUser == emailUser).ToList();
                var productInfo = _context.tblsanpham.ToList();
                ViewBag.productInfo = productInfo;
                return PartialView(cartItem);
            }
            else
            {

                return null;
            }
        }

        public ActionResult RemoveFromCart(int id, string returnUrl)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var itemRemoved = _context.tblgiohang.Where(x => x.ID == id).FirstOrDefault();
                _context.tblgiohang.Remove(itemRemoved);
                _context.SaveChanges();
                SetAlert("success", "Đã xóa sản phẩm khỏi giỏ hàng");
                //return RedirectToAction("Index");
                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                SetAlert("error", ex.ToString());
                return RedirectToAction("Index");
            }
        }
    }
}