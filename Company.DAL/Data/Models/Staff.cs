
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.DAL.Data.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        [Required][StringLength(50, MinimumLength = 5)]
        public string FullName { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required][RoleCheck]
        public string Role { get; set; }
        [Column(TypeName = "decimal(15,2)")]
        public double Salary { get; set; }
        public string? Image { get; set; }
        public int? ManagerId { get; set; }
        public Staff? Manager { get; set; }
        public ICollection<Staff> TeamMembers { get; set; } = new List<Staff>();
        public  static readonly HashSet<string> ROLES_LIST = new () {
            "manager",
            "employee"
        };


    }
    
}
