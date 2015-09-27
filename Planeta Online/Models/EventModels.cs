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
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Lecturer { get; set; }
        public List<ApplicationUser> Listeners { get; set; }
        public DateTime Time { get; set; }
    }

    public class EventViewModel
    {
        [Required]
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
        public virtual string UserId { get; set; }
    }
}