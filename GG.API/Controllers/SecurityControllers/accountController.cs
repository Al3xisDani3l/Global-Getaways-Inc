using AutoMapper;
using GG.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net;
using IdentityServer4;

namespace GG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class accountController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
     //   private readonly SignInManager<PrivateUser> _signInManager;

        public accountController(IConfiguration configuration, IAccountRepository securityService, IPasswordService passwordService, IMapper mapper)//, SignInManager<PrivateUser> signInManager)
        {
            _configuration = configuration;
            _accountService = securityService;
            _passwordService = passwordService;
            _mapper = mapper;
           // _signInManager = signInManager;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Session>> Login(UserLogin login)
        {
            try
            {

                var user = await _accountService.GetLoginByCredentials(login);

                if (user == null)
                {
                    return NotFound(new { message = $"No existe el usuario con el correo {login.Email}!" });
                }

                var validation = _passwordService.Check(user.PasswordHash, login.Password);


                //if it is a valid user


                if (validation)
                {
                    var session = new Session();

                    session.JWTToken = GenerateToken(user);
                    session.User = _mapper.Map<User>(user);


                    return Ok(session);
                }
                else
                {
                    return Unauthorized(new { message = "La contraseña proporcionada es incorrecta" });
                }
            }
            catch (Exception err)
            {


                if (err.InnerException != null)
                {

                    return BadRequest(new { message = new { message = $"Error: {err.Message}\n Inner Error: {err.InnerException.Message}" } });


                }
                else
                {
                    return BadRequest(new { message = $"Error: {err.Message} " });


                }

            }
        }

        [HttpPost("SingUp")]
        public async Task<ActionResult<Session>> SingUp(UserSignUp userSignUp)
        {
            try
            {


                string password = userSignUp.Password;

                userSignUp.Password = _passwordService.Hash(userSignUp.Password);
                userSignUp.Role = RoleType.User;
                var created = await _accountService.RegisterUser(userSignUp);

                var dto = _mapper.Map<User>(created);
                var token = GenerateToken(created);



                var uri = $"{Request.Scheme}{@"://"}{Request.Host}/users/{dto.Id}";

                return Created(uri, new Session() { JWTToken = token, User = dto });




            }

            catch (Exception err)
            {

                if (err.InnerException != null)
                {
                    return BadRequest(new { message = $"Error: {err.Message}\n Inner Error: {err.InnerException.Message}" });
                }
                else
                {
                    return BadRequest(new { message = $"Error: {err.Message} " });
                }
            }
        }

        [HttpGet("signin-google")]
        public async Task<IActionResult> SigninGoogle(string? returnUrl = null)
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(externallogincallback), new[] { returnUrl, "" }) };
          
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);


        }



        //Callback action to retrive signin user details
        [HttpGet("externallogincallback", Name = "externallogincallback")]
        [AllowAnonymous]
        public async Task<IActionResult> externallogincallback(string? returnUrl = null, string? remoteError = null)
        {

            

              var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result is not null)
            {
                

                if (result.Succeeded)
                {
                    var claims = result.Principal?.Identities?.FirstOrDefault()?.Claims.ToList();

                    string email = claims.FirstOrDefault(c => c.Type.Contains("emailaddress")).Value;
                    string nameidentifier = claims.FirstOrDefault(c => c.Type.Contains("nameidentifier")).Value;
                    string name = claims.FirstOrDefault(c => c.Type.Contains("givenname")).Value;
                    string lastname = claims.FirstOrDefault(c => c.Type.Contains("surname")).Value;
                    string locale = claims.FirstOrDefault(c => c.Type.Contains("locale")).Value;
                    string picture = claims.FirstOrDefault(c => c.Type.Contains("picture")).Value;
                    var login = new UserLogin { Email = email, GoogleId = nameidentifier };

                    var user = await  _accountService.GetLoginByGoogle(login);

                    if (user is not null)
                    {
                        var session = new Session { JWTToken =  GenerateToken(user), User = _mapper.Map<User>(user)};
                        
                    }
                    else
                    {
                        var newUser = new UserSignUp();
                        newUser.Role = RoleType.User;
                        newUser.Name = name;
                        newUser.Lastname = lastname;
                        newUser.Email = email;


                        await _accountService.RegisterUser(newUser);


                    }
                    


                }
            
            }


            return Ok();
            

          
        }

       

        private string GenerateToken(PrivateUser user)
        {
            //Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new[]
            {

                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim("expire", DateTime.Now.AddDays(1).ToString())

            };

            //Payload
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddDays(1)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }


    public class Session
    {
        public string JWTToken { get; set; }

        public User User { get; set; }
    }

}
