using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.CommonDtos
{
    public record PageInfo(int? Next, int? Previous, int? Current, int PagesCount );
}
