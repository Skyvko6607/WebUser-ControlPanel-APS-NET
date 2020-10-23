using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuthProject.Models.User
{
    public class UserLoginData
    {
        [Required]
        [DisplayName("Username or Email")]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(64, ErrorMessage = "The password is too long. (Max. 64 characters)")]
        public string Password { get; set; }
    }
}
