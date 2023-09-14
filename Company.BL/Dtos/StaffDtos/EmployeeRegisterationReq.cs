using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.StaffDtos
{
    public record EmployeeRegisterationReq(
            string FullName,
            string Email,
            double Salary,
            string Role,
            string? Image,
            string UserName,
            string InitPassword,
            int? ManagerId
        );
}
