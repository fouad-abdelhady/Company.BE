using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Repos.Notification
{
    public interface INotificationRepo
    {
        public const int UNSEEN = 0;
        public const int SEEN = 1;

        ///Notification types
        public enum NotificationTypes
        {
            TASK_INSERTION = 0,
            TASK_UPDATE = 1,
            TASK_CHANGE_REQUEST = 3,
            OTHER_ACTIONS = 4
    }
        List<Data.Models.Notification> GetNotifications(int status, int callerId, int page, int limit);
        int GetUnseenNotificationsCount(int callerId);
        int AddNotification(Data.Models.Notification newNotification);
        bool SetUserNotificationsToSeen(int recieverId);
        int GetNotificationsCount(int callerId);
    }
}
