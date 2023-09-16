using Company.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.DAL.Data.Context
{
    public class CompanyContext:DbContext
    {
        public DbSet<Auth> Auths { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public CompanyContext(DbContextOptions<CompanyContext> options):base(options) {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Staff>().HasData(getStaffList());
           modelBuilder.Entity<Auth>().HasData(getAuthList());
           modelBuilder.Entity<Models.Task>().HasData(getTasksList());
        }
        private List<Auth> getAuthList() => new List<Auth>()
        {
            new Auth(){Id=1, UserName= "fouad.abdelhady", Password="123456789", StaffMemberId=1 },
            new Auth(){Id=2,UserName= "ahmed.abdelhady", Password="123456789", StaffMemberId=2 },
            new Auth(){Id=3, UserName="admin.admin", Password="Admin", StaffMemberId=3 }

        };
        private List<Staff> getStaffList()=> new List<Staff>()
        {
            new Staff(){ Id=1, FullName="Fouad Abdelhady Fouad", Role="Manager", Salary=40000.50, EmailAddress= "fouad.abdelhady@egabi.com"},
            new Staff(){ Id=2, FullName="Ahmed Abdelhady Fouad", Role="Employee", Salary=4000.50, ManagerId=1, EmailAddress= "ahmed.abdelhady@egabi.com"},
            new Staff(){ Id=3, FullName="Admin", Role="Admin", Salary= 20000, EmailAddress="admin@admin.com" }
        };

        private List<Models.Task> getTasksList() => new List<Models.Task>()
        {
            new Models.Task(){ Id=1, Title= "First Task", Description="First week task", CreatorId = 1, EmployeeId = 2 }
        };   
    }
}
