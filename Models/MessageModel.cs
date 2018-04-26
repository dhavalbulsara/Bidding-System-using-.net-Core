using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class MessageModel
    {
        public int ID { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        public int FromUserID { get; set; }

        [Display(Name = "From")]
        public string FromUser { get; set; }

        [Required]
        [Display(Name = "To")]
        public int ToUserID { get; set; }

        [Display(Name = "To")]
        public string ToUser { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Body { get; set; }
    }
}
