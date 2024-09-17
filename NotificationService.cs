using System;
using System.Windows;
using System.Windows.Threading;

namespace StudentStudyPlanner
{
    public class NotificationService
    {
        private DispatcherTimer _timer;

        public NotificationService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1); // Check for notifications every minute
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var notifications = TaskManager.GetNotifications();
            foreach (var notification in notifications)
            {
                ShowNotification(notification);
                notification.IsRead = true;
            }
        }

        private void ShowNotification(Notification notification)
        {
            MessageBox.Show(notification.Message, notification.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
