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
                if (_signInManager.IsSignedIn(User))
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

                return RedirectToPage("./Login", new { ReturnUrl = "./Shopping" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"\'{0}\' class on function \'{1}\' ", new[] { nameof(ShoppingController), nameof(Index) });
                return Redirect("./Error");
            }
         
            
            
        }

        public async Task AddTravel(int idTravel, string idUser)
        {



             var user = await _dbContex.Users.Include(u => u.ShoppingCart.Items).FirstOrDefaultAsync(u => u.Id == idUser);
             var travel = await _dbContex.TravelPackages.FirstOrDefaultAsync(t => t.Id == idTravel);

            if (user is null && travel is not null)
            {
                return;
            }

            if (user.ShoppingCart is null)
            {
                user.ShoppingCart = new ShoppingCart<PrivateTravelPackage>();
            }

            user.ShoppingCart.Add(travel);

            try
            {


                await _dbContex.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex,$"\'{0}\' class on function \'{1}\' ", new[] {nameof(ShoppingController),nameof(AddTravel)});
                
            }

            //var cartForUser =   _dbContex.ShoppingCarts.FirstOrDefault(s => s.IdUser == idUser);
            //if (cartForUser is null)
            //{
            //    cartForUser = new ShoppingCart(idUser);

            //    shoppingCarts.Add(cartForUser);
            //}
            //cartForUser.Add(travel);



        }

        public async Task RemoveTravel(int idTravel, string idUser)
        {

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

        public async Task RemoveAllTravels(int idTravel, string idUser)
        {

        //    var user = await _users.FindAsync(u => u.Id == idUser);
        //    var travel = await _travels.FindAsync(t => t.Id == idTravel);

        //    if (user is null && travel is not null)
        //    {
        //        return;
        //    }

        //    var cartForUser = shoppingCarts.FirstOrDefault(s => s.IdUser == idUser);
        //    if (cartForUser is null)
        //    {
        //        cartForUser = new ShoppingCart(idUser);

        //        shoppingCarts.Add(cartForUser);
        //    }
        //    cartForUser.RemoveAll(travel);
        }



    }
}
