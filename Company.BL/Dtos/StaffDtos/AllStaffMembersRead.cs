using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.StaffDtos
{
    public record AllStaffMembersRead(
            int?NextPage,
            int?PreviousPage,
            int CurrentPage,
            int TotalPages,
            List<StaffRead>? StaffMembers
        );
}
