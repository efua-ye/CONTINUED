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
    public class LeaveController : BaseController
    {
        private ILeave _leave;
        private IUserProfile _userProfile;

        private readonly UserManager<ApplicationUser> _userManager;
        public LeaveController(ILeave leave, IUserProfile userProfile, UserManager<ApplicationUser> userManager)
        {
            _leave = leave;
            _userManager = userManager;
            _userProfile = userProfile;
        }
        
        public async Task<IActionResult> Index()
        {
            var model = await _leave.GetAll();

            if (model != null)
                return View(model);
            return View();
        }

        public async Task<IActionResult> PersonalIndex()
        {
            var user = _userManager.GetUserName(User);
            var x = await _userManager.FindByNameAsync(user);
            var model = await _leave.GetPersonalAll(x.Email);

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
        public async Task<IActionResult> Create(Leave leave)
        {
            leave.CreatedBy = _userManager.GetUserName(User);
            
            var x = await _userManager.FindByNameAsync(leave.CreatedBy);

            leave.UserProfileId = _userProfile.GetIdByEmail(x.Email);
            leave.Status = "Pending";
            if(leave.StartDate <= DateTime.UtcNow)
            {
                Alert("Invalid start date!", NotificationType.error);
                return View();
            }
            
            leave.Days = _leave.GetBusinessDays(leave.StartDate, leave.EndDate);
            if ((leave.Days > 0 && leave.Days <= 30) || (leave.Days >= 60 && leave.Reason == "Maternity"))
            {
               
                var createLeave = await _leave.AddAsync(leave);

                if (createLeave)
                {
                    Alert("Leave created successfully.", NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("Duplicate Leave cannot created!", NotificationType.error);
                }
            }
           if(leave.Days <= 0)
            {
                Alert("Invalid start and end date", NotificationType.error);
            }
           else if(leave.Days > 30)
            {
                Alert("Leave days exceeded", NotificationType.error);
            }
        
          
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var editLeave = await _leave.GetById(id);

            if (editLeave == null)
            {
                return RedirectToAction("Index");
            }
            return View(editLeave);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Leave leave)
        {
            //var editLeave = await _leave.GetById(id);
            var editLeave = await _leave.Update(leave);


            if (editLeave && ModelState.IsValid)
            {
                //    editLeave.Name = leave.Name;
                //    context.SaveChanges();
                Alert("Leave edited successfully!", NotificationType.success);
                return RedirectToAction("Index");
                //return RedirectToAction("Details", new { id = editLeave.Id });
            }
            Alert("Leave not edited!", NotificationType.warning);
            return View();
        }


        public async Task<IActionResult> Delete(int id)
        {
            var deleteLeave = await _leave.Delete(id);

            if (deleteLeave)
            {
                Alert("Leave deleted successfully.", NotificationType.success);
                return RedirectToAction("Index");
            }
            Alert("Leave not deleted!", NotificationType.error);
            return View();
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Leave");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
