using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PeopleStorageApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public string UserDetails => $"First Name: {FirstName} \n Last Name: {LastName} \n Phone Number: {PhoneNumber}";
        string firstName = string.Empty;
        string lastName = string.Empty;
        string phoneNumber = string.Empty;
        public string FirstName
        {
            get => firstName;
            set
            {
                if (firstName == value)
                {
                    return;
                } else
                {
                    firstName = value;
                    onPropertyChanged(nameof(FirstName));
                    onPropertyChanged(nameof(UserDetails));
                }
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (lastName == value)
                {
                    return;
                }
                else
                {
                    lastName = value;
                    onPropertyChanged(nameof(LastName));
                    onPropertyChanged(nameof(UserDetails));
                }
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (phoneNumber == value)
                {
                    return;
                }
                else
                {
                    phoneNumber = value;
                    onPropertyChanged(nameof(PhoneNumber));
                    onPropertyChanged(nameof(UserDetails));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
