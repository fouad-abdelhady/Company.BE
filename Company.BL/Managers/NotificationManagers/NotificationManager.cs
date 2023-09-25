using Company.BL.Dtos.CommonDtos;
using Company.BL.Dtos.NotificationDtos;
using Company.BL.Dtos.StaffDtos;
using Company.DAL.Data.Models;
using Company.DAL.Repos.Notification;
using Company.DAL.Repos.Task;
namespace Company.BL.Managers.NotificationManagers
{
    public class NotificationManager : INotificationManager
    {

        private readonly INotificationRepo _notificationRepo;
        private readonly ITaskRepo _taskRepo;
        public NotificationManager(INotificationRepo notificationRepo, ITaskRepo taskRepo) {
            _notificationRepo = notificationRepo;
            _taskRepo = taskRepo;
        }

        public int AddNotification(int PosterId, int RecieverId, int TaskId, int NotificationType)
        {
            Notification notification = new Notification() { PosterId = PosterId, RecieverId = RecieverId, TaskId = TaskId, Type = NotificationType };
            int unseenNotificationCount = _notificationRepo.AddNotification(notification);
            return unseenNotificationCount;
        }

        public int GetUnseenNotificationsCount(int callerId)
        {
            int notificationsCount = _notificationRepo.GetUnseenNotificationsCount(callerId);
            return notificationsCount;
        }

        public PaginatedNotificationRes GetUnseenNotificationsList(int status, int callerId, int page, int limit)
        {
            List<Notification> notifications = _notificationRepo.GetNotifications(status, callerId, page, limit);
            if (status == INotificationRepo.UNSEEN && notifications.Any()) {
                UpdateTaskStatus(notifications);
                _notificationRepo.SetUserNotificationsToSeen(callerId);
            }
            var notificationsList = notifications.Select(notification => TransformToNotificationRes(notification)).ToList();
            PageInfo pageInfo;
            if (status == INotificationRepo.UNSEEN)
            {
                pageInfo = new PageInfo(Next: null, Previous: null, PagesCount: 1, Current: 1);
            }
            else {
                int count = _notificationRepo.GetNotificationsCount(callerId);
                int pagesCount = (int)Math.Ceiling((double)count / limit);
                int? nextPage = page >= pagesCount ? null : page + 1;
                int? previous = page <= 1 ? null : page - 1;
                int current = page;
                pageInfo = new PageInfo(Next: nextPage, Previous: previous, PagesCount: pagesCount, Current: current);
            }
            return new PaginatedNotificationRes(
                    PageInfo: pageInfo,
                    Notifications: notificationsList
            );
        }

        public void SendNotification()
        {
            throw new NotImplementedException();
        }

        private void UpdateTaskStatus(List<Notification> notifications)
        {
            int taskInsertion = (int)INotificationRepo.NotificationTypes.TASK_INSERTION;
            foreach (var notification in notifications) {
                if (notification.Type == taskInsertion)
                {
                    _taskRepo.SetTasksToSeen(notification.RecieverId);
                    return;
                }
            }
        }
        private NotificationRes TransformToNotificationRes(Notification notification) {
            return new NotificationRes(
                    Id: notification.Id,
                    Title: GetNotificationTitle(notification.Type, notification.Task).Original,
                    ArTitle: GetNotificationTitle(notification.Type, notification.Task).Translation,
                    Description: GetNotificationDescription(notification.Type, notification.Poster, notification.Task).Original,
                    ArDescription: GetNotificationDescription(notification.Type, notification.Poster, notification.Task).Translation,
                    Poster: FormStaffMemberRead(notification.Poster),
                    CreatedAt: notification.CreatedAt
                );
        }

        private StaffRead FormStaffMemberRead(Staff? poster) => new StaffRead(
                Id: poster.Id,
                Name: poster.FullName,
                Email: poster.EmailAddress,
                Role: poster.Role,
                Image: poster.Image);
        
        private TextTranslation  GetNotificationDescription(int Type, Staff? poster, DAL.Data.Models.Task task)
        {
            foreach (INotificationRepo.NotificationTypes notificationType in Enum.GetValues(typeof(INotificationRepo.NotificationTypes))) { 
                if(Type == (int) notificationType && notificationType == INotificationRepo.NotificationTypes.TASK_INSERTION) 
                    return new TextTranslation(Original: $"The task \"{task.Title} \" Is assigned to you.", Translation: $"لقد تم اضافة المهمه \" {task.ArTitle} \" اليك");
                
                if (Type == (int)notificationType && notificationType == INotificationRepo.NotificationTypes.TASK_UPDATE)
                    return new TextTranslation(Original: $"The task \"{task.Title} \" state updated.", Translation: $"لقد تم تعديل حالة المهمه \" {task.ArTitle} \"");

                if (Type == (int)notificationType && notificationType == INotificationRepo.NotificationTypes.TASK_CHANGE_REQUEST)
                    return new TextTranslation(Original: $"Changes Requested regarding to \"{task.Title} \".", Translation: $"لقد تم طلب تعديل في المهمه \" {task.ArTitle} \"");

                if (Type == (int)notificationType && notificationType == INotificationRepo.NotificationTypes.OTHER_ACTIONS)
                    return new TextTranslation(Original: $"{task.Title}", Translation: $"{task.ArTitle}" );
            }
            return new TextTranslation(Original: "", Translation: "" );
        }

        private TextTranslation GetNotificationTitle(int Type, DAL.Data.Models.Task? task)
        {
            foreach (INotificationRepo.NotificationTypes notificationType in Enum.GetValues(typeof(INotificationRepo.NotificationTypes)))
            {
                if (Type == (int)notificationType && notificationType == INotificationRepo.NotificationTypes.TASK_INSERTION)
                    return new TextTranslation(Original: $"New Task", Translation: $"مهمة جديدة");

                if (Type == (int)notificationType && notificationType == INotificationRepo.NotificationTypes.TASK_UPDATE)
                    return new TextTranslation(Original: $"Task state updated", Translation: $"تم تعديل حالة مهمة");

                if (Type == (int)notificationType && notificationType == INotificationRepo.NotificationTypes.TASK_CHANGE_REQUEST)
                    return new TextTranslation(Original: $"New Changes Requested", Translation: $"تم طلب تعديل");

                if (Type == (int)notificationType && notificationType == INotificationRepo.NotificationTypes.OTHER_ACTIONS)
                    return new TextTranslation(Original: $"{task.Title}", Translation: $"{task.ArTitle}");
            }
            return new TextTranslation(Original: "", Translation: "");
        }
    }
}
