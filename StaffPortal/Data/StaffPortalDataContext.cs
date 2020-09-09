using System;
using StaffPortal.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StaffPortal.Models;

namespace StaffPortal.Data
{
    public class StaffPortalDataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public StaffPortalDataContext(DbContextOptions<StaffPortalDataContext> options) : base(options)
        {
        }
        /* protected override void OnModelCreating(ModelBuilder builder)
        {
           
            base.OnModelCreating(builder);
            
             builder.Entity<Faculty>()
            .HasIndex(p => new { p.Name, p.Code })
            .IsUnique(true);

           

            base.OnModelCreating(builder);
            builder.Entity<Department>()
            .HasIndex(p => new { p.DeptCode, p.DeptName })
            .IsUnique(false);
           
    }
     */
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Leave> Leaves { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
       
        public virtual DbSet<UserProfile> UserProfiles { get; set; }

        public virtual DbSet<Salary> Salaries { get; set; }

        public virtual DbSet<NewState> NewStates { get; set; }
        public virtual DbSet<LGA> LGAs { get; set; }
        public virtual DbSet<News> NewsRoom { get; set; }



    }
}
