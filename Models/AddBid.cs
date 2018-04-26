using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class AddBid
    {

        [Required]
        [Display(Name = "Date")]
        public int PropertyID { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Bid Amount")]
        public int Bid_Amount { get; set; }
    }
}
