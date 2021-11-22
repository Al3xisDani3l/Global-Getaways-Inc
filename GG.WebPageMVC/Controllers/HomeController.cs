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

        private readonly IRecommender _recommender;

        public HomeController(ApplicationDbContext dbContext,IRepository<PrivateTravelPackage,int> repository, IRepository<PrivateRating, int> ratings, ILogger<HomeController> logger,IMapper mapper, SignInManager<PrivateUser> signInManager, UserManager<PrivateUser> userManager, IRecommender recommender)
        {
            _mapper = mapper;   
            _repository = repository;
            _logger = logger;
            _ratings = ratings;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _recommender = recommender;
        }


        public async Task<IActionResult> Index()
        {


            //Datos de prueba


            //obtenemos los paquetes de la base de datos;
            List<PrivateTravelPackage> packages = _dbContext.TravelPackages.Include(t => t.Ratings).ToList();

         
            //agregamos los paquetes del swipper
            ViewBag.Swipper = packages.Take(6).ToList();

            
            //Los paquetes con mas me gusta del mes
            ViewBag.Travels = packages.OrderByDescending(t => t.PunctuationAverage).Take(20).ToList();

            //Los reviews de los usuarios
            ViewBag.Ratings = _dbContext.Ratings.Include(u => u.IdUserNavigation).OrderBy(u => u.PostingDate).Take(20).ToList();
          
            //obtenemos el usuario logeado si lo hay
            var claimUser = (User as ClaimsPrincipal);

            string userId = claimUser.FindFirst(ClaimTypes.NameIdentifier) is not null ? claimUser.FindFirst(ClaimTypes.NameIdentifier).Value : "";

            ViewBag.CurrentIdUser = userId;

            if (!String.IsNullOrEmpty(userId))
            {
                //Paquetes sacados del recomender sistem para el usuario actual
                ViewBag.BestPackagesForUser = _recommender.PredictForUser(userId).ToList();
            }
            else
            {
                //paquetes genericos en dado caso que no haya usuario logeado
                ViewBag.BestPackagesForUser = packages.OrderByDescending(t => t.PunctuationAverage).Take(10).ToList();
            }
           

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


      
    }
}
