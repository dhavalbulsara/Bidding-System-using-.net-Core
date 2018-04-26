using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class BidModel
    {
        public int ID { get; set; }

        [Display(Name = "Date")]
        public DateTime BidDate { get; set; }

        [Display(Name = "Date")]
        public int PropertyID { get; set; }

        [Display(Name = "User")]
        public int UserID { get; set; }

        [Display(Name = "User")]
        public string Username { get; set; }

        [Display(Name = "Bid Amount")]
        public int Bid_Amount { get; set; }
    }
}
