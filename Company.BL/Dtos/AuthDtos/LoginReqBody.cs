﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.AuthDtos
{
    public record LoginReqBody(string UserName, string Password);
}