using LangLang.DAO;
using LangLang.Model;
using LangLang.Services;
using LangLang.ViewModel;
using System.Windows;

namespace LangLang.View
{
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            /*CourseDAO cd = CourseDAO.getInstance();

            Language lan = new Language("en", "en1");
            cd.AddCourse(new Course("kurs2", lan, Consts.LanguageLvl.A1, 2, null, System.DateTime.Now, false, 10, Consts.CourseState.Active, 100));
            cd.AddCourse(new Course("kurs3", lan, Consts.LanguageLvl.B1, 2, null, System.DateTime.Now, true, 5, Consts.CourseState.Active, 100));
            */
            /*
            Language lan = new Language("srp", "spr1");
            Language lan2 = new Language("swe", "swe1");

            ExamDAO examDAO = ExamDAO.GetInstance();
            examDAO.AddExam(new Exam(lan, Consts.LanguageLvl.C1, System.DateTime.Now, 122, 100));
            examDAO.AddExam(new Exam(lan, Consts.LanguageLvl.A1, System.DateTime.Now, 204, 50));
            examDAO.AddExam(new Exam(lan2, Consts.LanguageLvl.B1, System.DateTime.Now, 88, 30));
            examDAO.AddExam(new Exam(lan, Consts.LanguageLvl.B2, System.DateTime.Now, 7, 10));

            examDAO.AddExam(new Exam(lan2, Consts.LanguageLvl.C1, System.DateTime.Now, 100, 150));
            */
            InitializeComponent();
            DataContext = new StudentViewModel(this);
        }



        private void OpenStudentProfile(object sender, RoutedEventArgs e)
        {
            StudentAccountWindow view = new StudentAccountWindow();
            view.Show();

        }

        private void DeleteProfile(object sender, RoutedEventArgs e)
        {
            StudentService ss = StudentService.GetInstance();
            ss.DeleteMyAccount();
            MessageBox.Show("Your profile has been successfully deleted");
            LoginWindow view = new LoginWindow();
            view.Show();
            this.Close();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
