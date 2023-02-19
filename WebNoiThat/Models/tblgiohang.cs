namespace WebNoiThat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblgiohang")]
    public partial class tblgiohang
    {
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string EmailUser { get; set; }

        [Required]
        [StringLength(250)]
        public string TenSanPham { get; set; }

        public int DonGia { get; set; }

        public int SoLuong { get; set; }
    }
}
