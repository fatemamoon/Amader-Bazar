using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmaderBazar.BusinessModel
{
    public class LoginValidation
    {
        [Required(ErrorMessage = "Please Enter Your UserID")]
        public string UID { get; set; }
        [Required(ErrorMessage = "Please Enter A Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
        public string AccType { get; set; }
        public string AccStatus { get; set; }
    }
}