﻿using Company.BL.Dtos.CommonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.TaskDtos
{
    public record TasksRes( int CallerId, PageInfo pageInfo, List<TaskRes> tasksList);
}
