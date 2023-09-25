using Company.BL.Dtos.NotificationDtos;
using Company.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Managers.NotificationManagers
{
    public interface INotificationManager
    {
        public enum NotificationTypes
        {
            TASK_INSERTION = 0,
            TASK_UPDATE = 1,
            TASK_CHANGE_REQUEST = 3,
            OTHER_ACTIONS = 4
        }
        int GetUnseenNotificationsCount(int callerId);
        PaginatedNotificationRes GetUnseenNotificationsList(int status, int callerId, int page, int limit);
        void SendNotification();
        public int AddNotification(int PosterId, int RecieverId, int TaskId, int NotificationType);
    }
}
