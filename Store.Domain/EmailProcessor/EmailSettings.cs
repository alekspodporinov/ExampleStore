using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.EmailProcessor
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "maliu092@gmail.com";
        public bool UseSsl = true;
        public string Username = "maliu092@gmail.com";
        public string Password = "Qwerty383076100";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"d:\game_store_emails";
    }

}
