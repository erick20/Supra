using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supra.Models.SignUpModels
{
    public class SignUpModel
    {
        
    }
    public class PhoneNumber
    {
        public string Phone { get; set; }
    }
    public class ConfirmSmsCode
    {
        public string Phone { get; set; }

        public string Code { get; set; }
    }
    public class CreatePassword
    {
        public string Password { get; set; }

        //public string ConfirmPassword { get; set; }
    }
}
