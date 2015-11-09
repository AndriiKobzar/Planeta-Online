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

        [Display(Name = "Початок")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime From { get; set; }

        [Display(Name = "Кінець")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime Till { get; set; }
        [Display(Name="Вкладення")]
        public string Attachments { get; set; }
    }

    public class EventViewModel
    {
        [Required]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Початок")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime From { get; set; }
        [Display(Name = "Кінець")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Till { get; set; }
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
        [Display(Name = "Початок")]
        [DataType(DataType.DateTime)]
        public DateTime From { get; set; }


        [Display(Name = "Закінчення")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Till { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string CreatorEmail { get; set; }

        [Required]
        [Display(Name = "Ім'я та прізвище")]
        public string CreatorName { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        public string CreatorPhone { get; set; }

        [Display(Name="Вкладення")]
        public string Attachments { get; set; }
    }
    public class EventApplicationViewModel
    {
        [Required]
        [Display(Name = "Назва заходу")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Дата початку")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "Час початку")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public TimeSpan FromTime { get; set; }

        [Display(Name = "Дата закінчення")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime TillDate { get; set; }

        [Required]
        [Display(Name = "Час закінчення")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public TimeSpan TillTime { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string CreatorEmail { get; set; }

        [Required]
        [Display(Name = "Ім'я та прізвище")]
        public string CreatorName { get; set; }

        [Required]
        [Display(Name="Телефон")]
        [DataType(DataType.PhoneNumber)]
        public int CreatorPhone { get; set; }

        [Required]
        [Display(Name="Я погоджуюсь на обробку персональних даних.")]
        public bool Agreed { get; set; }
    }
    public class EventViewModelForAdmin : EventViewModel
    {
        public int Id { get; set; }
        [Display(Name="Відвідувачі")]
        public int Visitors { get; set; }
    }

    public class VisitorsViewModel
    {
        public string EventName { get; set; }
        public List<string> Visitors { get; set; }
    }
}