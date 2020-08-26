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
using StaffPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace StaffPortal.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class SalaryController : BaseController

    {
        private ISalary _salary;
        private StaffPortalDataContext _context;
        private IUserProfile _userProfile;

        private readonly UserManager<ApplicationUser> _userManager;
        public SalaryController(ISalary salary, IUserProfile userProfile, StaffPortalDataContext context, UserManager<ApplicationUser> userManager)
        {
            _salary = salary;
            _context = context;
            _userProfile = userProfile;
            _userManager = userManager;
        }

        //public async Task<IActionResult> Index()
        //{

        //    var model = await _salary.GetAll();

        //    if (model != null)
        //        return View(model);
        //    return View();
        //}
        //public async Task<IActionResult> test(string searchString)
        //{
        //    var salary = from m in _context.Salaries
        //                 select m;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        //salary = salary.Where(s => s.Month.Contains(searchString) || s.Year.Contains(searchString) || s.DateCreated.ToString().Contains(searchString) || s.CreatedBy.Contains(searchString));
        //        salary = salary.Where(s => s.Month.Contains(searchString) );

        //    }

        //    return View(await salary.ToListAsync());
        //}


        //public async Task<IActionResult> test(string salaryMonth, string salaryYear, string searchString)
        //{

        //    IQueryable<string> monthQuery = from m in _context.Salaries
        //                                    orderby m.Month
        //                                    select m.Month;

        //    IQueryable<string> yearQuery = from m in _context.Salaries
        //                                    orderby m.Year
        //                                    select m.Year;

        //    var salaries = from m in _context.Salaries
        //                 select m;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        salaries = salaries.Where(s => s.Month.Contains(searchString) || s.Year.Contains(searchString) || s.DateCreated.ToString().Contains(searchString) || s.CreatedBy.Contains(searchString));
        //    }

        //    if (!string.IsNullOrEmpty(salaryMonth))
        //    {
        //        salaries = salaries.Where(x => x.Month == salaryMonth);
        //    }

        //    if (!string.IsNullOrEmpty(salaryYear))
        //    {
        //        salaries = salaries.Where(x => x.Year == salaryYear);
        //    }

        //    var salaryMonthVM = new SalaryMonthViewModel
        //    {
        //        Months = new SelectList(await monthQuery.Distinct().ToListAsync()),
        //        Years = new SelectList(await yearQuery.Distinct().ToListAsync()),
        //        Sals = await salaries.ToListAsync()
        //    };

        //    return View(salaryMonthVM);



        //}
        public async Task<IActionResult> Index(string salaryMonth, string salaryYear, string searchString, string Sorting_Order)
        {
            ViewBag.SortingMonth = String.IsNullOrEmpty(Sorting_Order) ? "Month_Desc" : "";
            ViewBag.SortingYear = Sorting_Order == "Year" ? "Year_Desc" : "Year";
            ViewBag.SortingDate = Sorting_Order == "Date_created" ? "Date_Desc" : "Date_created";
            ViewBag.SortingCreate = Sorting_Order == "CreatedBy" ? "CreatedBy_Desc" : "CreatedBy";
            ViewBag.SortingId = Sorting_Order == "ID" ? "ID_Desc" : "ID";
            ViewBag.Sortinguser = Sorting_Order == "UserProfile" ? "UserProfile_Desc" : "UserProfile";

            IQueryable<string> monthQuery = from m in _context.Salaries
                                            orderby m.Month
                                            select m.Month;

            IQueryable<string> yearQuery = from m in _context.Salaries
                                           orderby m.Year
                                           select m.Year;

            var salaries = from m in _context.Salaries
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                salaries = salaries.Where(s => s.Month.Contains(searchString) || s.Year.Contains(searchString) || s.DateCreated.ToString().Contains(searchString) || s.CreatedBy.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(salaryMonth))
            {
                salaries = salaries.Where(x => x.Month == salaryMonth);
            }

            if (!string.IsNullOrEmpty(salaryYear))
            {
                salaries = salaries.Where(x => x.Year == salaryYear);
            }
            var salariesList = new List<Salary>();
            switch (Sorting_Order)
            {
                case "Month_Asc":
                    //students = students.OrderByDescending(stu => stu.FirstName);
                    salariesList = salaries.OrderBy(sal => sal.Month).ToList();
                    break;

                case "Month_Desc":
                    salariesList = salaries.OrderByDescending(sal => sal.Month).ToList();
                    break;

                case "ID":

                    salariesList = salaries.OrderBy(sal => sal.Id).ToList();
                    break;

                case "ID_Desc":
                    salariesList = salaries.OrderByDescending(sal => sal.Id).ToList();
                    break;

                case "UserProfile":

                    salariesList = salaries.OrderBy(sal => sal.UserProfileId).ToList();
                    break;

                case "UserProfile_Desc":
                    salariesList = salaries.OrderByDescending(sal => sal.UserProfileId).ToList();
                    break;


                case "Date_created":
                    salariesList = salaries.OrderBy(sal => sal.DateCreated).ToList();
                    break;
                case "Date_Desc":

                    salariesList = salaries.OrderByDescending(sal => sal.DateCreated).ToList();
                    break;

                case "CreatedBy":
                    salariesList = salaries.OrderBy(sal => sal.CreatedBy).ToList();
                    break;
                case "CreatedBy_Desc":

                    salariesList = salaries.OrderByDescending(sal => sal.CreatedBy).ToList();
                    break;

                case "Year":
                    salariesList = salaries.OrderBy(sal => sal.Year).ToList();
                    break;

                case "Year_Description":
                    salariesList = salaries.OrderByDescending(sal => sal.Year).ToList();
                    break;
                default:
                    //students = students.OrderBy(stu => stu.FirstName);
                    salariesList = salaries.OrderBy(sal => sal.Month).ToList();
                    break;
            }
                    var salaryMonthVM = new SalaryMonthViewModel
            {
                Months = new SelectList(await monthQuery.Distinct().ToListAsync()),
                Years = new SelectList(await yearQuery.Distinct().ToListAsync()),
                Sals =  salariesList
            };

            return View(salaryMonthVM);



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


        //GIT, DON'T ANGER ME!!!!!!!!!!!!!!
    }
}
