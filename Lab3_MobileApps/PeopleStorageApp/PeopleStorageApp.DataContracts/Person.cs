using System;

namespace PeopleStorageApp.DataContracts
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureBase64 { get; set; }
    }
}
