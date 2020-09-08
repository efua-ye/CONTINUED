using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Data;
using StaffPortal.Entities;
using Microsoft.EntityFrameworkCore;
using StaffPortal.Interface;
using Microsoft.AspNetCore.Identity;
using StaffPortal.Models;
using System.Linq;

namespace StaffPortal.Services
{
    public class UserProfileService : IUserProfile
    {
        private StaffPortalDataContext _context;
        //private IUserProfile _userProfile;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserProfileService(StaffPortalDataContext context, UserManager<ApplicationUser> userManager)
        {
            //_userProfile = userProfile;
            _userManager = userManager;
            _context = context;
        }

        public void Add(UserProfile userprofile) //Add
        {
            _context.Add(userprofile);
           
            _context.SaveChanges();
        }

        public async Task<bool> AddAsync(UserProfile userprofile) //AddAsync
        {
            try
            {

                await _context.AddAsync(userprofile);
               
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public async Task<bool> Delete(int Id)//Delete
        {

            var _userprofile = await _context.UserProfiles.FindAsync(Id);

            //var useremail = _userprofile.Email;

            if (_userprofile != null)
            {
                _context.UserProfiles.Remove(_userprofile);
                _context.SaveChanges();
                return true;
            }

            return false;
        }


        public async Task<IEnumerable<UserProfile>> GetAll()
        {

            return await _context.UserProfiles.Include(d => d.Department).Include(g => g.Grade).ToListAsync();
            //return await _context.UserProfiles.Include(d => d.Department).ToListAsync();
        }



        public async Task<Grade> FindGradeById(int id)
        {
            var grade = await _context.Grades.FirstAsync(n => n.Id == id);
            return grade;
        }

        public string FindNameByDepartmentId(int id)
        {
            var name = _context.Departments.First(n => n.Id == id);
            return name.DeptName;
        }
        public string FindFacultyNameByDepartmentId(int id)
        {
            var name = _context.Departments.First(n => n.Id == id);
            return _context.Faculties.First(f => f.Id == name.FacultyId).Name;
        }
        public async Task<UserProfile> GetById(int Id)
        {
            var _userprofile = await _context.UserProfiles.FindAsync(Id);

            return _userprofile;
        }

        public int GetIdByEmail(string Email)
        {
            try
            {
                var _user = _context.UserProfiles.First(u => u.Email == Email);
                return _user.Id;
            }
            catch (Exception)
            { return 0; }
         }


        //public async Task<IEnumerable<Local>> GetLocalByStateIdAsync(int id)
        //{

        //    try
        //    {
        //        return await _context.Locals.Where(u => u.States.Id == id);
        //        //var _user =await _userManager.FindByEmailAsync(Email);

        //        //var id = _context.UserProfiles.First(u => u.Email == _user.Email).Id;
        //        //return _user.Id;
        //    }
        //    catch (Exception)
        //    { return 0; }


        //}
        public async Task<bool> UpdateGrade( Grade grade) //Update
        {
            var _userprofile = await  _context.UserProfiles.Where(a => a.GradeId == grade.Id).ToListAsync();

            if (_userprofile != null)

            {
                _userprofile.ForEach(x =>
                {
                    x.GradeName = grade.GradeName;
                    x.GradeLevel = grade.Level.ToString();
                    x.GradeStep = grade.Step.ToString();
                    x.BasicSalary = grade.BasicSalary;
                    x.Tax = grade.Tax;
                    x.TaxPercent = grade.TaxPercent;
                    x.Housing = grade.Housing;
                    x.HousingPercent = grade.HousingPercent;
                    x.Lunch = grade.Lunch;
                    x.LunchPercent = grade.LunchPercent;
                    x.Transport = grade.Transport;
                    x.TransportPercent = grade.TransportPercent;
                    x.Medical = grade.Medical;
                    x.MedicalPercent = grade.MedicalPercent;

                });

                await _context.SaveChangesAsync();


                

            }
            return true;
        }


        public async Task<bool> Update(UserProfile userprofile,Grade grade) //Update
        {
            var _userprofile = await _context.UserProfiles.FindAsync(userprofile.Id);

            
            if (_userprofile != null)
            {
                //_userprofile.GradeId = Convert.ToInt32(userprofile.GradeStep);
                _userprofile.DepartmentId = userprofile.DepartmentId;
                _userprofile.GradeId = userprofile.GradeId;
                //var grade = await _userProfile.FindGradeById(_userprofile.GradeId);
                _userprofile.TotAllowance = grade.TotAllowance;
                _userprofile.TotDeduction = grade.TotDeduction;
                _userprofile.NetPay = grade.NetSalary;
                _userprofile.GradeName = grade.GradeName;
                _userprofile.GradeStep = grade.Step.ToString();
                _userprofile.GradeLevel = grade.Level.ToString();

                _userprofile.BasicSalary = grade.BasicSalary;
                _userprofile.Tax = grade.Tax;
                _userprofile.TaxPercent = grade.TaxPercent;
                _userprofile.Housing = grade.Housing;
                _userprofile.HousingPercent = grade.HousingPercent;
                _userprofile.Lunch = grade.Lunch;
                _userprofile.LunchPercent = grade.LunchPercent;
                _userprofile.Transport = grade.Transport;
                _userprofile.TransportPercent = grade.TransportPercent;
                _userprofile.Medical = grade.Medical;
                _userprofile.MedicalPercent = grade.MedicalPercent;
                

                _userprofile.DepartmentName = userprofile.DepartmentName;
                _userprofile.FacultyName = userprofile.FacultyName;


                //_userprofile.DepartmentName = _userProfile.FindNameByDepartmentId(_userprofile.DepartmentId);
                //_userprofile.FacultyName = _userProfile.FindFacultyNameByDepartmentId(_userprofile.DepartmentId);

                //_context.Update(_userprofile);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;

        }

        public async Task<bool> UpdateUser(UserProfile userprofile) //Update
        {
            var _userprofile = await _context.UserProfiles.FindAsync(userprofile.Id);


            if (_userprofile != null)
            {
               
                _userprofile.FirstName = userprofile.FirstName;
                _userprofile.LastName = userprofile.LastName;
                _userprofile.NewStates = userprofile.NewStates;
                _userprofile.LGAs = userprofile.LGAs;

                //_context.Update(_userprofile);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;

        }
        //public async Task<bool> UpdateUser(UserProfile userprofile) //Update
        //{
        //    var _userprofile = await _context.UserProfiles.FindAsync(userprofile.Id);
        //    if (_userprofile != null)
        //    {
        //        _userprofile.FirstName = userprofile.FirstName;
        //        _userprofile.LastName = userprofile.LastName;
        //        //_userprofile.Email = userprofile.Email;

        //        //_userprofile.Department.FacultyId = userprofile.Department.FacultyId;
        //        _userprofile.DepartmentId = userprofile.DepartmentId;
        //        _userprofile.DepartmentName = userprofile.DepartmentName;
        //        _userprofile.FacultyName = userprofile.FacultyName;
        //        _userprofile.LGAs = userprofile.LGAs;
        //        _userprofile.NewStates = userprofile.NewStates;

        //        //_userprofile.NewStateId = userprofile.NewStateId;
        //        //_userprofile.LGAId = userprofile.LGAId;
        //        _userprofile.Country = userprofile.Country;


        //        await _context.SaveChangesAsync();
        //        return true;
        //    }

        //    return false;

        //}


    }
}
