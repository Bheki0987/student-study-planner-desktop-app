using System;
using System.Windows;

namespace StudentStudyPlanner.Services
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : Window;
        void Close();
    }

    public class NavigationService : INavigationService
    {
        private Window _currentWindow;

        public void NavigateTo<T>() where T : Window
        {
            var window = Activator.CreateInstance<T>();
            window.Show();
            _currentWindow?.Close();
            _currentWindow = window;
        }

        public void Close()
        {
            _currentWindow?.Close();
            _currentWindow = null;
        }
    }
}