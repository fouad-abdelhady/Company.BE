using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.TaskDtos
{
    public record TaskStateUpdate(int TaskId, int State);
}
