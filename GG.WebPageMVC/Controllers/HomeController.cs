using AutoMapper;
using GG.WebPageMVC.Models;
using GG.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GG.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GG.WebPageMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRepository<PrivateTravelPackage, int> _repository;

        private readonly IRepository<PrivateRating, int> _ratings;

        private readonly ApplicationDbContext _dbContext;

        readonly SignInManager<PrivateUser> _signInManager;

        readonly UserManager<PrivateUser> _userManager;

        private readonly IMapper _mapper;

        public HomeController(ApplicationDbContext dbContext,IRepository<PrivateTravelPackage,int> repository, IRepository<PrivateRating, int> ratings, ILogger<HomeController> logger,IMapper mapper, SignInManager<PrivateUser> signInManager, UserManager<PrivateUser> userManager)
        {
            _mapper = mapper;   
            _repository = repository;
            _logger = logger;
            _ratings = ratings;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {


            //Datos de prueba



            List<PrivateTravelPackage> packages = _dbContext.TravelPackages.Include(t => t.Ratings).ToList();

         
            ViewBag.Swipper = packages.Take(5).ToList();


            ViewBag.Travels = packages.OrderByDescending(t => t.PunctuationAverage).Take(20).ToList();

            ViewBag.Ratings = _dbContext.Ratings.Include(u => u.IdUserNavigation).OrderBy(u => u.PostingDate).Take(20).ToList();
            ViewBag.BestPackages = packages.OrderByDescending(t => t.PunctuationAverage).Take(10).ToList();
            ViewBag.CurrentIdUser = (User as ClaimsPrincipal).FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> LikedItem(int id)
        {


            return NotFound();

        }
    }
}
