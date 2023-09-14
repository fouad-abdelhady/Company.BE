
using System.ComponentModel.DataAnnotations;

namespace Company.DAL.Data.Models
{
    internal class RoleCheckAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string role = value as string;
            if (role == null) return false;
            return Staff.ROLES_LIST.Contains(role.ToLower());
        }
    }
}