using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StaffPortal.Models;
using StaffPortal.Interface;
using StaffPortal.Entities;
using StaffPortal.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace StaffPortal.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class NewsController : BaseController
    {
        private INews _news;

        //private readonly UserManager<ApplicationUser> _userManager;
        public NewsController(INews news)
        {
            _news = news;
            //_userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            var model = await _news.GetAll();

            if (model != null)
                return View(model);
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            
            var createNews = await _news.AddAsync(news);

            if (createNews)
            {
                Alert("News created successfully.", NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("News not created!", NotificationType.error);
            }


            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var editNews = await _news.GetById(id);

            if (editNews == null)
            {
                return RedirectToAction("Index");
            }
            return View(editNews);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(News news)
        {
            //var editNews = await _news.GetById(id);
            var editNews = await _news.Update(news);
            

            if (editNews && ModelState.IsValid)
            {
                //    editNews.Name = news.Name;
                //    context.SaveChanges();
                Alert("News edited successfully!", NotificationType.success);
                return RedirectToAction("Index");
                //return RedirectToAction("Details", new { id = editNews.Id });
            }
            Alert("News not edited!", NotificationType.warning);
            return View();
        }

        //[HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var deleteNews = await _news.Delete(id);
            
            if (deleteNews)
            {
                Alert("News deleted successfully.", NotificationType.success);
                return RedirectToAction("Index");
            }
            Alert("News not deleted!", NotificationType.error);
            return View();
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "News");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
