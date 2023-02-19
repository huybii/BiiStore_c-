namespace WebNoiThat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbldatlichtuvan")]
    public partial class tbldatlichtuvan
    {
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string TenKhachHang { get; set; }

        public int SDT { get; set; }

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; }
    }
}
