using System;
using System.Linq;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace StudentStudyPlanner
{
    public partial class ProgressTrackingPage : UserControl
    {
        public SeriesCollection WeeklyStudyData { get; set; }
        public string[] DaysOfWeek { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public ProgressTrackingPage()
        {
            InitializeComponent();
            InitializeChart();
            LoadProgressData();
        }

        private void InitializeChart()
        {
            DaysOfWeek = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            YFormatter = value => value.ToString("N0") + " min";

            WeeklyStudyData = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Study Time",
                    Values = new ChartValues<double> { 0, 0, 0, 0, 0, 0, 0 }
                }
            };

            DataContext = this;
        }

        public void UpdateProgress()
        {
            LoadProgressData();
            UpdateWeeklyProgressChart();
        }

        private void LoadProgressData()
        {
            TimeSpan todayStudyTime = StudyTimerManager.GetTotalStudyTimeForDay(DateTime.Today);
            TimeSpan weekStudyTime = StudyTimerManager.GetTotalStudyTimeForWeek(DateTime.Today);
            TimeSpan monthStudyTime = StudyTimerManager.GetTotalStudyTimeForMonth(DateTime.Today);

            TodayStudyTime.Text = FormatTimeSpan(todayStudyTime);
            WeekStudyTime.Text = FormatTimeSpan(weekStudyTime);
            MonthStudyTime.Text = FormatTimeSpan(monthStudyTime);

            int completedTasks = TaskManager.GetCompletedTaskCount();
            int totalTasks = TaskManager.GetTotalTaskCount();
            double completionRate = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;

            CompletedTasksCount.Text = completedTasks.ToString();
            TaskCompletionRate.Text = $"{completionRate:F1}%";

            UpdateWeeklyProgressChart();
        }

        private void UpdateWeeklyProgressChart()
        {
            var today = DateTime.Today;
            var weekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var dailyStudyTimes = Enumerable.Range(0, 7)
                .Select(i => StudyTimerManager.GetTotalStudyTimeForDay(weekStart.AddDays(i)).TotalMinutes)
                .ToList();

            WeeklyStudyData[0].Values = new ChartValues<double>(dailyStudyTimes);
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours}h {timeSpan.Minutes}m";
        }
    }
}