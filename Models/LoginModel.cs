using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        [MaxLength(20), MinLength(5)]
        public string username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }


        public int id { get; set; }
    }
}
