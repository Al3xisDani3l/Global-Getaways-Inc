
using GG.Core;
using GG.Database;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
namespace GG.Infrastructure.Repositories
{
    public class AccountRepository : RepositoryBase<PrivateUser>, IAccountRepository
    {


         readonly IMapper _mapper;
        public AccountRepository(GGContext context,IMapper mapper ) : base(context)
        {
            _mapper = mapper;
        }


        public async Task<PrivateUser> GetLoginByCredentials(UserLogin login)
        {
            var list = await base.GetAllAsync();
            var result = list.FirstOrDefault(x => x.Email == login.Email);

            return result;
        }

        public async Task<PrivateUser> RegisterUser(UserSignUp security)
        {
            var map = _mapper.Map<PrivateUser>(security);
               return await base.CreateAsync(map);

        }
    }
}
