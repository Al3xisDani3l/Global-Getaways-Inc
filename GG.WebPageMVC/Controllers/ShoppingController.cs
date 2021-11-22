using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using GG.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GG.Data;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GG.WebPageMVC.Controllers
{
    [Route("[controller]")]
    public class ShoppingController : Controller
    {

        readonly ApplicationDbContext _dbContex;
        readonly ILogger<ShoppingController> _logger;
        readonly SignInManager<PrivateUser> _signInManager;
        readonly UserManager<PrivateUser>   _userManager;
        public ShoppingController(ApplicationDbContext dbContex, ILogger<ShoppingController> logger, SignInManager<PrivateUser> signInManager, UserManager<PrivateUser> userManager)
        {
            _dbContex = dbContex;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {

            try
            {
               

                    var iduser = (User as ClaimsPrincipal).FindFirst(ClaimTypes.NameIdentifier).Value;

                    var realUser = await _dbContex.Users.Include(u => u.ShoppingCart.Items).FirstOrDefaultAsync(u => u.Id == iduser);



                    if (realUser != null)
                    {

                        if (realUser.ShoppingCart is null)
                        {
                            realUser.ShoppingCart = new ShoppingCart<PrivateTravelPackage>();

                        }

                      

                    }

                    await _dbContex.SaveChangesAsync();

                    return View(realUser.ShoppingCart);

                

              

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\'{0}\' class on function \'{1}\' ", new[] { nameof(ShoppingController), nameof(Index) });
                return Redirect("./Error");
            }
         
            
            
        }

        [HttpPost("addTravel")]
        public async Task<IActionResult> AddTravel([FromForm] int packageId, [FromForm] string userId)
        {



             var user = await _dbContex.Users.Include(u => u.ShoppingCart.Items).FirstOrDefaultAsync(u => u.Id == userId);
             var travel = await _dbContex.TravelPackages.FirstOrDefaultAsync(t => t.Id == packageId);

            if (user is null && travel is not null)
            {
                return NotFound();
            }

            if (user.ShoppingCart is null)
            {
                user.ShoppingCart = new ShoppingCart<PrivateTravelPackage>();
            }

            user.ShoppingCart.Add(travel);

            try
            {


                await _dbContex.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex,$"\'{0}\' class on function \'{1}\' ", new[] {nameof(ShoppingController),nameof(AddTravel)});
                return Redirect("./Error");

            }

            //var cartForUser =   _dbContex.ShoppingCarts.FirstOrDefault(s => s.IdUser == idUser);
            //if (cartForUser is null)
            //{
            //    cartForUser = new ShoppingCart(idUser);

            //    shoppingCarts.Add(cartForUser);
            //}
            //cartForUser.Add(travel);



        }

        [HttpPost("removeTravel")]
        public async Task<IActionResult> RemoveTravel([FromForm] int packageId, [FromForm] string userId)
        {


            var user = await _dbContex.Users.Include(u => u.ShoppingCart.Items).FirstOrDefaultAsync(u => u.Id == userId);
            var travel = await _dbContex.TravelPackages.FirstOrDefaultAsync(t => t.Id == packageId);

            if (user is null && travel is not null)
            {
                return NotFound();
            }

            if (user.ShoppingCart is null)
            {
                user.ShoppingCart = new ShoppingCart<PrivateTravelPackage>();
            }

            user.ShoppingCart.Remove(travel);

            try
            {


                await _dbContex.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"\'{0}\' class on function \'{1}\' ", new[] { nameof(ShoppingController), nameof(AddTravel) });
                return Redirect("./Error");
            }

            //var user = await _users.FindAsync(u => u.Id == idUser);
            //var travel = await _travels.FindAsync(t => t.Id == idTravel);

            //if (user is null && travel is not null)
            //{
            //    return;
            //}

            //var cartForUser = shoppingCarts.FirstOrDefault(s => s.IdUser == idUser);
            //if (cartForUser is null)
            //{
            //    cartForUser = new ShoppingCart(idUser);

            //    shoppingCarts.Add(cartForUser);
            //}
            //cartForUser.Remove(travel);



        }

        [HttpPost("removeAllTravel")]
        public async Task<IActionResult> RemoveAllTravels([FromForm] int packageId, [FromForm] string userId)
        {


            var user = await _dbContex.Users.Include(u => u.ShoppingCart.Items).FirstOrDefaultAsync(u => u.Id == userId);
            var travel = await _dbContex.TravelPackages.FirstOrDefaultAsync(t => t.Id == packageId);

            if (user is null && travel is not null)
            {
                return NotFound();
            }

            if (user.ShoppingCart is null)
            {
                user.ShoppingCart = new ShoppingCart<PrivateTravelPackage>();
            }

            user.ShoppingCart.RemoveAll(travel);

            try
            {


                await _dbContex.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"\'{0}\' class on function \'{1}\' ", new[] { nameof(ShoppingController), nameof(AddTravel) });
                return Redirect("./Error");
            }
        }



    }
}
