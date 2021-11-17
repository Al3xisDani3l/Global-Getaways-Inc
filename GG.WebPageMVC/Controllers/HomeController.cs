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

             var travels =   await _repository.GetAllAsync();

            ViewBag.Swipper = travels.Take(5).ToList();

            ViewBag.Travels = travels.Take(10).ToList();

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
