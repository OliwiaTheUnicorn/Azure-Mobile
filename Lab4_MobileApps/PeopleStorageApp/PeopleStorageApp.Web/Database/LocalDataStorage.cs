using PeopleStorageApp.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleStorageApp.Web.Database
{
    public class LocalDataStorage
    {
        private List<Person> people = new List<Person>();
        public IReadOnlyCollection<Person> People => people;

        public void AddPerson(Person p)
        {
            p.Id = people.OrderByDescending(by => by.Id).FirstOrDefault()?.Id + 1 ?? 1;
            people.Add(p);
        }
    }
}
