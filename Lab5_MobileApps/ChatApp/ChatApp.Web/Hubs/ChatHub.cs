
using ChatApp.DTOs;
using ChatApp.DTOs.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Web.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(UserChatMessage message)
        {
            message.TimeStamp = System.DateTime.Now;
            await Clients.All.SendAsync(Consts.RECIEVE_MESSAGE, message);
        }

        public async Task RegisterUser(String userName)
        {
            await Clients.All.SendAsync(Consts.USER_JOINED, userName);
        }
    }
}
