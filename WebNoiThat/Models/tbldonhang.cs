namespace WebNoiThat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbldonhang")]
    public partial class tbldonhang
    {
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string EmailUser { get; set; }

        public int TongTien { get; set; }

        [Required]
        [StringLength(250)]
        public string TenNguoiNhan { get; set; }

        public int SDT { get; set; }

        [Required]
        [StringLength(500)]
        public string ThanhPho { get; set; }

        [Required]
        [StringLength(100)]
        public string Huyen { get; set; }

        [Required]
        [StringLength(100)]
        public string Xa { get; set; }

        [Required]
        [StringLength(500)]
        public string DiaChiChiTiet { get; set; }

        public DateTime NgayDat { get; set; }

        [Required]
        [StringLength(100)]
        public string TrangThai { get; set; }
    }
}
