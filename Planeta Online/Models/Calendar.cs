using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeta_Online.Models
{
    class Day
    {
        
        public List<Event> Events { get; set; }
    }
    class Calendar
    {
        public List<Day> Days { get; set; }
        public Calendar(List<Event> events)
        {
            foreach(Event e in events)
            {
                
            }
        }
    }
}
