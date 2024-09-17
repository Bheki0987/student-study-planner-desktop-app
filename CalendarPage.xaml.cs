using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace StudentStudyPlanner
{
    public partial class CalendarPage : Window
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

        private void ViewSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic to view the schedule
            SchedulePage schedulePage = new SchedulePage();
            schedulePage.Show();
            this.Close();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic to add a task
             AddTaskWindow addTaskWindow = new AddTaskWindow();
             if (addTaskWindow.ShowDialog() == true)
            {
                 Task newTask = addTaskWindow.NewTask;
                 TaskManager.AddTask(newTask);
                 UpdateCalendar();
             }
        }

        private void UpdateCalendar()
        {
            MonthYearText.Text = currentMonth.ToString("MMMM yyyy");

            CalendarGrid.Children.Clear();
            weekNumbers.Clear();

            // Add day headers
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

                // Add week number
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

                    foreach (var task in tasksForDay.Take(3)) // Limit to 3 indicators
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
                string tasks = string.Join("\n", tasksForDay.Select(t => $"{t.Name} at {t.DueTime:hh\\:mm}"));
                MessageBox.Show($"Tasks for {date:d}:\n\n{tasks}", "Day Tasks", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"No tasks for {date:d}", "Day Tasks", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

}