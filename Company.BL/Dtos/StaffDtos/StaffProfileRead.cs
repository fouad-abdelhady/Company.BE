using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.StaffDtos
{
    public record StaffProfileRead(
        StaffRead? Manager,
        List<StaffRead>? Team
        );
}
