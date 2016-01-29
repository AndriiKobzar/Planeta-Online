using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Planeta_Online.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        [Required]
        [DisplayName("Назва")]       
        public string Title { get; set; }

        [Required]
        [DisplayName("Текст")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
    public class BlogPostViewModel
    {
        [Required]
        [DisplayName("Назва")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Текст")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}
