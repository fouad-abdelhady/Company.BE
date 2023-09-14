using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.StaffDtos;


namespace Company.BL.Managers.StaffManagers
{
    public interface IStaffManager
    {
        StaffRead? AddStaffMemeber(EmployeeRegisterationReq staff);
        AllStaffMembersRead GetAllStaff(int page, int limit = 8);
        StaffProfileRead GetProfile(int StaffMemeberId);
        UserInfo? GetUserInfo(int StaffMemeberId);
        ResultDto UpdateProfilePic(int v, ImageUpdateReq imageUpdateReq);
        List<StaffRead>? GetAllManagers();
    }
}
