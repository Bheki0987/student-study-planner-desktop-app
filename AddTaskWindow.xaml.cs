using StudentStudyPlanner;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StudentStudyPlanner
{
    public partial class AddTaskWindow : Window
    {
        public Task NewTask { get; private set; }
        private Task _taskToEdit;

        public AddTaskWindow()
        {
            InitializeComponent();
            TaskDatePicker.SelectedDate = DateTime.Today;
            TaskTimePeriodComboBox.SelectedIndex = 0; // Default to AM
            TaskPriorityComboBox.SelectedIndex = 1; // Default to Medium priority
            TaskCategoryComboBox.SelectedIndex = 0; // Default to first category
            TaskColorComboBox.SelectedIndex = 0; // Default to first color
        }

        public AddTaskWindow(Task taskToEdit) : this()
        {
            _taskToEdit = taskToEdit;
            Title = "Edit Task";
            LoadTaskData();
        }

        private void LoadTaskData()
        {
            TaskNameTextBox.Text = _taskToEdit.Name;
            TaskDatePicker.SelectedDate = _taskToEdit.Date;
            SetTimeControls(_taskToEdit.DueTime);
            TaskPriorityComboBox.SelectedIndex = (int)_taskToEdit.Priority;
            TaskCategoryComboBox.SelectedItem = _taskToEdit.Category;
            TaskNotesTextBox.Text = _taskToEdit.Notes;
            TaskColorComboBox.SelectedItem = _taskToEdit.Color;
        }

        private void SetTimeControls(TimeSpan time)
        {
            int hour = time.Hours;
            bool isPM = hour >= 12;
            if (isPM && hour > 12)
            {
                hour -= 12;
            }
            if (hour == 0)
            {
                hour = 12;
            }
            TaskHourTextBox.Text = hour.ToString("D2");
            TaskMinuteTextBox.Text = time.Minutes.ToString("D2");
            TaskTimePeriodComboBox.SelectedIndex = isPM ? 1 : 0;
        }

        private TimeSpan GetSelectedTime()
        {
            if (int.TryParse(TaskHourTextBox.Text, out int hour) &&
                int.TryParse(TaskMinuteTextBox.Text, out int minute))
            {
                if (TaskTimePeriodComboBox.SelectedIndex == 1) // PM
                {
                    if (hour < 12)
                    {
                        hour += 12;
                    }
                }
                else // AM
                {
                    if (hour == 12)
                    {
                        hour = 0;
                    }
                }
                return new TimeSpan(hour, minute, 0);
            }
            return new TimeSpan();
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskNameTextBox.Text) || !TaskDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please enter a task name and select a date.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TaskHourTextBox.Text, out int hour) || hour < 1 || hour > 12 ||
                !int.TryParse(TaskMinuteTextBox.Text, out int minute) || minute < 0 || minute > 59)
            {
                MessageBox.Show("Please enter a valid time.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewTask = new Task
            {
                Name = TaskNameTextBox.Text,
                Date = TaskDatePicker.SelectedDate.Value,
                DueTime = GetSelectedTime(),
                Priority = (TaskPriority)TaskPriorityComboBox.SelectedIndex,
                Category = (TaskCategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Other",
                Notes = TaskNotesTextBox.Text,
                Color = (TaskColorComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Red",
                IsCompleted = _taskToEdit?.IsCompleted ?? false
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
}

public class Task
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan DueTime { get; set; }
    public TaskPriority Priority { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public string Notes { get; set; }
    public bool IsCompleted { get; set; }
    public bool NotificationSent { get; set; }

    public DateTime DueDateTime => Date.Add(DueTime);
}