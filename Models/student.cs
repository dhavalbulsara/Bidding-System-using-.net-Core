using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class student
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string fname { get; set; }
        [Required]
        public string mname { get; set; }
        [Required]
        public string lname { get; set; }
        
    }
}
