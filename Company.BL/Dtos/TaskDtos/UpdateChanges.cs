using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.TaskDtos
{
    public record UpdateChanges(int taskId, int employeeId, string changes, string arChanges);
    
}
