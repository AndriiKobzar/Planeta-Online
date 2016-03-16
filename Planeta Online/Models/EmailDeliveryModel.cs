using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Planeta_Online.Models
{
    public class EmailDeliveryModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        [Required]
        [Display(Name ="Заголовок")]
        public string Head { get; set; }
        [Required]
        [Display(Name = "Повідомлення")]
        [AllowHtml]
        public string Body { get; set; }
    }
}
