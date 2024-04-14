using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.UserServices
{
    public class DirectorService : IDirectorService
    {
        DirectorDAO directorDAO = DirectorDAO.GetInstance();
        public Director LoggedUser { get; set; }


        //Singleton
        private static DirectorService instance;
        private DirectorService()
        {
        }

        public static DirectorService GetInstance()
        {
            if (instance == null)
                instance = new DirectorService();
            return instance;
        }
    }
}