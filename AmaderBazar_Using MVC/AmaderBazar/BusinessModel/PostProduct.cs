using AmaderBazar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmaderBazar.BusinessModel
{
    public class PostProduct
    {
        public string UID { get; set; }
        [Required]
        public string CatName { get; set; }
        [Required(ErrorMessage = "Please Enter The Product Name")]
        [StringLength(20, ErrorMessage = "Product Name should not be so long")]
        public string PName { get; set; }
        [Required(ErrorMessage = "Please provide the quantity of the Product")]
        public int Qty { get; set; }
        [Required(ErrorMessage = "Please provide the price of each piece")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Please Enter An Address")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Please keep the product description within 20 and 500 characters")]
        public string Descr { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}