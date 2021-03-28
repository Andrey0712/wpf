using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budzet.Domain
{
    [Table("tblUser")]
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        public DateTime Tranіaction { get; set; }
        [Required, StringLength(4000)]
        public string Details { get; set; }
        public bool DebitKredit { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
    }
}
