using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace StudentStudyPlanner
{
    public partial class SchedulePage : UserControl
    {
        private List<Task> allTasks;
        private ProgressTrackingPage progressTrackingPage;

        public SchedulePage(ProgressTrackingPage progressTracking)
        {
            InitializeComponent();
            progressTrackingPage = progressTracking;
            LoadTasks();
        }

        public void LoadTasks()
        {
            allTasks = TaskManager.GetAllTasks();
            ApplyFiltersAndSort();
        }

        private void ApplyFiltersAndSort()
        {
            IEnumerable<Task> filteredTasks = allTasks;

            // Apply filters
            switch (FilterComboBox.SelectedIndex)
            {
                case 1: // Today's Tasks
                    filteredTasks = filteredTasks.Where(t => t.Date.Date == DateTime.Today);
                    break;
                case 2: // This Week's Tasks
                    var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(7);
                    filteredTasks = filteredTasks.Where(t => t.Date >= startOfWeek && t.Date < endOfWeek);
                    break;
                case 3: // Completed Tasks
                    filteredTasks = filteredTasks.Where(t => t.IsCompleted);
                    break;
                case 4: // Incomplete Tasks
                    filteredTasks = filteredTasks.Where(t => !t.IsCompleted);
                    break;
            }

            // Apply sorting
            switch (SortComboBox.SelectedIndex)
            {
                case 0: // Date (Ascending)
                    filteredTasks = filteredTasks.OrderBy(t => t.Date);
                    break;
                case 1: // Date (Descending)
                    filteredTasks = filteredTasks.OrderByDescending(t => t.Date);
                    break;
                case 2: // Priority (High to Low)
                    filteredTasks = filteredTasks.OrderByDescending(t => t.Priority);
                    break;
                case 3: // Priority (Low to High)
                    filteredTasks = filteredTasks.OrderBy(t => t.Priority);
                    break;
            }

            UpdateTaskList(filteredTasks);
        }

        private void UpdateTaskList(IEnumerable<Task> tasks)
        {
            var groupedTasks = tasks
                .GroupBy(t => GetTaskGroup(t))
                .OrderBy(g => g.Key)
                .ToList();

            TasksItemsControl.ItemsSource = null;
            TasksItemsControl.Items.Clear();

            foreach (var group in groupedTasks)
            {
                var groupHeader = new TextBlock
                {
                    Text = group.Key,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Margin = new Thickness(0, 20, 0, 10),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1976D2"))
                };
                TasksItemsControl.Items.Add(groupHeader);

                foreach (var task in group)
                {
                    var taskGrid = new Grid();
                    taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var checkBox = new CheckBox
                    {
                        IsChecked = task.IsCompleted,
                        VerticalAlignment = VerticalAlignment.Center,
                        DataContext = task
                    };
                    checkBox.Checked += NewTaskCheckBox_Checked;
                    checkBox.Unchecked += NewTaskCheckBox_Unchecked;
                    Grid.SetColumn(checkBox, 0);
                    taskGrid.Children.Add(checkBox);

                    var buttonStackPanel = CreateTaskButtons(task);
                    Grid.SetColumn(buttonStackPanel, 1);
                    taskGrid.Children.Add(buttonStackPanel);

                    var border = new Border
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(5),
                        Margin = new Thickness(0, 0, 0, 10),
                        Padding = new Thickness(15),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")),
                        BorderThickness = new Thickness(1),
                        Child = taskGrid
                    };

                    TasksItemsControl.Items.Add(border);
                }
            }
        }


        private string GetTaskGroup(Task task)
        {
            if (task.Date.Date == DateTime.Today)
                return "TODAY'S TASKS";
            if (task.Date.Date == DateTime.Today.AddDays(1))
                return "TOMORROW'S TASKS";
            return "UPCOMING TASKS";
        }

        private void ApplyFiltersAndSort_Click(object sender, RoutedEventArgs e)
        {
            ApplyFiltersAndSort();
        }

        private void NewTaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTaskCompletionStatus(sender, true);
        }

        private void NewTaskCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateTaskCompletionStatus(sender, false);
        }

        private void UpdateTaskCompletionStatus(object sender, bool isCompleted)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Task task)
            {
                task.IsCompleted = isCompleted;
                TaskManager.UpdateTask(task, task);
                LoadTasks();
                RefreshCalendarPage();
                progressTrackingPage.UpdateProgress();
            }
        }

        private void NewEditTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Task taskToEdit)
            {
                var editTaskWindow = new Window
                {
                    Title = "Edit Task",
                    Width = 400,
                    Height = 500,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Window.GetWindow(this)
                };

                var editTaskPage = new AddTaskPage(taskToEdit);
                editTaskPage.TaskSaved += (s, updatedTask) =>
                {
                    TaskManager.UpdateTask(taskToEdit, updatedTask);
                    LoadTasks();
                    RefreshCalendarPage();
                    progressTrackingPage.UpdateProgress();
                    editTaskWindow.Close();
                };
                editTaskPage.TaskCancelled += (s, args) => editTaskWindow.Close();

                editTaskWindow.Content = editTaskPage;
                editTaskWindow.ShowDialog();
            }
        }

        private void EditTask(Task task, Window parentWindow)
        {
            var editTaskWindow = new Window
            {
                Title = "Edit Task",
                Width = 400,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = parentWindow
            };

            var editTaskPage = new AddTaskPage(task);
            editTaskPage.TaskSaved += (s, updatedTask) =>
            {
                TaskManager.UpdateTask(task, updatedTask);
                LoadTasks();
                RefreshCalendarPage();
                progressTrackingPage.UpdateProgress();
                editTaskWindow.Close();
            };
            editTaskPage.TaskCancelled += (s, args) => editTaskWindow.Close();

            editTaskWindow.Content = editTaskPage;
            editTaskWindow.ShowDialog();
        }

        private void NewDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Task taskToDelete)
            {
                var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    TaskManager.RemoveTask(taskToDelete);
                    LoadTasks();
                    RefreshCalendarPage();
                    progressTrackingPage.UpdateProgress();
                }
            }
        }

        private void DeleteTask(Task task)
        {
            var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                TaskManager.RemoveTask(task);
                LoadTasks();
                RefreshCalendarPage();
                progressTrackingPage.UpdateProgress();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new Window
            {
                Title = "Add New Task",
                Width = 400,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this)
            };

            var addTaskPage = new AddTaskPage();
            addTaskPage.TaskSaved += (s, task) =>
            {
                TaskManager.AddTask(task);
                LoadTasks();
                RefreshCalendarPage();
                progressTrackingPage.UpdateProgress();
                addTaskWindow.Close();
            };
            addTaskPage.TaskCancelled += (s, args) => addTaskWindow.Close();

            addTaskWindow.Content = addTaskPage;
            addTaskWindow.ShowDialog();
        }

        private void RefreshCalendarPage()
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.RefreshCalendarPage();
        }

        private StackPanel CreateTaskButtons(Task task)
        {
            var buttonStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            string taskInfoText = $"{task.Name} - Due: {task.FormattedDueTime}";

            var taskInfo = new TextBlock
            {
                Text = taskInfoText,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0),
                FontSize = 14
            };

            var editButton = new Button
            {
                Content = "Edit",
                Margin = new Thickness(5, 0, 5, 0),
                Padding = new Thickness(10, 5, 10, 5),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3")),
                Foreground = Brushes.White,
                Style = (Style)FindResource("ModernButtonStyle")
            };
            editButton.Click += (sender, e) => NewEditTask_Click(sender, e);
            editButton.DataContext = task;

            var deleteButton = new Button
            {
                Content = "Delete",
                Padding = new Thickness(10, 5, 10, 5),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336")),
                Foreground = Brushes.White,
                Style = (Style)FindResource("ModernButtonStyle")
            };
            deleteButton.Click += (sender, e) => NewDeleteTask_Click(sender, e);
            deleteButton.DataContext = task;

            buttonStackPanel.Children.Add(taskInfo);
            buttonStackPanel.Children.Add(editButton);
            buttonStackPanel.Children.Add(deleteButton);

            return buttonStackPanel;
        }

        private void LogError(string message, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {message}. Exception: {ex.Message}");
            // You could also log to a file or show a message to the user here
        }
    }
}