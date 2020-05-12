using System.Windows;
using System.Configuration;
using System.Linq;
using System;

namespace LoginForm1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext dataContext;
        StaffLogin staff = new StaffLogin();
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["LoginForm1.Properties.Settings.KanchanDBConnectionString"].ConnectionString;
            dataContext = new DataClasses1DataContext(connectionString);
        }
        //Another window should pop out after verification from login btn
        private void Login_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = dataContext.StaffLogins.Where(i => i.Staff_ID.ToString() == EnterIdHere.Text && i.Password == EnterPasswordHere.Text);
                if(user != null )
                {
                    Window1 window = new Window1();
                    window.Show();
                    this.Hide();
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    } 
}
