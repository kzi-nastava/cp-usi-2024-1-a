namespace LangLang.Application.DTO
{
    public class DatabaseCredentials
    {
        public string Host { get; }
        public int Port { get; }
        public string Username { get; }
        public string Password { get; }
        public string Database { get; }

        public string ConnectionString { get; }

        public DatabaseCredentials(string host, int port, string username, string password, string database)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            Database = database;
            ConnectionString = "Host=" + Host + ";Port="+ Port.ToString() + ";Username=" + Username + ";Password=" + Password + ";Database=" + Database + ";";
        }

    }
}
