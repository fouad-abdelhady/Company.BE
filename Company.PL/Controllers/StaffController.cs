
using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.StaffDtos;
using Company.BL.Managers.AuthManagers;
using Company.BL.Managers.StaffManagers;
using Company.DAL.Data.Models;
using Company.PL.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace Company.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffManager _staffManager;
        private readonly IAuthManager _authManager;
        //private readonly IHttpContextAccessor _contextAccessor;
        public StaffController(/*IHttpContextAccessor contextAccessor ,*/ IStaffManager staffManager, IAuthManager authManager)
        {
            _staffManager = staffManager;
            _authManager = authManager;
         //   _contextAccessor = contextAccessor;
        }
        [HttpGet]
        [Route("Profile")]
        [Authorize]
        public ActionResult<StaffProfileRead> GetStaffMemberProfile()
        {
            Console.WriteLine("The Id is = " + User.FindFirst("UserId"));
            if (int.TryParse(User.FindFirst("UserId")!.Value, out int userId))
                return Ok(_staffManager.GetProfile(userId));
            return StatusCode(500);
        }
        [HttpGet]
        [Route("All")]
        [Authorize]
        public ActionResult<List<StaffRead>> GatAllStaffMembers([FromQuery] int page = 1, [FromQuery] int limit = 8)
        {
            return Ok(_staffManager.GetAllStaff(page, limit));
        }

        [HttpPost]
        [Authorize]
        [AdminAuth]
        public ActionResult<StaffRead> AddEmployee(EmployeeRegisterationReq employeeRegisterationReq)
        {
            var newStaffMember = _staffManager.AddStaffMemeber(employeeRegisterationReq);
            if (newStaffMember == null) return StatusCode(500);
            var regResult = _authManager.Register(employeeRegisterationReq, newStaffMember.Id);
            if (!regResult.State) return StatusCode(500); // should delete the newStaffMember
            return Ok(newStaffMember);
        }
        [HttpGet]
        [Authorize]
        [Route("UserInfo")]
        public ActionResult<UserInfo> ChechUserState()
        {
            UserInfo? userInfo = _staffManager.GetUserInfo(int.Parse(User.FindFirst("UserId")!.Value));
            return Ok(userInfo);
        }
        [HttpPut]
        [Authorize]
        [Route("ProfilePic")]
        public ActionResult<ResultDto> UpdateProfilePic(ImageUpdateReq imageUpdateReq)
        {
            return _staffManager.UpdateProfilePic(int.Parse(User.FindFirst("UserId")!.Value), imageUpdateReq);
        }

        [HttpGet]
        [Route("Managers")]
        [Authorize]
        [AdminAuth]
        public ActionResult<List<StaffRead>> GetManagers() => Ok(_staffManager.GetAllManagers());

        [HttpPost]
        [Route("UploadProfileImg")]
       // [Authorize]
        public async Task<ActionResult<ResultDto>> UploadProfileImageAsync(IFormFile Img)
        {
           // int.TryParse(User.FindFirst("UserId").Value, out int userId);
            string imageName = await saveImage(Img, 3);

            return Ok(/*_staffManager.UpdateProfilePic(3, imageName, Path.Combine(_hostingEnv.WebRootPath, "images", imageName))*/);
        }
       /* [HttpGet("{imageName}")]
        public ActionResult<string> GetImage(string imageName) {
            var imagePath = $"~/wwwroot/images/{imageName}";
            var imagedir = Url.Content(imagePath);
           // var baseUrl = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";
            return Ok($"/*{imagedir}");
        }*/

        [HttpGet("{password}")]
        public ActionResult hashMyPassword(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes("cf0jGhtiz7sdGtej+0WgBA==");

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            string hashedPassword = $"{hashed}";
            return Ok(new {
                password = password,
                hasshed = hashedPassword
            });
        }
        private async Task<string> saveImage(IFormFile image, int UserId)
        {
            string imageName = "";
            try
            {
                var extention = Path.GetExtension(image.FileName).ToLowerInvariant(); /* $".{image.FileName.Split('.')[image.FileName.Split('.').Length - 1]}"*/;
                imageName = $"{UserId}{extention}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", imageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return imageName;
        }
    }
}