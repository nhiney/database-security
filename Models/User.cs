using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_N6.Models 
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public User() { }

        public User(string userName, string password, string email, string address)
        {
            UserName = userName;
            Password = password;
            Email = email;
            Address = address;
        }
    }
}