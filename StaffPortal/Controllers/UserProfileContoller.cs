using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StaffPortal.Models;
using StaffPortal.Interface;
using StaffPortal.Entities;
using Microsoft.AspNetCore.Identity;
using StaffPortal.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using StaffPortal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage;
//using StaffPortal.Inteface;

namespace StaffPortal.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserProfileController : BaseController
    {
        private IUserProfile _userProfile;
        private IFaculty _faculty;
        private IGrade _grade;
        private IDepartment _department;
        private StaffPortalDataContext _context;
        //private ISalary _sal;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(IUserProfile userProfile, IFaculty faculty, IGrade grade, IDepartment department, StaffPortalDataContext context, UserManager<ApplicationUser> userManager)
        {
            _userProfile = userProfile;
            _faculty = faculty;
            _grade = grade;
            _department = department;
            _context = context;
            _userManager = userManager;
            //_sal = sal;

        }
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var model = await _userProfile.GetAll();

            if (model != null)
            {

                return View(model);
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> PersonalSalary()
        {

            var user = _userManager.GetUserName(User);
            var x = await _userManager.FindByNameAsync(user);

            var sal = _userProfile.GetIdByEmail(x.Email);

            var usersal = await _userProfile.GetById(sal);

           

            if (usersal == null)
            {
                return RedirectToAction("UserError", "UserProfile");
            }
            else
            {
                return View(usersal);
            }


        }
        
        /*
        [HttpGet]
        public async Task<IActionResult> SalaryReport(int id)
        {

           

            var usersal = await _userProfile.GetById(id);



            if (usersal == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(usersal);
            }


        }
        [HttpGet]
        public async Task<IActionResult> SalaryReporttt(int id)
        {



            var usersal = await _userProfile.GetById(id);



            if (usersal == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(usersal);
            }
        }
        */
        [HttpGet]
        public IActionResult UserError()
        {
            return View();
        }
        //[HttpGet]
        //public async Task<IActionResult> UserIndex()
        //{

        //    var user = _userManager.GetUserName(User);
        //    var x = await _userManager.FindByNameAsync(user);

        //    var editUserProfile = _userProfile.GetIdByEmail(x.Email);

        //    var userprof = await _userProfile.GetById(editUserProfile);
        //    //var y = _userProfile.FindNameByStateId(userprof.NewStateId);
        //    //userprof.NewStates = y;
        //    //var z = _userProfile.FindNameByLocalId(userprof.LGAId);
        //    userprof.LGAs = z;


        //    //var
        //    if (userprof == null)
        //    {
        //        return RedirectToAction("UserError");
        //    }
        //    else
        //    {
        //        return View(userprof);
        //    }


        //}

        //[HttpGet]
        //public IActionResult UserError()
        //{
        //    return View();
        //}

        //[Authorize(Roles = "Admin")]


        [HttpGet]
        public async Task<IActionResult> Create()
        {

          
            var grade = await _grade.GetAll();
            var department = await _department.GetAll();


            var departmentList = department.Select(d => new SelectListItem()
            {
                Value = d.Id.ToString(),
                Text = d.DeptName + " - " + d.DeptCode
            });

            List<string> tempEmailList = _context.UserProfiles.Select(q => q.Email).ToList();
            var temp = _context.Users.Where(u => !tempEmailList.Contains(u.Email));

            

            ViewBag.users = temp.ToList();
            bool isEmpty = !temp.ToList().Any();
            if (isEmpty)
            {
                Alert("No available User!", NotificationType.warning);
                return RedirectToAction("Index");
            }
            ViewBag.department = departmentList;
          
            ViewBag.grades = _context.Grades.Distinct().ToList();

            //var fac = await _grade.GetAll();
            //var FacList = fac.Select(f => new SelectListItem()
            //{
            //    Value = f.Id.ToString(),
            //    Text = f.GradeName
            //}).Distinct();


            //ViewBag.fac = FacList.Distinct();

            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(UserProfile userProfile)
        {

            //var user = _userManager.GetUserName(userProfile);
            var x = await _userManager.FindByEmailAsync(userProfile.Email);
            userProfile.CreatedBy = _userManager.GetUserName(User);
            userProfile.FirstName = x.FirstName;
            userProfile.LastName = x.LastName;
            userProfile.Country = x.Country;
            userProfile.NewStates = x.NewStates;
            userProfile.LGAs = x.LGAs;
         
            userProfile.GradeId = Convert.ToInt32(userProfile.GradeStep);


            var grade = await _userProfile.FindGradeById(userProfile.GradeId);
            userProfile.TotAllowance = grade.TotAllowance;
            userProfile.TotDeduction = grade.TotDeduction;
            userProfile.NetPay = grade.NetSalary;
            userProfile.GradeName = grade.GradeName;
            userProfile.GradeStep = grade.Step.ToString();
            userProfile.GradeLevel = grade.Level.ToString();

            userProfile.BasicSalary = grade.BasicSalary;
            userProfile.Housing = grade.Housing;
            userProfile.HousingPercent = grade.HousingPercent;
            userProfile.Tax = grade.Tax;
            userProfile.TaxPercent = grade.TaxPercent;
            userProfile.Lunch = grade.Lunch;
            userProfile.LunchPercent = grade.LunchPercent;
            userProfile.Transport = grade.Transport;
            userProfile.TransportPercent = grade.TransportPercent;
            userProfile.Medical = grade.Medical;
            userProfile.MedicalPercent = grade.MedicalPercent;
            



            userProfile.DepartmentName = _userProfile.FindNameByDepartmentId(userProfile.DepartmentId);
            userProfile.FacultyName = _userProfile.FindFacultyNameByDepartmentId(userProfile.DepartmentId);
            


            var createUserProfile = await _userProfile.AddAsync(userProfile);

            if (createUserProfile)
            {
                Alert("UserProfile created successfully.", NotificationType.success);
                return RedirectToAction("Index");
            }
            Alert("UserProfile not created!", NotificationType.error);
            return View();
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            //var user = _userManager.GetUserName(User);
            //var editAppUserProfile = await _userManager.FindByNameAsync(user);
            var editUserProfile = await _userProfile.GetById(id);


            if (editUserProfile == null)
            {
                return RedirectToAction("Index");
            }

            var grade = await _grade.GetAll();
            var department = await _department.GetAll();


            var departmentList = department.Select(d => new SelectListItem()
            {
                Value = d.Id.ToString(),
                Text = d.DeptName
            });


            

            ViewBag.department = departmentList;
           
            ViewBag.grade = _context.Grades.ToList();

            return View(editUserProfile);

          
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserProfile userProfile)
        {
            userProfile.GradeId = Convert.ToInt32(userProfile.GradeStep);
            var grade = await _userProfile.FindGradeById(userProfile.GradeId);

            userProfile.DepartmentName = _userProfile.FindNameByDepartmentId(userProfile.DepartmentId);
            userProfile.FacultyName = _userProfile.FindFacultyNameByDepartmentId(userProfile.DepartmentId);

            var editUserProfile = await _userProfile.Update(userProfile, grade);

            //return true;
            if (editUserProfile && ModelState.IsValid)
                {

                    Alert("UserProfile edited successfully.", NotificationType.success);
                    return RedirectToAction("Index");

                }

            Alert("UserProfile not edited!", NotificationType.error);
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> EditUser()
        //{

        //    var user = _userManager.GetUserName(User);
        //    var x = await _userManager.FindByNameAsync(user);

        //    var id = _userProfile.GetIdByEmail(x.Email);

        //    //var editUserProfile = await _userProfile.GetById(editUserProfile);

        //    var editUserProfile = await _userProfile.GetById(id);

        //    if (editUserProfile == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    var department = await _department.GetAll();

        //    var departmentList = department.Select(d => new SelectListItem()
        //    {
        //        Value = d.Id.ToString(),
        //        Text = d.DeptName
        //    });

        //    ViewBag.users = _context.Users.ToList();

        //    ViewBag.department = departmentList;
        //    ViewBag.state = _context.NewStates.ToList();



        //    return View(editUserProfile);
        //}



        //[HttpPost]
        //public async Task<IActionResult> EditUser(UserProfile userProfile)
        //{
        //    var user = _userManager.GetUserName(User);
        //    var x = await _userManager.FindByNameAsync(user);

        //    userProfile.Id = _userProfile.GetIdByEmail(x.Email);
        //    //userProfile.NewStates = _userProfile.FindNameByStateId(userProfile.NewStateId);

        //    //userProfile.LGAs = _userProfile.FindNameByLocalId(userProfile.LGAId);

        //    userProfile.DepartmentName = _userProfile.FindNameByDepartmentId(userProfile.DepartmentId);
        //    userProfile.FacultyName = _userProfile.FindFacultyNameByDepartmentId(userProfile.DepartmentId);
        //    var editUserProfile = await _userProfile.UpdateUser(userProfile);

        //    if (editUserProfile)
        //    {

        //        Alert("UserProfile edited successfully.", NotificationType.success);
        //        return RedirectToAction("UserIndex");

        //    }
        //    Alert("UserProfile not edited!", NotificationType.error);
        //    return View();
        //}



        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteUserProfile = await _userProfile.Delete(id);


            if (deleteUserProfile)
            {
                Alert("UserProfile deleted successfully.", NotificationType.success);
                return RedirectToAction("Index");
            }
            Alert("UserProfile not deleted!", NotificationType.error);
            return View();
        }


        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "UserProfile");
        }

        public IActionResult Cancell()
        {
            return RedirectToAction("UserIndex", "Account");
        }

        public JsonResult GetStep(string gradelevelid)
        {
            //int gradeid = Convert.ToInt32(id);
            //gradename = "grade1";
            var levelid = Convert.ToInt32(gradelevelid);

            //var gradelevel = Convert.ToInt32("7");
            var step = _grade.GetStepsById( levelid);


            var stepList = step.Select(l => new SelectListItem()
            {
                Value = l.Id.ToString(),
                Text = l.Step.ToString()
            }); ;

            return Json(stepList);

        }


        public JsonResult GetLevel(string gradename)
        {
            //int gradeid = Convert.ToInt32(id);
            //gradename = "grade1";
            var level = _grade.GetLevelsById(gradename);

            //level.Insert(0, new Grade { Id = 0, Step = "Select Local Government" });
            //level.Insert(0, new UserProfile { Id = 0, GradeStep = "Select Grade" });
            var levelList = level.Select(l => new SelectListItem()
            {
                Value = l.Id.ToString(),
                Text = l.Level.ToString()
            }); ;

            return Json(levelList);
            
        }

        
      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
