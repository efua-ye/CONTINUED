using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Data;
using StaffPortal.Entities;
using Microsoft.EntityFrameworkCore;
using StaffPortal.Interface;
using System.Linq;

namespace StaffPortal.Services
{
    public class GradeService : IGrade
    {
        private StaffPortalDataContext _context;
        public GradeService(StaffPortalDataContext context)
        {
            _context = context;
        }

        public void Add(Grade grade) //Add
        {
          

            _context.Add(grade);

            _context.SaveChanges();
        }
        public async Task<bool> AddAsync(Grade grade) //AddAsync
        {
            //  var existingGradeCount = _context.Grade.Count(g => g.GradeName == grade.GradeName);
            //   var existingGradeCountLevel = _context.Grade.Count(g => g.GradeLevel == grade.GradeLevel);
            var existingGradeCount = _context.Grades.Count(g => g.Step == grade.Step && g.Level == grade.Level && g.GradeName == grade.GradeName);

            if (existingGradeCount == 0)
            {
                try
                {
                    grade.Tax = grade.TaxPercent * grade.BasicSalary / 100;
                    grade.Lunch = grade.LunchPercent * grade.BasicSalary / 100;
                    grade.Transport = grade.TransportPercent * grade.BasicSalary / 100;
                    grade.Housing = grade.HousingPercent * grade.BasicSalary / 100;
                    grade.Medical = grade.MedicalPercent * grade.BasicSalary / 100;

                    grade.TotDeduction = grade.Tax;

                    grade.TotAllowance = grade.BasicSalary + grade.Lunch + grade.Medical + grade.Housing +
                        grade.Transport;

                    grade.NetSalary = grade.TotAllowance - grade.TotDeduction;
                    await _context.AddAsync(grade);

                    await _context.SaveChangesAsync();
                }

                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
//}
        public async Task<bool> Delete(int Id)//Delete
        {
            // find the entity/object
            var _grade = await _context.Grades.FindAsync(Id);

            if (_grade != null)
            {
                _context.Grades.Remove(_grade);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Grade>> GetAll() //GetAll
        {

            return await _context.Grades.ToListAsync();
        }

        public async Task<Grade> GetById(int Id) //GetById
        {
            var _grade = await _context.Grades.FindAsync(Id);

            return _grade;
        }
        public async Task<bool> Update(Grade grade) //Update
        {
            var _grade = await _context.Grades.FindAsync(grade.Id);
            if (_grade != null)
            {
                _grade.GradeName = grade.GradeName;
                _grade.Level = grade.Level;
                _grade.Step = grade.Step;
                _grade.BasicSalary = grade.BasicSalary;
                _grade.LunchPercent = grade.LunchPercent;
                _grade.TaxPercent = grade.TaxPercent;
                _grade.TransportPercent = grade.TransportPercent;
                _grade.HousingPercent = grade.HousingPercent;
                _grade.MedicalPercent = grade.MedicalPercent;

                 _grade.Tax = _grade.TaxPercent * _grade.BasicSalary / 100;
                _grade.Lunch = _grade.LunchPercent * _grade.BasicSalary / 100;
                _grade.Transport = _grade.TransportPercent * _grade.BasicSalary / 100;
                _grade.Housing = _grade.HousingPercent * _grade.BasicSalary / 100;
                _grade.Medical = _grade.MedicalPercent * _grade.BasicSalary / 100;



                _grade.TotAllowance = _grade.BasicSalary + _grade.Lunch + _grade.Medical + _grade.Housing +
                _grade.Transport;
                _grade.TotDeduction = _grade.Tax;
                _grade.NetSalary = _grade.TotAllowance - _grade.TotDeduction;

                /*
                _grade.LunchPercent = grade.LunchPercent;
                _grade.TaxPercent = grade.TaxPercent;
                _grade.TransportPercent = grade.TransportPercent;
                _grade.HousingPercent = grade.HousingPercent;
                _grade.MedicalPercent = grade.MedicalPercent;

                grade.Tax = grade.TaxPercent * grade.BasicSalary / 100;
                grade.Lunch = grade.LunchPercent * grade.BasicSalary / 100;
                grade.Transport = grade.Transport * grade.BasicSalary / 100;
                grade.Housing = grade.Housing * grade.BasicSalary / 100;
                grade.Medical = grade.Medical * grade.BasicSalary / 100;


              _grade.Tax = grade.Tax;
                _grade.Lunch = grade.Lunch;
                _grade.Transport = grade.Transport;
                _grade.Housing = grade.Housing;
                _grade.Medical = grade.Medical;


                s.TotAllowance = 0.0;
                s.TotDeduction = 0.0;
                */
                await _context.SaveChangesAsync();
                return true;
            }

            return false;

        }

        public IEnumerable<Grade> GetLevelsById(string gradenameid) //GetAll
        {
            var name = _context.Grades.First(n => n.GradeName == gradenameid);
            
            var list = _context.Grades.Where(u => u.GradeName == name.GradeName).Distinct().ToList();
            //list.Insert(0, new Grade { Id = 0, Level = -1 });
            return list;
        }


        public IEnumerable<Grade> GetStepsById( int gradelevelid) //GetAll
        {
            
            //try
            //{
                var name = _context.Grades.First(n => n.Id == gradelevelid);

                var list = _context.Grades.Where(u => u.Level == name.Level).Where(u => u.GradeName == name.GradeName).Distinct().ToList();
                //list.Insert(0, new Grade("--Select State--", "0"));
                //list.Insert(0, new Grade { Id = 0, Step = -1 });
                return list;
            //}
            //catch (Exception ex)
            //{
            //    return _context.Grades.ToList();
            //}
           
        }

    }
}
