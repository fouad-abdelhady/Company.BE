using Company.DAL.Data.Context;
using Company.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL
{
    public class StaffRepo : IStaffRepo
    {
        private readonly CompanyContext _companyContext;
        public StaffRepo(CompanyContext companyContext) { 
            _companyContext = companyContext;
        }
        //Level 1
        public Staff? AddStaffMemeber(Staff staff)
        {
            try
            {
                _companyContext.Staffs.Add(staff);
                _companyContext.SaveChanges();
                return staff;
            }
            catch (Exception ex) { 
                return null;
            }
           
        }
        public List<Staff>? getAllManagers()
        {
           var managers =  _companyContext.Staffs.Where(s=> s.Role.ToLower() == "manager").ToList()??new List<Staff>();
            Console.WriteLine(managers.First().EmailAddress);
            return managers;
        }
        //Level 1
        public IEnumerable<Staff>? GetAllStaffMembers(int page, int limit = 8)
        {
            int skip = (page - 1) * limit;
            return  _companyContext.Staffs.Where(employee=> employee.Role == "Employee")
                    .OrderBy(s => s.Id) 
                    .Skip(skip) 
                    .Take(limit)     
                    .ToList();
        }
        //Level 2
        public Staff? GetProfile(int staffMemberId)
        {
            Staff? staff = _companyContext.Staffs
                    .Include(s => s.Manager)
                    .Include(s => s.TeamMembers).FirstOrDefault(s => s.Id == staffMemberId);
            if (staff != null && staff.Role == "Employee" && staff.ManagerId != null)
                staff.TeamMembers = GetTeamMembers(staff.ManagerId ?? 1, staff.Id);
            return staff;
        }
        public Staff? GetStaffMemberInfo(int staffMemeberId) => _companyContext.Staffs.FirstOrDefault(s => s.Id == staffMemeberId);
        

        public int GetStaffMembersCount()=>_companyContext.Staffs.Count();

        private List<Staff>? GetTeamMembers(int managerId, int employeeId) => _companyContext.Staffs.Where(sm => sm.ManagerId == managerId && sm.Id != employeeId).ToList();

        bool IStaffRepo.UpdateProfilePic(int userId, string newImage)
        {
            Staff? staff = _companyContext.Staffs.FirstOrDefault(s => s.Id == userId);
            if (staff == null) return false;
            staff.Image = newImage;
            try
            {
                _companyContext.SaveChanges();
                return true;
           }
            catch (Exception e)
           {
                return false;
            }
        }
    }
}
