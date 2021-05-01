using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.DTOs.Models
{
    public class UserChatMessage
    {
        public String Username { get; set; }

        public String Message { get; set; }

        public DateTime TimeStamp { get; set; }

        public String TimeStampString => TimeStamp.ToString("dd-MM-yyyy, hh:mm:ss");
    }
}
