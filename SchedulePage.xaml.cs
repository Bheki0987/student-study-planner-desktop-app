using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;

namespace StudentStudyPlanner
{
    public partial class SchedulePage : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Task> _todayTasks;
        private ObservableCollection<Task> _tomorrowTasks;
        private ObservableCollection<Task> _upcomingTasks;
        private NotificationService _notificationService;

        public ObservableCollection<Task> TodayTasks
        {
            get => _todayTasks;
            set { _todayTasks = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Task> TomorrowTasks
        {
            get => _tomorrowTasks;
            set { _tomorrowTasks = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Task> UpcomingTasks
        {
            get => _upcomingTasks;
            set { _upcomingTasks = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SchedulePage()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize task collections
            TodayTasks = new ObservableCollection<Task>();
            TomorrowTasks = new ObservableCollection<Task>();
            UpcomingTasks = new ObservableCollection<Task>();

            // Load tasks from TaskManager
            LoadTasks();

            // Initialize notification service
            _notificationService = new NotificationService();
        }

        private void LoadTasks()
        {
            var allTasks = TaskManager.GetAllTasks();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            TodayTasks = new ObservableCollection<Task>(allTasks.Where(t => t.Date.Date == today));
            TomorrowTasks = new ObservableCollection<Task>(allTasks.Where(t => t.Date.Date == tomorrow));
            UpcomingTasks = new ObservableCollection<Task>(allTasks.Where(t => t.Date.Date > tomorrow));

            OnPropertyChanged(nameof(TodayTasks));
            OnPropertyChanged(nameof(TomorrowTasks));
            OnPropertyChanged(nameof(UpcomingTasks));
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow();
            if (addTaskWindow.ShowDialog() == true)
            {
                Task newTask = addTaskWindow.NewTask;
                TaskManager.AddTask(newTask);
                LoadTasks();
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            Task taskToEdit = ((FrameworkElement)sender).DataContext as Task;
            AddTaskWindow editTaskWindow = new AddTaskWindow(taskToEdit);
            if (editTaskWindow.ShowDialog() == true)
            {
                Task updatedTask = editTaskWindow.NewTask;
                UpdateTask(taskToEdit, updatedTask);
            }
        }

        private void UpdateTask(Task oldTask, Task newTask)
        {
            // Remove the old task and add the updated task
            TaskManager.RemoveTask(oldTask);
            TaskManager.AddTask(newTask);
            LoadTasks();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Task taskToDelete = ((FrameworkElement)sender).DataContext as Task;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TaskManager.RemoveTask(taskToDelete);
                LoadTasks();
            }
        }

        private void ViewCalendar_Click(object sender, RoutedEventArgs e)
        {
            CalendarPage calendarPage = new CalendarPage();
            calendarPage.Show();
            this.Close();
        }

        private void OpenStudyTimer_Click(object sender, RoutedEventArgs e)
        {
            StudyTimerWindow timerWindow = new StudyTimerWindow();
            timerWindow.Show();
        }

        private void OpenNotesPage_Click(object sender, RoutedEventArgs e)
        {
            NotesPage notesPage = new NotesPage();
            notesPage.Show();
        }
    }

   
}