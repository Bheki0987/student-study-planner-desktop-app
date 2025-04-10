using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace StudentStudyPlanner
{
    public class NotificationService
    {
        private DispatcherTimer _timer;
        private List<Notification> _notifications = new List<Notification>();

        public NotificationService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1); // Check for notifications every minute
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var currentTime = DateTime.Now;
            var dueNotifications = _notifications
                .Where(n => !n.IsRead && n.NotificationTime <= currentTime)
                .ToList();

            foreach (var notification in dueNotifications)
            {
                ShowNotification(notification);
                notification.IsRead = true;
            }

            // Remove read notifications
            _notifications.RemoveAll(n => n.IsRead);
        }

        private void ShowNotification(Notification notification)
        {
            MessageBox.Show(notification.Message, notification.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotificationForTask(Task task)
        {
            if (task.DueTime.HasValue)
            {
                DateTime notificationTime = task.Date.Add(task.DueTime.Value).AddMinutes(-30); // Notify 30 minutes before task
                Notification notification = new Notification(
                    $"Reminder: {task.Name}",
                    $"Your task '{task.Name}' is due soon.",
                    notificationTime
                );
                AddNotification(notification);
            }
            else
            {
                // If no specific time is set, notify at the start of the task's date
                DateTime notificationTime = task.Date.Date;
                Notification notification = new Notification(
                    $"Reminder: {task.Name}",
                    $"You have a task '{task.Name}' scheduled for today.",
                    notificationTime
                );
                AddNotification(notification);
            }
        }
    }
}