using Company.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.AuthDtos
{
    //Note: access Token(authId, staffId, role)
    public record LoginRead( int Id,
        string FullName, string Role, string Email,string AccessToken, string?Image);
}
