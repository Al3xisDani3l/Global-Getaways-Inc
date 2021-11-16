
using GG.Core;
using GG.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System;

namespace GG.Infrastructure.Repositories
{
    public class AccountRepository : RepositoryBase<PrivateUser,string>, IAccountRepository
    {


         readonly IMapper _mapper;
       
        public AccountRepository(ApplicationDbContext context,IMapper mapper ) : base(context)
        {
            _mapper = mapper;
        }


        public async Task<PrivateUser> GetLoginByCredentials(UserLogin login)
        {
            if (login is null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var list = await base.GetAllAsync();
            PrivateUser result = null;

            if (login.Email is not null)
            {
                 result = list.FirstOrDefault(x => x.Email == login.Email);
            }
            else if( login.Phone is not null)
            {
                 result = list.FirstOrDefault(x => x.PhoneNumber == login.Phone);
            }
            
           

            return result;
        }

        public async Task<PrivateUser> GetLoginByGoogle(UserLogin login)
        {
            if (login is not null)
            {
              var users =  await this.GetAllAsync();

                var userbyemail = users.FirstOrDefault(u => u.Email == login.Email);
                


                if (userbyemail is not null)
                {
                    
                    return userbyemail;
                }
                return null;

                
            }
            else
            {
                throw new ArgumentNullException(nameof(login));
            }
        }

        public async Task<PrivateUser> RegisterUser(UserSignUp security)
        {
            var map = _mapper.Map<PrivateUser>(security);
               return await base.CreateAsync(map);

        }
    }
}
