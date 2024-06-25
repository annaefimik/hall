using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CinemaHall
{
    class Worker
    {
        private string surname;
        private string name;
        private string login;
        private string password;
        private string email;
        private string post;

        public string Surname { get { return surname; } set { surname = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Login { get { return login; } set { login = value; } }
        public string Password { get { return password; } set { password = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Post { get { return post; } set { post = value; } }

        public Worker (string surname, string name, string login, string password, string email, string post)
        {
            Surname = surname;
            Name = name;
            Login = login;
            Password = password;
            Email = email;
            Post = post;
           
        }

        
    }
}
