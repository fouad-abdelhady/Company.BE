using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.TaskDtos
{
    public record NewTaskReq(
        string Title,
        string ArTitle,
        string Description, 
        string ArDescription,
        int EmployeeId);
}
