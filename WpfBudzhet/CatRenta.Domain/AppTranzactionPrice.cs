using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Budzet.Domain
{
    [Table("tblTranzactionPrices")]
    public class AppTranzactionPrice
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreate { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }

    }
}
