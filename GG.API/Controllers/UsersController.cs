using AutoMapper;
using GG.Api.Filters;
using GG.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GG.Api.Controllers
{
    /// <summary>
    /// Administra toda la logica de los usuarios
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : GenericController<User, PrivateUser,string>
    {
        

        public UsersController(IRepository<PrivateUser,string> repository, IMapper mapper) : base(repository, mapper)
        {
            
        }
    }
}
