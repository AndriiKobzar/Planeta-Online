using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}
