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
    public class DepartmentController : BaseController

    {
        private IDepartment _department;
        private IFaculty _faculty;

        private readonly UserManager<ApplicationUser> _userManager;
        public DepartmentController(IDepartment department, IFaculty faculty, UserManager<ApplicationUser> userManager)
        {
            _department = department;
            _userManager = userManager;
            _faculty = faculty;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _department.GetAll();

            if (model != null)
                return View(model);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var fac = await _faculty.GetAll();
            var FacList = fac.Select(f => new SelectListItem()
            {
                Value = f.Id.ToString(),
                Text = f.Name + " - " +  f.Code
            });


            ViewBag.fac = FacList;


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            department.CreatedBy = _userManager.GetUserName(User);
            department.DateCreated = DateTime.Now;
            var createDepartment = await _department.AddAsync(department);

            //if (createDepartment)
            //{
            //    return RedirectToAction("Index");
            //}

            if (createDepartment)
            {
                Alert("Department created successfully😃.", NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("Department not created😔!", NotificationType.error);
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var editDepartment = await _department.GetById(id);

            if (editDepartment == null)
            {
                return RedirectToAction("Index");
            }

            var fac = await _faculty.GetAll();
            var FacList = fac.Select(f => new SelectListItem()
            {
                Value = f.Id.ToString(),
                Text = f.Name
            });


            ViewBag.fac = FacList;
            return View(editDepartment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            department.CreatedBy = _userManager.GetUserName(User);
            department.DateCreated = DateTime.Now;
            var editDepartment = await _department.Update(department);

            if (editDepartment && ModelState.IsValid)
            {
                Alert("Department edited successfully😃.", NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("Department not edited😔.", NotificationType.error);
            }
            return View();
        }


        public async Task<IActionResult> Delete(int id)
        {
            var deleteDepartment = await _department.Delete(id);
            if (deleteDepartment)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Department");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
