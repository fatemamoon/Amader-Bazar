using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmaderBazar.Models
{
    public class BuyerHomeBind
    {
        public string Search { get; set; }
        public string SearchType { get; set; }
        public string Filter { get; set; }
        public string SearchMsg { get; set; }
        public int TotalProducts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}