using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Krakostop.Models
{

    public enum ShirtSize
    { 
        XS = 1,
        S = 2,
        M = 3,
        L = 4,
        XL = 5, 
        XXL = 6
    }

    public class Person
    {
        public Person()
        { 
            RegisterDate = DateTime.Now;
        }
        
        public int ID { get; set; }

        public int? PairID { get; set; }

        [Required()]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required()]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required()]
        [EmailAddress(ErrorMessage = "Tenże email jest niepoprawny")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required()]
        [Display(Name = "Adres")]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 3)]
        public string Adress { get; set; }

        [Required()]
        [Display(Name = "Miasto")]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 3)]
        public string City { get; set; }

        [Required()]
        [StringLength(11, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 10)]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "PESEL składa się tylko z liczb!")]
        public string PESEL { get; set; }

        [Required()]
        [Display(Name = "Telefon")]
        [StringLength(20, 
            ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", 
            MinimumLength = 7)]
        public string Phone { get; set; }

        [Required()]
        [Display(Name = "Nr dowodu osobistego")]
        [StringLength(12, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 6)]
        public string CardID { get; set; }

        [Display(Name = "Powrót autokarem?")]
        public bool Autocar { get; set; }

        [Required()]
        [Display(Name = "Zobowiązuję się do wykupienia ubezpieczenia na wyjazd/już takie posiadam.")]
        public bool Insurance { get; set; }

        [Required()]
        [Display(Name = "Rozmiar koszulki")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz poprawny rozmiar")]
        public ShirtSize Size { get; set; }

        [Required()]
        [Display(Name = "Imię i nazwisko bliskiej osoby")]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 3)]
        public string ClosedPersonName { get; set; }

        [Required()]
        [Display(Name = "Telefon bliskiej osoby")]
        [StringLength(20, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 8)]
        public string ClosedPersonPhone { get; set; }

        public bool AutocarPayments { get; set; }

        public DateTime RegisterDate { get; set; }
        public virtual  Pair Pair { get; set; }
    }
}