using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebNoiThatWebService.Models;

namespace WebNoiThatWebService
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        WebNoiThatDbContext _context;
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<tbldanhmuc> GetCategory()
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var lstCategory = _context.tbldanhmuc.ToList();
                return lstCategory;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [WebMethod]
        public bool AddCategory(tbldanhmuc item , ref string error)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                _context.tbldanhmuc.Add(item);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                error = ex.Message;
                return false;
            }
        }
        [WebMethod]
        public bool EditCategory(int id , string TenDanhMuc, string DuongDanAnh , ref string error)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var item = _context.tbldanhmuc.Where(x => x.ID == id).FirstOrDefault();
                item.TenDanhMuc = TenDanhMuc;
                item.DuongDanAnh = DuongDanAnh;
                //_context.tbldanhmuc.Add(item);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                error = ex.Message;
                return false;
            }
        }
        [WebMethod]
        public bool RemoveCategory(int id , ref string error)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var item = _context.tbldanhmuc.Where(x => x.ID == id).FirstOrDefault();
                _context.tbldanhmuc.Remove(item);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                error = ex.Message;
                return false;
            }
        }

        [WebMethod]
        public List<tbldanhmuc> GetCategoryByName(string keyWord)
        {
            try
            {
                _context = new WebNoiThatDbContext();
                var lstCategory = _context.tbldanhmuc.Where(x => x.TenDanhMuc.Contains(keyWord)).ToList();
                return lstCategory;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
