using System;
using System.ComponentModel.DataAnnotations;

namespace Planeta_Online.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}
