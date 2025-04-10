using StudentStudyPlanner.Views;
using System.Windows;
using System.Windows.Controls;

namespace StudentStudyPlanner
{
    public partial class MainWindow : Window
    {
        private SchedulePage schedulePage;
        private CalendarPage calendarPage;
        private NotesPage notesPage;
        private StudyTimerPage studyTimerPage;
        private ProgressTrackingPage progressTrackingPage;



        public MainWindow()
        {
            InitializeComponent();
            progressTrackingPage = new ProgressTrackingPage();
            schedulePage = new SchedulePage(progressTrackingPage);
        }

        public void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        public void NavigateToSchedule(object sender, RoutedEventArgs e)
        {
            if (schedulePage == null)
            {
                schedulePage = new SchedulePage(progressTrackingPage);
            }
            schedulePage.LoadTasks();
            MainFrame.Navigate(schedulePage);
        }

        public void NavigateToCalendar(object sender, RoutedEventArgs e)
        {
            if (calendarPage == null)
            {
                calendarPage = new CalendarPage();
            }
            calendarPage.RefreshCalendar();
            MainFrame.Navigate(calendarPage);
        }

        public void NavigateToNotes(object sender, RoutedEventArgs e)
        {
            if (notesPage == null)
            {
                notesPage = new NotesPage();
            }
            MainFrame.Navigate(notesPage);
        }

        public void NavigateToStudyTimer(object sender, RoutedEventArgs e)
        {
            if (studyTimerPage == null)
            {
                studyTimerPage = new StudyTimerPage();
            }
            MainFrame.Navigate(studyTimerPage);
        }

        public void NavigateToProgressTracking(object sender, RoutedEventArgs e)
        {
            if (progressTrackingPage == null)
            {
                progressTrackingPage = new ProgressTrackingPage();
            }
            progressTrackingPage.UpdateProgress();
            MainFrame.Navigate(progressTrackingPage);
        }

        public void RefreshCalendarPage()
        {
            if (calendarPage != null)
            {
                calendarPage.RefreshCalendar();
            }
        }
    }
}