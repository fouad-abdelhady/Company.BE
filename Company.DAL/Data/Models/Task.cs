using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CreatorId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int Status { get; set; } = 0;
        public int? Grade { get; set; }
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime StateChangedAt { get; set; } = DateTime.Now;

        // Navigation properties for the foreign keys
        public Staff? Creator { get; set; } = null;
        public Staff? Employee { get; set; } = null;
    }
}
