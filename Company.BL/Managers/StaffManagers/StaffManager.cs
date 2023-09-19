using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.StaffDtos;
using Company.DAL;
using Company.DAL.Data.Models;


namespace Company.BL.Managers.StaffManagers
{
    public class StaffManager : IStaffManager
    {
        private readonly IStaffRepo _staffRepo;
        public StaffManager(IStaffRepo staffRepo) { 
            _staffRepo = staffRepo;
        }

        public StaffRead? AddStaffMemeber(EmployeeRegisterationReq staff)
        {
           Staff? newStaff =  _staffRepo.AddStaffMemeber(new Staff() { FullName= staff.FullName, 
                EmailAddress= staff.Email,
               Role= staff.Role,
               Salary= staff.Salary,
               Image= staff.Image,
               ManagerId = staff.ManagerId
            });
            if(newStaff == null) return null;
            return new StaffRead(Id: newStaff.Id, 
                Name: newStaff.FullName, 
                Email: newStaff.EmailAddress, 
                Role: newStaff.Role, 
                Image: newStaff.Image);
        }

        public List<StaffRead>? GetAllManagers() =>_staffRepo.getAllManagers().
            Select(m => new StaffRead(Id: m.Id, Name: m.FullName, Email: m.EmailAddress, Role: m.Role, Image: m.Image)).ToList();
        

        public AllStaffMembersRead GetAllStaff(int page, int limit = 8)
        {
            int pagesCount = (int)Math.Ceiling((double)_staffRepo.GetStaffMembersCount() / limit);
            int? previousPage = page == 1? null: page - 1;
            int? nextPage = page >= pagesCount ? null : page + 1;
            return new AllStaffMembersRead(
                NextPage: nextPage,
                PreviousPage: previousPage,
                CurrentPage: page,
                TotalPages: pagesCount,
                StaffMembers: _staffRepo
                    .GetAllStaffMembers(page, limit)
                    .Select(member => new StaffRead(
                        Id: member.Id,
                        Name: member.FullName,
                        Email: member.EmailAddress, // Use EmailAddress property
                        Role: member.Role,
                        Image: member.Image
                    ))
                    .ToList());
        }

        public StaffProfileRead? GetProfile(int StaffMemeberId)
        {
            var result = _staffRepo.GetProfile(StaffMemeberId);
            if (result == null) {
                return null;
            }
            StaffRead? manager = null;
            if (result.Manager != null) 
                manager = new StaffRead(Id: result.Manager.Id, 
                    Name: result.Manager.FullName, 
                    Email: result.Manager.EmailAddress, 
                    Role: result.Manager.Role,
                    Image:result.Manager.Image);

            List<StaffRead> staffMembers = new List<StaffRead>();
            if (result.TeamMembers != null && result.TeamMembers.Any())
                staffMembers = result.TeamMembers
                    .Select(sm => 
                        new StaffRead(Id: sm.Id, Name: sm.FullName,
                        Email: sm.EmailAddress, Role: sm.Role,Image: sm.Image))
                    .ToList<StaffRead>();

            return new StaffProfileRead(Manager: manager, Team: staffMembers);
        }

        public UserInfo? GetUserInfo(int StaffMemeberId)
        {
            Staff staffMemeber = _staffRepo.GetStaffMemberInfo(StaffMemeberId);
            return new UserInfo(
                Id: staffMemeber.Id,
                FullName: staffMemeber.FullName, 
                Role: staffMemeber.Role, 
                Email: staffMemeber.EmailAddress, 
                Image: staffMemeber.Image);
        }

        public ResultDto? UpdateProfilePic(int userId, ImageUpdateReq imageUpdateReq)
        {
            bool success = _staffRepo.UpdateProfilePic(userId, imageUpdateReq.NewImage);
            return new ResultDto(State: success, Message: success ? imageUpdateReq.NewImage : "Something Went Wrong, Try Agin!");
        }
    }
}
