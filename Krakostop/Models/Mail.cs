using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Krakostop.Models
{
    public class Mail
    {
        [Required()]
        [EmailAddress(ErrorMessage = "Tenże email jest niepoprawny")]
        [Display(Name = "Twoj email")]
        public string From { get; set; }

        [Required()]
        [Display(Name = "Imie i nazwisko / pseudonim")]
        public string Name { get; set; }

        public string To { get; set; }

        [Required()]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Required()]
        [Display(Name = "Treść maila")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}