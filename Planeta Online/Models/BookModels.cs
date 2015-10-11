using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planeta_Online.Models
{
    public class Book
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
    }
    public class BookViewModel
    {
        [Required]
        public String Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}