using System;
using System.Collections.Generic;
using System.Text;

namespace AuthCommon.Models
{
    public class ChangePasswordModel
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
