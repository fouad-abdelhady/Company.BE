using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.AuthDtos
{
    public record UserInfo(int Id,string FullName, string Role, string Email, string? Image);
}
