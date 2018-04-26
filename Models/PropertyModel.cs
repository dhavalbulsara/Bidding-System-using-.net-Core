using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class PropertyModel
    {
        public int ID { get; set; }
        public DateTime PostingDate { get; set; }
        public int UserID { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Minimum Price is required")]
        [Display(Name = "Minimum Price")]
        public int MinPrice { get; set; }

        [Required(ErrorMessage = "Please Select Type")]
        [Display(Name = "Type")]
        public string Type { get; set; }

        public string status { get; set; }

    }
}
