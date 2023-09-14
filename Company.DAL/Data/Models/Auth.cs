using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.Models;

public class Auth
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100,MinimumLength =5)]
    public string UserName { get; set; } 
    [Required]
    [StringLength(32, MinimumLength = 8)]
    public string Password { get; set; }
    public int StaffMemberId { get; set; }
    public Staff? StaffMember { get; set; }
}

