using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace StudentStudyPlanner
{
    public static class TaskManager
    {
        private static List<Task> _tasks = new List<Task>();
        private const string TasksFilePath = "tasks.xml";

        static TaskManager()
        {
            LoadTasks();
        }

        public static void AddTask(Task task)
        {
            _tasks.Add(task);
            SaveTasks();
        }

        public static void RemoveTask(Task task)
        {
            _tasks.Remove(task);
            SaveTasks();
        }

        public static void UpdateTask(Task oldTask, Task newTask)
        {
            int index = _tasks.IndexOf(oldTask);
            if (index != -1)
            {
                _tasks[index] = newTask;
                SaveTasks();
            }
        }

        public static List<Task> GetAllTasks()
        {
            return _tasks.ToList();
        }

        public static List<Task> GetTasksForDate(DateTime date)
        {
            return _tasks.Where(t => t.Date.Date == date.Date && !t.IsCompleted).ToList();
        }

        public static int GetTotalTaskCount()
        {
            return _tasks.Count;
        }

        public static int GetCompletedTaskCount()
        {
            return _tasks.Count(t => t.IsCompleted);
        }

        private static void SaveTasks()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Task>));
            using (StreamWriter writer = new StreamWriter(TasksFilePath))
            {
                serializer.Serialize(writer, _tasks);
            }
        }

        private static void LoadTasks()
        {
            if (File.Exists(TasksFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Task>));
                using (StreamReader reader = new StreamReader(TasksFilePath))
                {
                    _tasks = (List<Task>)serializer.Deserialize(reader);
                }
            }
        }
    }
}