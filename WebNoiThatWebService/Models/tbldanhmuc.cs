namespace WebNoiThatWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbldanhmuc")]
    public partial class tbldanhmuc
    {
        public int ID { get; set; }

        [Required]
        [StringLength(150)]
        public string TenDanhMuc { get; set; }

        [Required]
        [StringLength(500)]
        public string DuongDanAnh { get; set; }
    }
}
