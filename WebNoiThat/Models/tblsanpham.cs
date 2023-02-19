namespace WebNoiThat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblsanpham")]
    public partial class tblsanpham
    {
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string TenSanPham { get; set; }

        public int IdDanhMuc { get; set; }

        [Required]
        [StringLength(500)]
        public string DuongDanAnh { get; set; }

        public int GiaGoc { get; set; }

        public int GiaKhuyenMai { get; set; }

        [Required]
        [StringLength(1000)]
        public string MoTaSanPham { get; set; }
    }
}
