using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmaderBazar.BusinessModel
{
    public class EditProfile
    {
        public string UID { get; set; }
        [Required(ErrorMessage = "Please Enter Your Name")]
        [StringLength(50, ErrorMessage = "Name should not be so long")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Your Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter An Address")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Maximum 30 characters")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please Enter A Contact Number")]
        [StringLength(14, ErrorMessage = "Invalid Phone Number")]
        public string Contact { get; set; }
    }
}