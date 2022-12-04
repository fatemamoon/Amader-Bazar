using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmaderBazar.Models.Entities
{
    public class CartProduct
    {
        public int PID { get; set; }
        public string PName { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
    }
}