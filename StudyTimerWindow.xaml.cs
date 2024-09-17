using System;
using System.Windows;
using System.Windows.Threading;

namespace StudentStudyPlanner
{
    public partial class StudyTimerWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        private bool isWorkSession = true;
        private const int WorkMinutes = 25;
        private const int BreakMinutes = 5;

        public StudyTimerWindow()
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
                StatusDisplay.Text = "Work session finished. Take a break!";
                timeLeft = TimeSpan.FromMinutes(BreakMinutes);
                isWorkSession = false;
            }
            else
            {
                StatusDisplay.Text = "Break finished. Ready to work?";
                timeLeft = TimeSpan.FromMinutes(WorkMinutes);
                isWorkSession = true;
            }
            UpdateDisplay();
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            StartButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
            StatusDisplay.Text = isWorkSession ? "Work session in progress" : "Break in progress";
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            StatusDisplay.Text = "Paused";
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
        }

        private void ResetTimer()
        {
            timer.Stop();
            isWorkSession = true;
            timeLeft = TimeSpan.FromMinutes(WorkMinutes);
            UpdateDisplay();
            StatusDisplay.Text = "Ready to start";
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }
    }
}