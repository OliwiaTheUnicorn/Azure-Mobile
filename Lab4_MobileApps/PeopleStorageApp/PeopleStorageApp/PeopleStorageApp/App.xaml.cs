using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeopleStorageApp.DataContracts;

namespace PeopleStorageApp
{
    public partial class App : Application
    {
        private const String API_URL = "http://192.168.179.1:5000/api";

        public App()
        {
            var client = RestEase.RestClient.For<IPeopleClient>(API_URL);
            InitializeComponent();

            MainPage = new MainPage(client);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
