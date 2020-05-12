using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LoginForm1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DataClasses1DataContext dataContext = new DataClasses1DataContext();
        University university = new University();
        Student student = new Student();
        UniversityManager universityManager = new UniversityManager();
        public Window1()
        {
            InitializeComponent();
            ShowUniversity();
            ShowStudents();
            ShowAssociatedStudents();
        }
        //Codes related to University
        //Displays university in listuniversity listbox.
        public void ShowUniversity()
        {
            var showUniversity = from names in dataContext.Universities select names;
            foreach (var names in showUniversity)
            {
                ListUniversity.ItemsSource = showUniversity;
                ListUniversity.DisplayMemberPath = "Location";
                ListUniversity.SelectedValuePath = "UniversityId";
            }
        }
        private void ListUniversity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowUniversity();
            ShowAssociatedStudents();
            ShowUniversityInTextbox();
        }
        //Adds university from the values provided in textbox.
        private void AddUniversity_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(MyTextBox_1.Text != null && MyTextBox_2 != null)
            {
                university = new University()
                {
                    UniversityId = int.Parse(MyTextBox_1.Text),
                    Location = MyTextBox_2.Text
                };
                dataContext.Universities.InsertOnSubmit(university);
                dataContext.SubmitChanges();
                ShowUniversity();
            }
            else
            {
                MessageBox.Show("Incorrect Format.");
            }
        }
        //Deletes selected university in database/listbox
        private void DeleteUniversity_Btn_Click(object sender, RoutedEventArgs e)
        {
            var deleteUniversity = from name in dataContext.Universities where name == ListUniversity.SelectedItem select name;
            foreach(var name in deleteUniversity)
            {
                dataContext.Universities.DeleteOnSubmit(name);
                dataContext.SubmitChanges();
                ShowUniversity();
            }
        }
        
        //Codes related to students
        //Displays all students in student listbox
        public void ShowStudents()
        {
            var showStudents = from s in dataContext.Students select s;
            foreach (var s in showStudents)
            {
                ListStudents.ItemsSource = showStudents;
                ListStudents.SelectedValuePath = "StudentId";
                ListStudents.DisplayMemberPath = "Name";
            }
        }
        private void ListStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowStudents();
            ShowStudentyInTextbox();
        }
        //Adds student from values provided in text boxes
        private void AddStudent_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(MyTextBox_1.Text != null && MyTextBox_2.Text != null)
            {
                student = new Student()
                {
                    StudentId = int.Parse(MyTextBox_1.Text),
                    Name = MyTextBox_2.Text
                };
                dataContext.Students.InsertOnSubmit(student);
                dataContext.SubmitChanges();
                ShowStudents();
            }
            else
            {
                MessageBox.Show("Incorrect Format.");
            }
        }
        //Deletes selected student in database/listbox
        private void DeleteStudent_Btn_Click(object sender, RoutedEventArgs e)
        {
            var deleteStudent = dataContext.Students.Where(d => d == ListStudents.SelectedItem);
            foreach(var d in deleteStudent)
            {
                dataContext.Students.DeleteOnSubmit(d);
                dataContext.SubmitChanges();
                ShowStudents();
            }
        }
        //Codes related to associated students
        //Displays students from selected university
        public void ShowAssociatedStudents()
        {
            if(ListUniversity.SelectedItem != null)
            {
                var showAssociatedStudents = from S in dataContext.Students
                                             join uM in dataContext.UniversityManagers on S.StudentId equals uM.StdFK
                                             join U in dataContext.Universities on uM.UniFK equals U.UniversityId
                                             where uM.UniFK == int.Parse(ListUniversity.SelectedValue.ToString())
                                             select uM.Student;
                foreach (var S in showAssociatedStudents)
                { 
                    ListAssociatedStudents.ItemsSource = showAssociatedStudents;
                    ListAssociatedStudents.DisplayMemberPath = "Name";
                    ListAssociatedStudents.SelectedValuePath = "StudentId";
                }
            }
        }
        //Adds selected student in selected university
        private void AddAssociatedStudent_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(ListUniversity.SelectedItem != null && ListStudents.SelectedItem != null)
            {
                universityManager = new UniversityManager()
                {
                    UniFK = int.Parse(ListUniversity.SelectedValue.ToString()),
                    StdFK = int.Parse(ListStudents.SelectedValue.ToString())
                };
                dataContext.UniversityManagers.InsertOnSubmit(universityManager);
                dataContext.SubmitChanges();
                ShowAssociatedStudents();
            }
        }
        //Removes selected student from associated student table
        private void RemoveAssociatedStudent_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(ListAssociatedStudents.SelectedItem != null)
            {
                var removeQuery = from r in dataContext.UniversityManagers
                                  where r.UniFK == int.Parse(ListUniversity.SelectedValue.ToString()) && r.StdFK == int.Parse(ListAssociatedStudents.SelectedValue.ToString())
                                  select r;
                foreach (var r in removeQuery)
                {
                    dataContext.UniversityManagers.DeleteOnSubmit(r);
                    dataContext.SubmitChanges();
                    ShowAssociatedStudents();
                }
            }
            else
            {
                MessageBox.Show("Failed.");
            }
        }
        //Display selected university or Student in text box
        public void ShowUniversityInTextbox()
        {
            if(ListUniversity.SelectedItem != null)
            {
                var showQuery = from s in dataContext.Universities
                                where s == ListUniversity.SelectedItem
                                select s;
                foreach(var s in showQuery)
                {
                    MyTextBox_1.Text = s.UniversityId.ToString();
                    MyTextBox_2.Text = s.Location;
                }
            }
        }
        public void ShowStudentyInTextbox()
        {
            if (ListStudents.SelectedItem != null)
            {
                var showQuery = from s in dataContext.Students
                                where s == ListStudents.SelectedItem
                                select s;
                foreach (var s in showQuery)
                {
                    MyTextBox_1.Text = s.StudentId.ToString();
                    MyTextBox_2.Text = s.Name;
                }
            }
        }
    }
}
