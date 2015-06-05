using System;
using System.ComponentModel.DataAnnotations;

namespace Krakostop.Models
{
    public class ManagePasswordUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Obecne hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi się posiadać co najmniej {2} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie są zgodne")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email / Identyfikator")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętać Was?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź")]
        [Compare("Password", ErrorMessage = "Nie istnieje zgodność pomiędzy wpisanymi hasłami")]
        public string ConfirmPassword { get; set; }

        public Person Person1 { get; set; }
        public Person Person2 { get; set; }

        [Required]
        [Display(Name = @"Wyrażam zgodę na przetwarzanie danych osobowych podanych w formularzu zgłoszeniowym przez Organizatora wyścigu dla potrzeb realizacji procesu 
            zgłoszeniowego uczestników, prezentacji list startowych i wyników wyścigu oraz do wykorzystania wizerunku dla potrzeb związanych z organizacją i promocją 
            imprezy zgodnie z ustawą z dnia 29 sierpnia 1997r. o ochronie danych osobowych (Dz.U. z 2002r. Nr 101, poz. 926 z poź. zm.). Podanie danych osobowych jest 
            dobrowolne, ale również niezbędne do udziału w wyścigu oraz daje prawo dostępu do ich treści oraz żądania ich poprawienia.")]
        public bool TermsAndRules { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi składać się z co najmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
