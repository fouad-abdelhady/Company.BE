using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.StaffDtos;
using Company.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Managers.AuthManagers
{
    public interface IAuthManager
    {
        LoginRead? LogIn(string username, string password);
        ResultDto Register(EmployeeRegisterationReq auth, int authId);
        ResultDto LogOut(int authId);
        ResultDto ChangePassword(PasswordUpdateReq passwordUpdateReq, int authId);
    }
}
