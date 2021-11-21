using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using GG.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GG.WebPageMVC.Controllers
{
    public class LikesController : Controller
    {

        IRepository<PrivateUser, string> _users;
        IRepository<LikedPackage, int> _likes;
        ILogger<LikesController> _logger;

        public LikesController(IRepository<PrivateUser, string> users, IRepository<LikedPackage, int> likes, ILogger<LikesController> logger)
        {
            _users = users;
            _likes = likes;
            _logger = logger;
        }


        // GET: LikesController
        public async Task<ActionResult> Index(string? idUser)
        {
            var user = await _users.FindByKeyAsync(idUser);

            if (user is not null)
            {


            }


            return View();
            
        }

       
      

        // GET: LikesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LikesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LikesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LikesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LikesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LikesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LikesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
