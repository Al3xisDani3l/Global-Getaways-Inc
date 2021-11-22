using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using GG.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GG.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System;

namespace GG.WebPageMVC.Controllers
{
 
    [Route("[controller]")]
    public class LikesController : Controller
    {
        
        readonly ApplicationDbContext _context;
        readonly SignInManager<PrivateUser> _signInManager;
        ILogger<LikesController> _logger;


        public LikesController(ApplicationDbContext context, SignInManager<PrivateUser> signInManager,ILogger<LikesController> logger)
        {

            _context = context;
            _logger = logger;
            _signInManager = signInManager;
        }


        // GET: LikesController
        public async Task<ActionResult> Index()
        {
           
                var claimUser = (User as ClaimsPrincipal);

                string userId = claimUser.FindFirst(ClaimTypes.NameIdentifier) is not null ? claimUser.FindFirst(ClaimTypes.NameIdentifier).Value : "";

                var user = await _context.Users.Include(u => u.MyLikes).ThenInclude(m => m.IdTravelPackageNavigation).FirstOrDefaultAsync(u => u.Id == userId);

                var myLikes = user.MyLikes.ToList();

                ViewBag.MyLikes = myLikes;

           
           

           

            return View();
            
        }

        [HttpPost("like")]
        public async Task<IActionResult> Like([FromForm]int packageId,[FromForm] string userId)
        {

            try
            {

                if (!string.IsNullOrEmpty(userId))
                {
                  

             

                    PrivateUser user = await _context.Users.Include(u => u.MyLikes).ThenInclude(m => m.IdTravelPackageNavigation).FirstOrDefaultAsync(u => u.Id == userId);

                    PrivateTravelPackage privateTravel = await _context.TravelPackages.FirstOrDefaultAsync(p => p.Id == packageId);

                    
                    var liked = new LikedPackage() { UserId = user.Id, TravelPackageId = privateTravel.Id };

                    if (user.MyLikes.FirstOrDefault(l => l.TravelPackageId == packageId) != null)
                    {
                        return Ok();
                    }

                    _context.LikedPackages.Add(liked);

                   

                    await _context.SaveChangesAsync();

                    return Ok();



                }
                else
                {
                    return RedirectToPage("./Account/Account", new { ReturnUrl = "./Likes" });

                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "\'{0}\' class on function \'{1}\' ", new[] { nameof(LikesController), nameof(Like) });
                return Redirect("./Error");

            }
        }

        [HttpPost("dislike")]
        public async Task<IActionResult> Dislike([FromBody]int packageId,[FromBody] string userId)
        {

            try
            {

                if (!string.IsNullOrEmpty(userId))
                {
                   

                  

                    PrivateUser user = await _context.Users.Include(u => u.MyLikes).ThenInclude(m => m.IdTravelPackageNavigation).FirstOrDefaultAsync(u => u.Id == userId);

                    var disliked = user.MyLikes.FirstOrDefault(u => u.Id == packageId);

                    if (disliked == null)
                    {
                        return Ok();
                    }

                    _context.Remove(disliked);

                   await  _context.SaveChangesAsync();

                    return Ok();



                }
                else
                {
                    return RedirectToPage("./Account/Account", new { ReturnUrl = "./Likes" });

                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "\'{0}\' class on function \'{1}\' ", new[] { nameof(LikesController), nameof(Dislike) });
                return Redirect("./Error");

            }

        }

       
        
    }
}
