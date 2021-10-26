using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GG.Core
{
    public interface IAccountRepository
    {
        Task<PrivateUser> GetLoginByCredentials(UserLogin userLogin);

        Task<PrivateUser> RegisterUser(UserSignUp security);

    }
}
