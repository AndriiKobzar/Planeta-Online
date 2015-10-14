using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeta_Online.Models
{
    public class Event
    {      
        public int Id { get; set; }
        [Display(Name="Назва")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Опис")]
        public string Description { get; set; }
        public List<ApplicationUser> Listeners { get; set; }
        [Display(Name = "Дата та час")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }
        public string Attachments { get; set; }
    }

    public class EventViewModel
    {
        [Required]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        public string Lecturer { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        public DateTime Time { get; set; }
    }
    public class EventRegistration
    {
        public int Id { get; set; }
        public virtual int EventId { get; set; }
        public string VisitorName { get; set; }
        public string VisitorEmail { get; set; }
    }
    public class EventRegistrationViewModel
    {
        [Required]
        [Display(Name = "Ім'я та прізвище")]
        public string VisitorName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string VisitorEmail { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; }
    }
    public class EventApplication
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Назва заходу")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Дата")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Time { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string CreatorEmail { get; set; }

        [Required]
        [Display(Name = "Ім'я та прізвище")]
        public string CreatorName { get; set; }

        public string Attachments { get; set; }
    }
}