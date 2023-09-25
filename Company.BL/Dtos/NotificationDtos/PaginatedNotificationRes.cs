using Company.BL.Dtos.CommonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.NotificationDtos
{
    public record PaginatedNotificationRes(
            PageInfo PageInfo,
            List<NotificationRes> Notifications
        );
}
