using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StudentStudyPlanner
{
    public partial class AddTaskPage : UserControl
    {
        public Task NewTask { get; private set; }
        private Task _taskToEdit;

        public event EventHandler<Task> TaskSaved;
        public event EventHandler TaskCancelled;

        public AddTaskPage()
        {
            InitializeComponent();
            TaskDatePicker.SelectedDate = DateTime.Today;
            TaskTimePeriodComboBox.SelectedIndex = 0; // Default to AM
            TaskPriorityComboBox.SelectedIndex = 1; // Default to Medium priority
            TaskCategoryComboBox.SelectedIndex = 0; // Default to first category
            TaskColorPicker.SelectedColor = Colors.Red; // Default color
        }

        public AddTaskPage(Task taskToEdit) : this()
        {
            _taskToEdit = taskToEdit;
            LoadTaskData();
        }

        private void LoadTaskData()
        {
            TaskNameTextBox.Text = _taskToEdit.Name;
            TaskDatePicker.SelectedDate = _taskToEdit.Date;
            SetTimeControls(_taskToEdit.DueTime);
            TaskPriorityComboBox.SelectedIndex = (int)_taskToEdit.Priority;
            TaskCategoryComboBox.SelectedItem = _taskToEdit.Category;
            TaskColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(_taskToEdit.Color);
        }

        private void SetTimeControls(TimeSpan? time)
        {
            if (time.HasValue)
            {
                int hour = time.Value.Hours;
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
                TaskMinuteTextBox.Text = time.Value.Minutes.ToString("D2");
                TaskTimePeriodComboBox.SelectedIndex = isPM ? 1 : 0;
            }
            else
            {
                TaskHourTextBox.Text = "";
                TaskMinuteTextBox.Text = "";
                TaskTimePeriodComboBox.SelectedIndex = 0;
            }
        }

        private TimeSpan? GetSelectedTime()
        {
            if (string.IsNullOrWhiteSpace(TaskHourTextBox.Text) || string.IsNullOrWhiteSpace(TaskMinuteTextBox.Text))
            {
                return null;
            }

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
            return null;
        }

        private void TaskColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // This method is called when the color is changed in the ColorPicker
            // You can add any additional logic here if needed
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                NewTask = new Task
                {
                    Name = TaskNameTextBox.Text,
                    Date = TaskDatePicker.SelectedDate.Value,
                    DueTime = GetSelectedTime(),
                    Priority = (TaskPriority)TaskPriorityComboBox.SelectedIndex,
                    Category = (TaskCategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Other",
                    Color = TaskColorPicker.SelectedColor.HasValue ? TaskColorPicker.SelectedColor.Value.ToString() : "#FF0000",
                    IsCompleted = _taskToEdit?.IsCompleted ?? false
                };

                TaskSaved?.Invoke(this, NewTask);
                CloseAddTaskPage();
            }
        }

       

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TaskNameTextBox.Text) || !TaskDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please enter a task name and select a date.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(TaskHourTextBox.Text) || !string.IsNullOrWhiteSpace(TaskMinuteTextBox.Text))
            {
                if (!int.TryParse(TaskHourTextBox.Text, out int hour) || hour < 1 || hour > 12 ||
                    !int.TryParse(TaskMinuteTextBox.Text, out int minute) || minute < 0 || minute > 59)
                {
                    MessageBox.Show("Please enter a valid time.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            TaskCancelled?.Invoke(this, EventArgs.Empty);
            CloseAddTaskPage();
        }

        private void CloseAddTaskPage()
        {
            // Find the parent window and close it
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}