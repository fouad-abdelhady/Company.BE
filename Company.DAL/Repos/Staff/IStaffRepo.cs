using Company.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL
{
    public interface IStaffRepo
    {
        //level 1
        Staff? AddStaffMemeber(Staff staff);
        //list of level 1
        IEnumerable<Staff>? GetAllStaffMembers(int page, int limit=8);
        //level 2
        Staff GetProfile(int managerId);

        int GetStaffMembersCount();
        Staff? GetStaffMemberInfo(int staffMemeberId);
        bool UpdateProfilePic(int userId, string newImage);
        List<Staff> getAllManagers();
    }
}
