
using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.StaffDtos;
using Company.BL.Managers.AuthManagers;
using Company.BL.Managers.StaffManagers;
using Company.DAL.Data.Models;
using Company.PL.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffManager _staffManager;
        private readonly IAuthManager _authManager;
        public StaffController(IStaffManager staffManager, IAuthManager authManager) {
            _staffManager = staffManager;
            _authManager = authManager;
        }
        [HttpGet]
        [Route("Profile")]
        [Authorize]
        public ActionResult<StaffProfileRead> GetStaffMemberProfile() {
            if (int.TryParse(User.FindFirst("UserId").Value, out int userId))
                return Ok(_staffManager.GetProfile(userId));
            return StatusCode(500);
        }
        [HttpGet]
        [Route("All")]
        [Authorize]
        public ActionResult<List<StaffRead>> GatAllStaffMembers([FromQuery]int page = 1, [FromQuery]int limit = 8) { 
            return Ok(_staffManager.GetAllStaff(page, limit));
        }

        [HttpPost]
        [Authorize]
        [AdminAuth]
        public ActionResult<StaffRead> AddEmployee(EmployeeRegisterationReq employeeRegisterationReq) {
            var newStaffMember = _staffManager.AddStaffMemeber(employeeRegisterationReq);
            if(newStaffMember == null) return StatusCode(500);
            var regResult = _authManager.Register(employeeRegisterationReq, newStaffMember.Id);
            if(!regResult.State) return StatusCode(500); // should delete the newStaffMember
            return Ok(newStaffMember);
        }
        [HttpGet]
        [Authorize]
        [Route("UserInfo")]
        public ActionResult<UserInfo> ChechUserState()
        {
            UserInfo? userInfo = _staffManager.GetUserInfo(int.Parse(User.FindFirst("UserId").Value));
            return Ok(userInfo);
        }
        [HttpPut]
        [Authorize]
        [Route("ProfilePic")]
        public ActionResult<ResultDto> UpdateProfilePic(ImageUpdateReq imageUpdateReq) {
             return _staffManager.UpdateProfilePic(int.Parse(User.FindFirst("UserId").Value), imageUpdateReq);
        }

        [HttpGet]
        [Route("Managers")]
        [Authorize]
        [AdminAuth]
        public ActionResult<List<StaffRead>> GetManagers() => Ok(_staffManager.GetAllManagers());
    }
}