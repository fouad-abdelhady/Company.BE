using Company.DAL.Data.Models;

namespace Company.DAL;

public interface IAuthRepo
{
   Auth? LogIn(string username, string password);
   bool Register(Auth auth);
   bool LogOut(int authId);
   bool ChangePassword(string oldPassword, string newPassword, int authId);
}
