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
<<<<<<< HEAD
        
=======
        [HttpGet]
        public async Task<IActionResult> UserYear()
        {

            var user = _userManager.GetUserName(User);
            var x = await _userManager.FindByNameAsync(user);

            var sal = _userProfile.GetIdByEmail(x.Email);
            if (sal != 0)
            {
                var usersal = await _salary.GetUserYear(sal);



                if (usersal != null)
                {
                    return View(usersal);
                }
            }


            return RedirectToAction("UserError", "UserProfile");



        }
>>>>>>> 52466e7b508a31b6e1a8429e02feae69266f3c94

            public async Task<IActionResult> Year()
        {
            

            var model = await _salary.GetYear();

            if (model != null)
            {

                return View(model);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> YearlyReport(int id)
        {
           
           
            var sal = await _salary.GetById(id);
            var userp = await _userProfile.GetById(sal.UserProfileId);
            var x = await _userManager.FindByEmailAsync(userp.Email);
            var model = await _salary.GetById(sal);

            var salaryYearVM = new SalaryYearViewModel
            {
                //Months = new SelectList(await monthQuery.Distinct().ToListAsync()),
                //Years = new SelectList(await yearQuery.Distinct().ToListAsync()),
                //Gradenames = new SelectList(await gradeQuery.Distinct().ToListAsync()),
                FullName = x.FullName,
                Appuser = await _context.Users.Where(a => a.Email == userp.Email).ToListAsync(),
                Grades = await _context.Grades.Where(a => a.Id == userp.GradeId).ToListAsync(),
                Sals = await _context.Salaries.Include(u => u.UserProfile).Where(a => a.Year == sal.Year).Where(a => a.UserProfileId == sal.UserProfileId).ToListAsync()


                //Sals = await PaginatedList<Salary>.CreateAsync(salaries.AsNoTracking(), pageNumber ?? 1, pageSize)
            };

            return View(salaryYearVM);



            //if (model != null)
            //{

            //    return View(model);
            //}
            //return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserYearlyReport(int id)
        {


            var sal = await _salary.GetById(id);
            var userp = await _userProfile.GetById(sal.UserProfileId);
            var x = await _userManager.FindByEmailAsync(userp.Email);
            var model = await _salary.GetYearReport(sal);

            var salaryYearVM = new SalaryYearViewModel
            {
               
                FullName = x.FullName,
                Appuser = await _context.Users.Where(a => a.Email == userp.Email).ToListAsync(),
                Grades = await _context.Grades.Where(a => a.Id == userp.GradeId).ToListAsync(),
                Sals = await _context.Salaries.Include(u => u.UserProfile).Where(a => a.Year == sal.Year).Where(a => a.UserProfileId == sal.UserProfileId).ToListAsync()


                //Sals = await PaginatedList<Salary>.CreateAsync(salaries.AsNoTracking(), pageNumber ?? 1, pageSize)
            };

            return View(salaryYearVM);



        }


        public async Task<IActionResult> Index(string salaryMonth, string salaryYear, string searchString, string Sorting_Order, string currentFilter, int? pageNumber)
        {
            ViewBag.CurrentSort = Sorting_Order;
            ViewBag.SortingMonth = String.IsNullOrEmpty(Sorting_Order) ? "Month_Desc" : "";
            ViewBag.SortingYear = Sorting_Order == "Year" ? "Year_Desc" : "Year";
            ViewBag.SortingDate = Sorting_Order == "Date_created" ? "Date_Desc" : "Date_created";
            ViewBag.SortingCreate = Sorting_Order == "CreatedBy" ? "CreatedBy_Desc" : "CreatedBy";
            ViewBag.SortingId = Sorting_Order == "ID" ? "ID_Desc" : "ID";
            ViewBag.Sortingfirst = Sorting_Order == "UserProfile" ? "UserProfile_Desc" : "UserProfile";
            ViewBag.Sortinglast = Sorting_Order == "UserProfilelast" ? "UserProfile_Desclast" : "UserProfilelast";
            ViewBag.Sortingemail = Sorting_Order == "UserProfileemail" ? "UserProfile_Descemail" : "UserProfileemail";

            IQueryable<string> monthQuery = from m in _context.Salaries
                                            orderby m.Month
                                            select m.Month;

            IQueryable<string> yearQuery = from m in _context.Salaries
                                           orderby m.Year
                                           select m.Year;
            IQueryable<string> gradeQuery = from m in _context.Salaries
                                           orderby m.UserProfile.GradeName
                                           select m.UserProfile.GradeName;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

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
            //IQueryable salariesList = new List<Salary>();
            //IQueryable<T> salariesList;
            switch (Sorting_Order)
            {
                case "Month_Asc":
                    //students = students.OrderByDescending(stu => stu.FirstName);
                    salaries = salaries.OrderBy(sal => sal.Month);
                    break;

                case "Month_Desc":
                    salaries = salaries.OrderByDescending(sal => sal.Month);
                    break;

                case "ID":

                    salaries = salaries.OrderBy(sal => sal.Id);
                    break;

                case "ID_Desc":
                    salaries = salaries.OrderByDescending(sal => sal.Id);
                    break;

                case "UserProfile":

                    salaries = salaries.OrderBy(sal => sal.UserProfile.FirstName);
                    break;

                case "UserProfile_Desc":
                    salaries = salaries.OrderByDescending(sal => sal.UserProfile.FirstName);
                    break;

                case "UserProfilelast":

                    salaries = salaries.OrderBy(sal => sal.UserProfile.LastName);
                    break;

                case "UserProfile_Desclast":
                    salaries = salaries.OrderByDescending(sal => sal.UserProfile.LastName);
                    break;

                case "UserProfileemail":

                    salaries = salaries.OrderBy(sal => sal.UserProfile.Email);
                    break;

                case "UserProfile_Descemail":
                    salaries = salaries.OrderByDescending(sal => sal.UserProfile.Email);
                    break;

                case "Date_created":
                    salaries = salaries.OrderBy(sal => sal.DateCreated);
                    break;
                case "Date_Desc":

                    salaries = salaries.OrderByDescending(sal => sal.DateCreated);
                    break;

                case "CreatedBy":
                    salaries = salaries.OrderBy(sal => sal.CreatedBy);
                    break;
                case "CreatedBy_Desc":

                    salaries = salaries.OrderByDescending(sal => sal.CreatedBy);
                    break;

                case "Year":
                    salaries = salaries.OrderBy(sal => sal.Year);
                    break;

                case "Year_Description":
                    salaries = salaries.OrderByDescending(sal => sal.Year);
                    break;
                default:
                    //students = students.OrderBy(stu => stu.FirstName);
                    salaries = salaries.OrderBy(sal => sal.Month);
                    break;
            }
            //int pageSize = 3;
            var salaryMonthVM = new SalaryMonthViewModel
            {
                Months = new SelectList(await monthQuery.Distinct().ToListAsync()),
                Years = new SelectList(await yearQuery.Distinct().ToListAsync()),
                Gradenames = new SelectList(await gradeQuery.Distinct().ToListAsync()),
                Sals = salaries.Include(u => u.UserProfile).ToList()


                //Sals = await PaginatedList<Salary>.CreateAsync(salaries.AsNoTracking(), pageNumber ?? 1, pageSize)
            };

            return View(salaryMonthVM);


            //return View(await PaginatedList<Salary>.CreateAsync(salaries.AsNoTracking(), pageNumber ?? 1, pageSize));


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
<<<<<<< HEAD
            //var sam = salary.TransportPercent_;
            //var nuel = salary.UserProfile.Transport;
            //salary.Transport = salary.TransportPercent_ + 5000;
=======
>>>>>>> 52466e7b508a31b6e1a8429e02feae69266f3c94
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
                Alert("Cannot create duplicate salary record😔!", NotificationType.error);
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
        public async Task<IActionResult> DeleteYear(int id)
        {
            var deleteSalary = await _salary.DeleteYear(id);
            if (deleteSalary)
            {
                return RedirectToAction("Year", "Salary");
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


        [HttpGet]
        public async Task<IActionResult> MonthlySalary(int id)
        {
            /*
            var user = _userManager.GetUserName(User);
            var x = await _userManager.FindByNameAsync(user);

            var sal = _userProfile.GetIdByEmail(x.Email);

            var usersal = await _userProfile.GetById(sal);
            */

            var sal = await _salary.GetById(id);
            var userp = await _userProfile.GetById(sal.UserProfileId);
            var x = await _userManager.FindByEmailAsync(userp.Email);
            var sali = _userProfile.GetIdByEmail(x.Email);

            //var usersal = await _userProfile.GetById(id);
            // var model = await _salary.GetById(sal);



            if (sal == null)
            {
                return RedirectToAction("UserError");
            }
            else
            {
                return View(sal);
            }

        }
        //UserMonthlyReport
    }
}
