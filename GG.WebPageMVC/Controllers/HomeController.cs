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

namespace GG.WebPageMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRepository<PrivateTravelPackage, int> _repository;

        private readonly IMapper _mapper;

        public HomeController(IRepository<PrivateTravelPackage,int> repository,ILogger<HomeController> logger,IMapper mapper)
        {
            _mapper = mapper;   
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            //Datos de prueba

            List<PrivateTravelPackage> privateTravelPackages = new List<PrivateTravelPackage>();

            privateTravelPackages.AddRange(
                new[]
                {
                    new PrivateTravelPackage(){ Country = "China",
                        PathingImage = "https://superhistoria.pl/_thumb/12/24/7b68d5c149xxe73f15a82fb0a533.jpeg",
                        Price = 1807.3M, 
                        Labels = "China; Gran Muralla; Muralla; Gran Muralla China;",
                        Review = "La Gran Muralla China es una antigua fortificación china,construida y reconstruida entre el siglo v a. C. y el siglo xvi para proteger la frontera norte del Imperio chino durante las sucesivas dinastías imperiales de los ataques de los nómadas xiongnu de Mongolia y Manchuria.",
                        Punctuation = 1
                    }
                      
                }

                );

            ViewBag.Travels = privateTravelPackages;

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
