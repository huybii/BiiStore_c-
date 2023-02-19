using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebNoiThat.Models
{
    public partial class WebNoiThatDbContext : DbContext
    {
        public WebNoiThatDbContext()
            : base("name=WebNoiThatDbContext")
        {
        }

        public virtual DbSet<tbldangnhap> tbldangnhap { get; set; }
        public virtual DbSet<tbldanhmuc> tbldanhmuc { get; set; }
        public virtual DbSet<tbldatlichtuvan> tbldatlichtuvan { get; set; }
        public virtual DbSet<tbldathang> tbldathang { get; set; }
        public virtual DbSet<tbldonhang> tbldonhang { get; set; }
        public virtual DbSet<tblgiohang> tblgiohang { get; set; }
        public virtual DbSet<tblsanpham> tblsanpham { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
