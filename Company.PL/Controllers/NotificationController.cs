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
        public ActionResult<List<NotificationRes>> GetNotifications(int status) {
            int.TryParse(User.FindFirst("UserId").Value, out int callerId);
            List<NotificationRes> notifications = _notificationManager.GetUnseenNotificationsList(status, callerId);
            return Ok(notifications);
        }
    }
}
