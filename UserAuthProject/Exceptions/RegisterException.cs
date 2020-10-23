using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthProject.Models.User;

namespace UserAuthProject.Exceptions
{
    public class RegisterException : Exception
    {
        public RegisterException(UserRegisterData userRegisterData, string message) :
            base($"An error occurred trying to register user: {userRegisterData.Username}. Reason: " + message)
        {
        }
    }
}