using StudentStudyPlanner.Views;
using System.Windows;

namespace StudentStudyPlanner
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}