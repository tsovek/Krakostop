using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Krakostop.Models
{
    public class Content
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public virtual Person Person { get; set; }
    }
}