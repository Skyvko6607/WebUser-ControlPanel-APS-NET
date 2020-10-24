using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuthProject.Services.Interfaces
{
    public interface IPasswordEncryptionService
    {
        string CreateHash(string password, string salt);
        string CreateSalt();
    }
}
