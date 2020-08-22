using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StaffPortal.Models;
using StaffPortal.Interface;
using StaffPortal.Entities;
using StaffPortal.Dtos;
using Microsoft.AspNetCore.Identity;
using StaffPortal.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using StaffPortal.Data;

namespace StaffPortal.Controllers
{
    public class AccountController : BaseController
    {

        private readonly IAccount _account;
        private ILocal _local;
        private IUserProfile _userPro;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private StaffPortalDataContext _context;
        public AccountController(IAccount account, IUserProfile userPro, StaffPortalDataContext context, ILocal local, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _userPro = userPro;
            _account = account;
            _signInManager = signInManager;
            _context = context;
            _local = local;
            _userManager = userManager;

        }

        //[HttpPost]
        //public async Task<IActionResult> EditUser(UserProfile userProfile)
        //{
        //    var user = _userManager.GetUserName(User);
        //    var x = await _userManager.FindByNameAsync(user);

        //    userProfile.Id = _userProfile.GetIdByEmail(x.Email);
        //    userProfile.NewStates = _userProfile.FindNameByStateId(userProfile.NewStateId);

        //    userProfile.LGAs = _userProfile.FindNameByLocalId(userProfile.LGAId);

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

        [HttpGet]
        public async Task<IActionResult> UserIndex()
        {

            var user = _userManager.GetUserName(User);
            var userprof = await _userManager.FindByNameAsync(user);

            //var editUserProfile = _userProfile.GetIdByEmail(x.Email);

            //var userprof = await _userProfile.GetById(editUserProfile);
            //var y = _userProfile.FindNameByStateId(userprof.NewStateId);
            //userprof.NewStates = y;
            //var z = _userProfile.FindNameByLocalId(userprof.LGAId);
            //userprof.LGAs = z;
            //var w = _userProfile.FindNameByDepartmentId(userprof.DepartmentId);
            //userprof.DepartmentName = w;

            //var
            if (userprof == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(userprof);
            }


        }

        [HttpGet]
        public async Task<IActionResult> EditUser()
        {

            var user = _userManager.GetUserName(User);
            var editUserProfile = await _userManager.FindByNameAsync(user);

            //var id = _userProfile.GetIdByEmail(x.Email);

            //var editUserProfile = await _userProfile.GetById(editUserProfile);

            //var editUserProfile = await _userProfile.GetById(id);

            if (editUserProfile == null)
            {
                return RedirectToAction("Index");
            }

            

            //ViewBag.users = _context.Users.ToList();

           
            ViewBag.state = _context.NewStates.ToList();
            //ViewBag.stateid = new SelectList( "DepartmentId", "DepartmentName", employee.DepartmentId);



            return View(editUserProfile);
        }



        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser userProfile)
        {
            var user = _userManager.GetUserName(User);
            var x = await _userManager.FindByNameAsync(user);
            userProfile.Id = x.Id;
            //userProfile.Id = _userProfile.GetIdByEmail(x.Email);
            userProfile.NewStates = _account.FindNameByStateId(userProfile.NewStateId);

            userProfile.LGAs = _account.FindNameByLocalId(userProfile.LGAId);

            
            var editUserProfile = await _account.UpdateUser(userProfile);

            if (editUserProfile)
            {

                Alert("UserProfile edited successfully.", NotificationType.success);
                return RedirectToAction("UserIndex");

            }
            Alert("UserProfile not edited!", NotificationType.error);
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> PersonalInfo(int id)
        {
            var _userprofile = await _context.UserProfiles.FindAsync(id);
            //var user = _userManager.GetUserName(User);
            var editUserProfile = await _userManager.FindByEmailAsync(_userprofile.Email);
            //var editUserProfile = await _userManager.FindByEmailAsync(Email);
            //var id = _userProfile.GetIdByEmail(x.Email);

            //var editUserProfile = await _userProfile.GetById(editUserProfile);

            //var editUserProfile = await _userProfile.GetById(id);

            if (editUserProfile == null)
            {
                return RedirectToAction("Index");
            }





            ViewBag.state = _context.NewStates.ToList();



            return View(editUserProfile);
        }



        [HttpPost]
        public async Task<IActionResult> PersonalInfo(ApplicationUser userProfile)
        {
            //var _userprofile = await _context.UserProfiles.FindAsync(id);
            //var user = _userManager.GetUserName(User);
            var _userprofile = await _context.UserProfiles.FindAsync(Convert.ToInt32(userProfile.Id));
            userProfile.Email = _userprofile.Email;
            var x = await _userManager.FindByEmailAsync(userProfile.Email);
            //var x = await _userManager.FindByNameAsync(user);
            userProfile.Id = x.Id;
            //userProfile.Id = _userProfile.GetIdByEmail(x.Email);
            userProfile.NewStates = _account.FindNameByStateId(userProfile.NewStateId);

            userProfile.LGAs = _account.FindNameByLocalId(userProfile.LGAId);

            _userprofile.FirstName = userProfile.FirstName;
            _userprofile.LastName = userProfile.LastName;
            _userprofile.NewStates = userProfile.NewStates;

            _userprofile.LGAs = userProfile.LGAs;

            var editUserPro = await _userPro.UpdateUser(_userprofile);

            var editUserProfile = await _account.UpdateUser(userProfile);

            if (editUserProfile)
            {

                Alert("UserProfile edited successfully.", NotificationType.success);
                return RedirectToAction("Index", "UserProfile");

            }
            Alert("UserProfile not edited!", NotificationType.error);
            return View();
        }




        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "UserName/Password is incorrect");
                return View();
            }

            var signin = await _account.LoginIn(login);

            if (signin)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();



        }
        public async Task<IActionResult> UserProfile()
        {

            var model = await _account.GetAll();

            if (model != null)
                return View(model);
            return View();
        }
        
        public IActionResult Signup()
        {
            ViewBag.state = _context.NewStates.ToList();
            return View();
        }

       

        [HttpPost]
        public async Task<IActionResult> Signup( SigninViewModel signupmodel)
        {
           
            ApplicationUser user = new ApplicationUser
            {
                UserName = signupmodel.UserName,
                Email = signupmodel.Email,
                FirstName = signupmodel.FirstName,
                LastName = signupmodel.LastName,
                Country = signupmodel.Country,
                NewStateId = signupmodel.NewStateId,
                LGAId = signupmodel.LGAId,
                NewStates = _account.FindNameByStateId(signupmodel.NewStateId),
                LGAs = _account.FindNameByLocalId(signupmodel.LGAId)
                
               
        };

            var sign = await _account.CreateUser(user, signupmodel.Password);
            if (sign)
            {
                Alert("Account Created successfully", NotificationType.success);
                return RedirectToAction("Login", "Account");
                //return RedirectToAction("Index", "Home");

            }
            Alert("Account not created!", NotificationType.error);
            return View();
            //ApplicationUser user = new ApplicationUser();



        }




        [HttpGet]
        public async Task<IActionResult> LogOut()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");


        }

        public JsonResult GetLGA(int id)
        {
            int stateid = Convert.ToInt32(id);
            var local = _local.GetLGAsById(stateid);


            var localList = local.Select(f => new SelectListItem()
            {
                Value = f.Id.ToString(),
                Text = f.Name
            });

            return Json(localList);
           
        }
        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
