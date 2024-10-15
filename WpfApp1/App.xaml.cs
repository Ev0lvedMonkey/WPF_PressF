using System.Windows;
using System.Windows.Navigation;
using WpfApp1.Database;
using WpfApp1;
using System.Data.Entity;
using System.Linq;
using System.Windows.Controls;
using WpfApp1.Pages.RolesPages;
using WpfApp1.Pages;
using System;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FactoryDBEntities DB = new FactoryDBEntities();
        public static User CurrentUser { get; private set; }
        public static string CurrentUserRole { get; private set; }

        public enum Roles { Customer, Director, Manager, Master, Сonstructor}


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadUserSettings();
        }

        public static bool SetNewUser(User user)
        {
            if (user == null)
            {
                MessageBox.Show("Пользователь нулевый");
                return false;
            }
            else
            {
                CurrentUser = user;
                CurrentUserRole = DB.Role.FirstOrDefault(x => x.RoleID == CurrentUser.RoleID).RoleName;
                return true;
            }
        }



        public static void SaveUserSettings(User user)
        {
            WpfApp1.Properties.Settings.Default.UserID = user.UserID;
            WpfApp1.Properties.Settings.Default.Save();
        }

        public static void LoadUserSettings()
        {
            if (WpfApp1.Properties.Settings.Default.UserID != 0)
                CurrentUser = DB.User.FirstOrDefault(x => x.UserID == WpfApp1.Properties.Settings.Default.UserID);
        }

        public static void ClearUserSettings()
        {
            WpfApp1.Properties.Settings.Default.UserID = 0;
            WpfApp1.Properties.Settings.Default.Save();
        }

        public static Page GetRightPage()
        {
            switch (CurrentUser.UserID)
            {
                case 1:
                    return new DirectorPage();
                case 2:
                    return new СonstructorPage();
                case 3:
                    return new ManagerPage();
                case 4:
                    return new MasterPage();
                case 5:
                    return new CustomerPage();
                default:
                    return new AuthPage();
            }
        }

        public static void RemoveCurrentUser() =>
            CurrentUser = null;

        //RedColor = "#bb0c07";
        //BlueColor = "#4867ac";
        //BeigeColor = "#f3cc8d";
        //LightBeigeColor = "#fef9de";
        //LightGrayColor = "#f1f1f1";
    }
}
