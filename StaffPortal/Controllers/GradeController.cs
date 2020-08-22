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
    [Authorize(Roles = "Admin")]
    public class GradeController : BaseController
    {
        private IGrade _grade;
        private IUserProfile _userProfile;
        private readonly UserManager<ApplicationUser> _userManager;
        public GradeController(IGrade grade, IUserProfile userProfile, UserManager<ApplicationUser> userManager)
        {
            _userProfile = userProfile;
            _grade = grade;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _grade.GetAll();

            if (model != null)
                return View(model);
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Grade grade)
        {
            grade.CreatedBy = _userManager.GetUserName(User);

            var createGrade = await _grade.AddAsync(grade);

            if (createGrade)
            {
                Alert("Grade created successfully.", NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("Grade not created!", NotificationType.error);
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var editGrade = await _grade.GetById(id);

            if (editGrade == null)
            {
                return RedirectToAction("Index");
            }
            return View(editGrade);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Grade grade)
        {
            //var editGrade = await _Grade.GetById(id);
            var editGrade = await _grade.Update(grade);
            var edituserGrade = await _userProfile.UpdateGrade(grade);


            if (edituserGrade && editGrade && ModelState.IsValid)
            {
                
                Alert("Grade edited successfully!", NotificationType.success);
                return RedirectToAction("Index");
                //return RedirectToAction("Details", new { id = editGrade.Id });
            }
            Alert("Grade not edited!", NotificationType.warning);
            return View();
        }

        //[HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var deleteGrade = await _grade.Delete(id);

            if (deleteGrade)
            {
                Alert("Grade deleted successfully.", NotificationType.success);
                return RedirectToAction("Index");
            }
            Alert("Grade not deleted!", NotificationType.error);
            return View();
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Grade");
        }
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
