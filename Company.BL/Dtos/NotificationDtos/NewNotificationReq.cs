using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Dtos.NotificationDtos
{
    public record NewNotificationReq(
            string Title, 
            string ArTitle, 
            string Description, 
            string ArDescription, 
            int PosterId, 
            int RecieverId, 
            int RelatedTaskId, 
            int Type 
        );
}
