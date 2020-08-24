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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace StaffPortal.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class SalaryController : BaseController

    {
        private ISalary _salary;
        private IUserProfile _userProfile;

        private readonly UserManager<ApplicationUser> _userManager;
        public SalaryController(ISalary salary, IUserProfile userProfile, UserManager<ApplicationUser> userManager)
        {
            _salary = salary;
            _userProfile = userProfile;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _salary.GetAll();

            if (model != null)
                return View(model);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
           
         
            var user = await _userProfile.GetAll();


            var userList = user.Select(d => new SelectListItem()
            {
                Value = d.Id.ToString(),
                Text = d.Email
            });
            ViewBag.user = userList;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Salary salary)
        {
            salary.CreatedBy = _userManager.GetUserName(User);
            salary.DateCreated = DateTime.Now;
            var createSalary = await _salary.AddAsync(salary);

            //if (createSalary)
            //{
            //    return RedirectToAction("Index");
            //}

            if (createSalary)
            {
                Alert("Salary created successfully😃.", NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("Salary not created😔!", NotificationType.error);
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var editSalary = await _salary.GetById(id);

            if (editSalary == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userProfile.GetAll();


            var userList = user.Select(d => new SelectListItem()
            {
                Value = d.Id.ToString(),
                Text = d.Email
            });
            ViewBag.user = userList;
            return View(editSalary);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Salary salary)
        {
            salary.CreatedBy = _userManager.GetUserName(User);
            salary.DateCreated = DateTime.Now;
            var editSalary = await _salary.Update(salary);

            if (editSalary && ModelState.IsValid)
            {
                Alert("Salary edited successfully😃.", NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("Salary not edited😔.", NotificationType.error);
            }
            return View();
        }


        public async Task<IActionResult> Delete(int id)
        {
            var deleteSalary = await _salary.Delete(id);
            if (deleteSalary)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Salary");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
