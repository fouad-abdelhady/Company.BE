using Company.BL.Dtos.StaffDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.TaskDtos
{
    public record TaskRes(
        int Id, 
        string Title,
        string ArTitle,
        string Description,
        string ArDescription,
        int Status, 
        StaffRead? StaffMember, 
        int? Grade,
        string? Changes,
        DateTime? CreatedAt, 
        DateTime? LastStateChange,
        string? ArChanges
        );
       
}