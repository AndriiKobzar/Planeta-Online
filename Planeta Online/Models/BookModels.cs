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
        [Display(Name = "Назва")]
        public String Title { get; set; }
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Display(Name = "Жанр")]
        public string Genre { get; set; }
    }
    public class BookViewModel
    {
        [Required]
        [Display(Name="Назва")]
        public String Title { get; set; }
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Display(Name = "Жанр")]
        public string Genre { get; set; }
    }
}