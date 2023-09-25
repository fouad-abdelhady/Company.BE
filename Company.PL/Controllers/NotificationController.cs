using Company.BL.Dtos.NotificationDtos;
using Company.BL.Managers.NotificationManagers;
using Company.PL.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private INotificationManager _notificationManager;
        public NotificationController(INotificationManager notificationManager) {
            _notificationManager = notificationManager;
        }
        [HttpGet]
        [Route("Count")]
        [Authorize]
        [ManagerEmployeeAuth]
        public ActionResult<int> GetUnseenNotificationsCount() {
            int.TryParse(User.FindFirst("UserId").Value, out int callerId);
            int unseenTasksCount = _notificationManager.GetUnseenNotificationsCount(callerId);
            return Ok(unseenTasksCount);
        }
        [HttpGet]
        [Route("{status}")]
        [Authorize]
        [ManagerEmployeeAuth]
        [PaginationValidator]
        public ActionResult<PaginatedNotificationRes> GetNotifications(int status, [FromQuery] int page, [FromQuery] int limit) {
            int.TryParse(User.FindFirst("UserId").Value, out int callerId);
            PaginatedNotificationRes notifications = _notificationManager.GetUnseenNotificationsList(status, callerId, page, limit);
            return Ok(notifications);
        }
    }
}
