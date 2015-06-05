using System.ComponentModel.DataAnnotations;

namespace Krakostop.Models
{
    public class CreateJoinerViewModel
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

        [Required()]
        [EmailAddress(ErrorMessage = "Tenże email jest niepoprawny")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        public Pair_Joiner Joiner { get; set; }
    }

    public class EditJoinerViewModel
    {
        [Required()]
        [EmailAddress(ErrorMessage = "Tenże email jest niepoprawny")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public Pair_Joiner Joiner { get; set; }
    }
}