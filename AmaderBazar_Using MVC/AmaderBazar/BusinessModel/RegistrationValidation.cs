using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AmaderBazar.BusinessModel
{
    public class RegistrationValidation
    {
        public int ID { get; set; }
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
        [Required(ErrorMessage = "Please Select an Account Type")]
        public string AccType { get; set; }
        public string AccStatus { get; set; }
        [Required(ErrorMessage = "Please Enter A Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Retype The Password")]
        [Compare("Password", ErrorMessage = "Password Doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}