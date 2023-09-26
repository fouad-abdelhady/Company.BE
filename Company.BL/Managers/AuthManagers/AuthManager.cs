
using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.StaffDtos;
using Company.DAL;
using Company.DAL.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Company.BL.Managers.AuthManagers
{
    public class AuthManager : IAuthManager
    {

        private readonly IConfiguration _configration;
        private readonly IAuthRepo _authRepo;
        public AuthManager(IAuthRepo authRepo, IConfiguration configuration) {
            _authRepo = authRepo;
            _configration = configuration;
        }

        public ResultDto ChangePassword(PasswordUpdateReq passwordUpdateReq, int authId)
        {
            bool result = _authRepo.ChangePassword(passwordUpdateReq.oldPassword, passwordUpdateReq.newPassword, authId);
            string message = result ? "Password Updated" : "You Entered Wrong Password";
            return new ResultDto(State:result, Message: message);
        }

        public LoginRead? LogIn(string username, string password)
        {
            password = GetHashedPassword(password);
            var SmAuth = _authRepo.LogIn(username, password);
            if (SmAuth == null) return null;
            var accessToken = GenerateToken(SmAuth);
            return new LoginRead(
                    Id: SmAuth.StaffMemberId,
                    FullName: SmAuth.StaffMember!.FullName,
                    Role: SmAuth.StaffMember.Role,
                    Email: SmAuth.StaffMember.EmailAddress,
                    AccessToken: accessToken,
                    Image: SmAuth.StaffMember.Image
                );
        }

        public ResultDto LogOut(int authId)
        {
            throw new NotImplementedException();
        }

        public ResultDto Register(EmployeeRegisterationReq auth, int staffMemberId)
        {
           Auth newAuth = new Auth() { UserName= auth.UserName, Password = GetHashedPassword(auth.InitPassword), StaffMemberId= staffMemberId };
            var result = _authRepo.Register(newAuth);
            if (result) return new ResultDto(State: result, Message: "Error Occured While Registering");
            return new ResultDto(State: result, Message: "Created Successfully");
        }

        private string GenerateToken(Auth auth) {
            var claims = new List<Claim> {
                new Claim("AuthId", auth.Id.ToString()),
                new Claim("UserId", auth.StaffMemberId.ToString()),
                new Claim("Role", auth.StaffMember.Role??"")
            };

            var tokenKeyStr = _configration["accesstoken"]??"";
            var tokenKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKeyStr));
            var tokenKeySignature = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256Signature);

            var accessTokenObj = new JwtSecurityToken(
                    signingCredentials:tokenKeySignature,
                    expires: DateTime.Now.AddDays(1),
                    claims:claims
                );

            return new JwtSecurityTokenHandler().WriteToken(accessTokenObj);
        }

        string GetHashedPassword(string password) {
            byte[] salt = Encoding.ASCII.GetBytes("cf0jGhtiz7sdGtej+0WgBA==");
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
