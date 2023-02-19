namespace WebNoiThatWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbldangnhap")]
    public partial class tbldangnhap
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string MatKhau { get; set; }

        [Required]
        [StringLength(250)]
        public string TenHienThi { get; set; }

        [Required]
        [StringLength(12)]
        public string SDT { get; set; }

        [Required]
        [StringLength(50)]
        public string PhanQuyen { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayTao { get; set; }

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; }
    }
}
