using ChatApp.DTOs;
using ChatApp.DTOs.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        private readonly string userName;
        private readonly HubConnection hubConnection;

        private ObservableCollection<UserChatMessage> Messages { get; } = new ObservableCollection<UserChatMessage>();
        public ChatPage(string userName)
        {
            InitializeComponent();
            this.userName = userName;

            btnSend.Clicked += BtnSend_Clicked;

            lvMessages.ItemsSource = Messages;

            hubConnection = new HubConnectionBuilder().WithUrl("http://192.168.179.1:5000/chathub").Build();
            hubConnection.On<UserChatMessage>(Consts.RECIEVE_MESSAGE, ReceiveMessage_Event);
            hubConnection.On<String>(Consts.USER_JOINED, UserJoined_Event);
            hubConnection.StartAsync();

            hubConnection.SendAsync(Consts.REGISTER_USER, userName);
        }

        private void UserJoined_Event(string userName)
        {
            Messages.Insert(0, new UserChatMessage
            {
                Message = "User joined the conversation",
                Username = userName,
                TimeStamp = DateTime.Now
            });
        }

        private void ReceiveMessage_Event(UserChatMessage obj)
        {
            Messages.Insert(0, obj);
        }

        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
            var message = txtMessage.Text;
            if(string.IsNullOrEmpty(message))
            {
                await DisplayAlert("Validation Failed", "The message is required", "OK");
                return;
            }

            await hubConnection.SendAsync(Consts.SEND_Message, new UserChatMessage
            {
                Message = message,
                Username = userName
            });
        }
    }
}