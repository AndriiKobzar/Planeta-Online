using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planeta_Online.Models
{
    public class TestMailModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Head { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}