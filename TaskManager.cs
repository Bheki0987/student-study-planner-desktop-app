using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentStudyPlanner
{
    public static class TaskManager
    {
        private static List<Task> _tasks = new List<Task>();
        private static List<Notification> _notifications = new List<Notification>();

        public static void AddTask(Task task)
        {
            _tasks.Add(task);
            CreateNotificationForTask(task);
        }

        public static void RemoveTask(Task task)
        {
            _tasks.Remove(task);
            // Remove associated notifications
            _notifications.RemoveAll(n => n.Title == $"Reminder: {task.Name}");
        }

        public static List<Task> GetAllTasks()
        {
            return _tasks.ToList();
        }

        public static List<Task> GetTasksForDate(DateTime date)
        {
            return _tasks.Where(t => t.Date.Date == date.Date).ToList();
        }

        public static List<Notification> GetNotifications()
        {
            return _notifications.Where(n => !n.IsRead && n.NotificationTime <= DateTime.Now).ToList();
        }

        private static void CreateNotificationForTask(Task task)
        {
            DateTime notificationTime = task.Date.AddMinutes(-30); // Notify 30 minutes before task
            Notification notification = new Notification(
                $"Reminder: {task.Name}",
                $"Your task '{task.Name}' is due soon.",
                notificationTime
            );
            _notifications.Add(notification);
        }
    }
}