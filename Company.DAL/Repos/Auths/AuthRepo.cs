using Company.DAL.Data.Context;
using Company.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace Company.DAL
{
    public class AuthRepo:IAuthRepo
    {
        private readonly CompanyContext _companyContext;
        public AuthRepo(CompanyContext companyContext) {
            _companyContext = companyContext;
        }

        public bool ChangePassword(string oldPassword, string newPassword, int authId)
        {
            var userAuth = _companyContext.Auths.FirstOrDefault(auth => auth.Id == authId && auth.Password == oldPassword);
            if(userAuth == null) return false;
            userAuth.Password = newPassword;
            try
            {
                _companyContext.SaveChanges();
                return true;
            }
            catch (Exception ex) { 
                return false;
            }
        }

        public Auth? LogIn(string username, string password)
        {
            var result = _companyContext.Auths.Include(a => a.StaffMember).
                FirstOrDefault(a => a.UserName == username && a.Password == password);
            return result;
        }

       
        public bool LogOut(int authId)
        {
            throw new NotImplementedException();
        }

        public bool Register(Auth auth)
        {
            try
            {
                _companyContext.Auths.Add(auth);
                _companyContext.SaveChanges();
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }
    }
}
