using System;

namespace StudentStudyPlanner
{
    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime NotificationTime { get; set; }
        public bool IsRead { get; set; }

        public Notification(string title, string message, DateTime notificationTime)
        {
            Title = title;
            Message = message;
            NotificationTime = notificationTime;
            IsRead = false;
        }
    }
}