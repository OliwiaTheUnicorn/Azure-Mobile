using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Web.Models
{
    public class SigninVM
    {
        [Required (ErrorMessage = "Usrename is required")]
        public string UserName { get; set; }
    }
}
