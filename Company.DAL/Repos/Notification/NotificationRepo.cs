using Company.DAL.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Repos.Notification
{
    public class NotificationRepo : INotificationRepo
    {
       

        private readonly CompanyContext _companyContext;
        public NotificationRepo(CompanyContext companyContext)
        {
            _companyContext = companyContext;
        }

        public int AddNotification(Data.Models.Notification newNotification)
        {
            _companyContext.Notifications.Add(newNotification);
            try { 
                _companyContext.SaveChanges();
            } catch (Exception e) { 
                Console.WriteLine(e.ToString());
            }
            return GetUnseenNotificationsCount(newNotification.RecieverId);
        }

        public List<Data.Models.Notification> GetNotifications(int status, int callerId, int page, int limit)
        {
            if(status == 0)
                return _companyContext.Notifications
                    .Include(notification => notification.Poster)
                    .Include(notification => notification.Task)
                    .Where(notification => notification.Status == status && notification.RecieverId == callerId)
                    .OrderByDescending(notification => notification.Id)
                    .ToList();
            int skip = (page - 1) * limit;
            return _companyContext.Notifications
                    .Include(notification => notification.Poster)
                    .Include(notification => notification.Task)
                    .Where(notification => notification.Status == status && notification.RecieverId == callerId)
                    .OrderByDescending(notification => notification.Id)
                    .Skip(skip)
                    .Take(limit)
                    .ToList();
        }
        public int GetUnseenNotificationsCount(int callerId)
        {
            return _companyContext.Notifications
                .Where(notification => notification.RecieverId == callerId && notification.Status == INotificationRepo.UNSEEN)
                .Count();
        }

        public int GetNotificationsCount(int callerId) {
            return _companyContext.Notifications
                .Where(notification => notification.RecieverId == callerId)
                .Count();
        }

        public bool SetUserNotificationsToSeen(int recieverId) {
            var unseenNotificationsList = _companyContext.Notifications.Where(notification => notification.RecieverId == recieverId && notification.Status == INotificationRepo.UNSEEN).ToList();
            foreach (var notification in unseenNotificationsList) {
                notification.Status = INotificationRepo.SEEN;
                notification.StateChangedAt = DateTime.Now;
            }
            try
            {
                _companyContext.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public int SetTaskStatus(int notificationId, int notificationStatus = 1) {
            var notification = _companyContext.Notifications.Include(note=> note.Task).FirstOrDefault(notification => notification.Id == notificationId);
            try
            {
                notification.Status = notificationStatus;
                _companyContext.SaveChanges();
                return notification.TaskId;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return 0;
            }
            

        }
    }
}
