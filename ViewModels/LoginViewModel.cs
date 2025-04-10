using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentStudyPlanner.Commands;

namespace StudentStudyPlanner.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        private bool CanLogin(object obj) => !string.IsNullOrWhiteSpace(Username);
        private bool CanRegister(object obj) => !string.IsNullOrWhiteSpace(Username);

        private void Login(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox?.Password;

            if (UserManager.AuthenticateUser(Username, password))
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox?.Password;

            var (success, message) = UserManager.RegisterUser(Username, password);
            if (success)
            {
                MessageBox.Show(message, "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(message, "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}