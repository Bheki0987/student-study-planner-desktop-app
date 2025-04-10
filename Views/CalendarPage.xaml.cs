using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace StudentStudyPlanner
{
    public partial class CalendarPage : UserControl
    {
        private DateTime currentMonth;
        private ObservableCollection<int> weekNumbers;

        public CalendarPage()
        {
            InitializeComponent();
            currentMonth = DateTime.Today;
            weekNumbers = new ObservableCollection<int>();
            WeekNumberPanel.ItemsSource = weekNumbers;
            UpdateCalendar();
        }

        public void RefreshCalendar()
        {
            UpdateCalendar();
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = currentMonth.AddMonths(-1);
            UpdateCalendar();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = currentMonth.AddMonths(1);
            UpdateCalendar();
        }

        private void Today_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = DateTime.Today;
            UpdateCalendar();
        }

        private void UpdateCalendar()
        {
            MonthYearText.Text = currentMonth.ToString("MMMM yyyy");

            CalendarGrid.Children.Clear();
            weekNumbers.Clear();

            string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < 7; i++)
            {
                CalendarGrid.Children.Add(new TextBlock
                {
                    Text = dayNames[i],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                });
                Grid.SetRow((UIElement)CalendarGrid.Children[CalendarGrid.Children.Count - 1], 0);
                Grid.SetColumn((UIElement)CalendarGrid.Children[CalendarGrid.Children.Count - 1], i);
            }

            var firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);

            int currentWeekNumber = -1;

            for (int i = 1; i <= daysInMonth; i++)
            {
                var currentDate = new DateTime(currentMonth.Year, currentMonth.Month, i);
                int row = (i + (int)firstDayOfMonth.DayOfWeek - 1) / 7 + 1;
                int col = ((int)currentDate.DayOfWeek + 7) % 7;

                int weekNumber = GetIso8601WeekOfYear(currentDate);
                if (weekNumber != currentWeekNumber)
                {
                    currentWeekNumber = weekNumber;
                    weekNumbers.Add(weekNumber);
                }

                var border = new Border
                {
                    Style = (Style)FindResource("DayCellStyle")
                };

                var stackPanel = new StackPanel();
                stackPanel.Children.Add(new TextBlock
                {
                    Text = i.ToString(),
                    Style = (Style)FindResource("DayTextStyle")
                });

                if (currentDate.Date == DateTime.Today.Date)
                {
                    border.Background = Brushes.LightYellow;
                }

                var tasksForDay = TaskManager.GetTasksForDate(currentDate);
                if (tasksForDay.Any())
                {
                    var taskIndicator = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 2, 0, 0)
                    };

                    foreach (var task in tasksForDay.Take(3))
                    {
                        taskIndicator.Children.Add(new Ellipse
                        {
                            Width = 6,
                            Height = 6,
                            Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(task.Color)),
                            Margin = new Thickness(1, 0, 1, 0)
                        });
                    }

                    stackPanel.Children.Add(taskIndicator);
                }

                border.Child = stackPanel;
                Grid.SetRow(border, row);
                Grid.SetColumn(border, col);
                CalendarGrid.Children.Add(border);

                border.MouseEnter += (sender, e) => border.Background = Brushes.LightGray;
                border.MouseLeave += (sender, e) => border.Background = currentDate.Date == DateTime.Today.Date ? Brushes.LightYellow : Brushes.White;

                border.MouseLeftButtonDown += (sender, e) => ShowTasksForDay(currentDate);
            }
        }

        private static int GetIso8601WeekOfYear(DateTime date)
        {
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private void ShowTasksForDay(DateTime date)
        {
            var tasksForDay = TaskManager.GetTasksForDate(date);
            if (tasksForDay.Any())
            {
                var taskWindow = new Window
                {
                    Title = $"Tasks for {date:d}",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Window.GetWindow(this)
                };

                var stackPanel = new StackPanel { Margin = new Thickness(10) };

                foreach (var task in tasksForDay)
                {
                    var taskPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
                    taskPanel.Children.Add(new TextBlock { Text = $"{task.Name}", VerticalAlignment = VerticalAlignment.Center });

                    var editButton = new Button { Content = "Edit", Margin = new Thickness(10, 0, 0, 0) };
                    editButton.Click += (sender, e) => EditTask(task, taskWindow);
                    taskPanel.Children.Add(editButton);

                    var deleteButton = new Button { Content = "Delete", Margin = new Thickness(10, 0, 0, 0) };
                    deleteButton.Click += (sender, e) => DeleteTask(task, taskWindow);
                    taskPanel.Children.Add(deleteButton);

                    stackPanel.Children.Add(taskPanel);
                }

                taskWindow.Content = stackPanel;
                taskWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show($"No tasks for {date:d}", "Day Tasks", MessageBoxButton.OK, MessageBoxImage.Information);
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
                UpdateCalendar();
                editTaskWindow.Close();
                parentWindow.Close();
            };
            editTaskPage.TaskCancelled += (s, args) => editTaskWindow.Close();

            editTaskWindow.Content = editTaskPage;
            editTaskWindow.ShowDialog();
        }

        private void DeleteTask(Task task, Window parentWindow)
        {
            var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                TaskManager.RemoveTask(task);
                UpdateCalendar();
                parentWindow.Close();
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
                UpdateCalendar();
                addTaskWindow.Close();
            };
            addTaskPage.TaskCancelled += (s, args) => addTaskWindow.Close();

            addTaskWindow.Content = addTaskPage;
            addTaskWindow.ShowDialog();
        }
    }
}