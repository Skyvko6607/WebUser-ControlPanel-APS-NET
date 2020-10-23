using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuthProject.Services.Interfaces
{
    public interface IPasswordEncryptionService
    {
        string EncryptPassword(string password);
    }
}
