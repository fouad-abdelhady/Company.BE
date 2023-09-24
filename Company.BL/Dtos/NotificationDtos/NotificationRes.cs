using Company.BL.Dtos.StaffDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.NotificationDtos
{
    public record NotificationRes(
        int Id, 
        string Title, 
        string ArTitle, 
        string Description, 
        string ArDescription, 
        StaffRead Poster, 
        DateTime CreatedAt
        );
}

