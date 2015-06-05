using System.ComponentModel.DataAnnotations;

namespace Krakostop.Models
{
    public enum Sex
    {
        Man = 1,
        Woman = 2
    }

    public class Pair_Joiner
    {
        public int ID { get; set; }

        [Required()]
        [Display(Name = "Imię / Pseudonim")]
        public string Name { get; set; }

        [Required()]
        [Display(Name = "Twój opis")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required()]
        [Display(Name = "Wiek")]
        public int Age { get; set; }

        [Required()]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Aktualne")]
        public bool IsActual { get; set; }

        [Display(Name = "Płeć")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz płeć")]
        public Sex Sex { get; set; }

        public virtual KrakostopUser User { get; set; }
    }
}