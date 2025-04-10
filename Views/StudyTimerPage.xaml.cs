using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StudentStudyPlanner
{
    public partial class StudyTimerPage : UserControl
    {
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        private bool isWorkSession = true;
        private int focusMinutes = 25;
        private int breakMinutes = 5;
        private DateTime sessionStartTime;

        public StudyTimerPage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            ResetTimer();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft == TimeSpan.Zero)
            {
                FinishSession();
            }
            else
            {
                timeLeft = timeLeft.Add(TimeSpan.FromSeconds(-1));
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            TimerDisplay.Text = timeLeft.ToString(@"mm\:ss");
        }

        private void FinishSession()
        {
            timer.Stop();
            if (isWorkSession)
            {
                StatusDisplay.Text = "Focus session finished. Take a break!";
                timeLeft = TimeSpan.FromMinutes(breakMinutes);
                isWorkSession = false;

                StudyTimerManager.AddStudySession(sessionStartTime, DateTime.Now);
            }
            else
            {
                StatusDisplay.Text = "Break finished. Ready to focus?";
                timeLeft = TimeSpan.FromMinutes(focusMinutes);
                isWorkSession = true;
            }
            UpdateDisplay();
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(FocusTimeTextBox.Text, out int newFocusMinutes) &&
                int.TryParse(BreakTimeTextBox.Text, out int newBreakMinutes))
            {
                focusMinutes = newFocusMinutes;
                breakMinutes = newBreakMinutes;
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for focus and break times.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            timer.Start();
            StartButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
            StatusDisplay.Text = isWorkSession ? "Focus session in progress" : "Break in progress";

            if (isWorkSession)
            {
                sessionStartTime = DateTime.Now;
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            StatusDisplay.Text = "Paused";

            if (isWorkSession)
            {
                StudyTimerManager.AddStudySession(sessionStartTime, DateTime.Now);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
        }

        private void ResetTimer()
        {
            timer.Stop();
            isWorkSession = true;
            if (int.TryParse(FocusTimeTextBox.Text, out int newFocusMinutes))
            {
                focusMinutes = newFocusMinutes;
            }
            timeLeft = TimeSpan.FromMinutes(focusMinutes);
            UpdateDisplay();
            StatusDisplay.Text = "Ready to start";
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }
    }
}