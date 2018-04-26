using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class PropertyListModel
    {
        public int ID { get; set; }

        [Display(Name = "Date")]
        public DateTime PostingDate { get; set; }
        public int UserID { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Minimum Price")]
        public int MinPrice { get; set; }

        [Display(Name = "Minimum Price")]
        public int CurrentPrice { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        public string status { get; set; }

    }
}
