using PeopleStorageApp.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PeopleStorageApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IPeopleClient peopleClient;
        private Person person = new Person();
        public MainPage(IPeopleClient client)
        {
            InitializeComponent();
            peopleClient = client;
            btnCamera.Clicked += btnCamera_Clicked;
            btnSave.Clicked += BtnSave_Clicked;

            entFirstName.TextChanged += EntFirstName_TextChanged;
            entLastName.TextChanged += EntLastName_TextChanged;
            entPhoneNumber.TextChanged += EntPhoneNumber_TextChanged;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (!Validate())
            {
                await DisplayAlert("Validation Error", "First name, last name, phone number and picture are required.", "Ok");
                return;
            }

            try
            {
                await peopleClient.AddPersonAsync(person);
                await DisplayAlert("Success", "Data has been saved.", "Ok");
                clear();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private void clear()
        {
            entFirstName.Text = string.Empty;
            entLastName.Text = string.Empty;
            entPhoneNumber.Text = string.Empty;
            imgPhoto.Source = null;
            person = new Person();
        }

        private void EntPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            person.PhoneNumber = e.NewTextValue;
        }

        private void EntFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            person.FirstName = e.NewTextValue;
        }

        private void EntLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            person.LastName = e.NewTextValue;
        }

        private async void btnCamera_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {

            });

            if (photo == null)
            {
                return;
            } else
            {
                imgPhoto.Source = ImageSource.FromStream(() => photo.GetStream());
                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    photo.GetStream().CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                string base64 = Convert.ToBase64String(bytes);
                person.PictureBase64 = base64;
            }
        }

        private bool Validate()
        {
            return !(string.IsNullOrWhiteSpace(person.FirstName) ||
                    string.IsNullOrWhiteSpace(person.LastName) ||
                    string.IsNullOrWhiteSpace(person.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(person.PictureBase64)
                );
        }
    }
}
