using Company.BL.Dtos.AuthDtos;
using Company.BL.Managers.AuthManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {   
        private readonly IAuthManager _authManager;
        public AuthController(IAuthManager authManager) { 
            _authManager = authManager;
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult<LoginRead> Login(LoginReqBody reqBody) { 
            var result = _authManager.LogIn(reqBody.UserName, reqBody.Password);
            if (result == null) return Unauthorized();
            return Ok(result);
        }
        [HttpPut]
        [Route("Password")]
        [Authorize]
        public ActionResult<ResultDto> UpdateStaffMemberPassword(PasswordUpdateReq passwordUpdateReq) {
            var result = _authManager.ChangePassword(passwordUpdateReq, int.Parse(User.FindFirst("AuthId").Value));
            return Ok(result);
        }
    }
}
