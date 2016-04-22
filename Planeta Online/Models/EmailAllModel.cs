using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Planeta_Online.Models
{
    public class EmailAllModel
    {
        public string Head { get; set; }
        [AllowHtml]
        public string Body { get; set; }

    }
}
