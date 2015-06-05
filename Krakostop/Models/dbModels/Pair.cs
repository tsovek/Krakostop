using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Krakostop.Models
{
    public class Pair
    {
        public Pair()
        { 
            Persons = new List<Person>();
        }

        public int ID { get; set; }

        public bool Payments { get; set; }

        public virtual ICollection<Person> Persons { get; set; }

        public virtual KrakostopUser User { get; set; }
    }
}