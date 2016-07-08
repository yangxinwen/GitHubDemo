using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Host
{
    public class PersonManageService : IPersonManageService
    {
        private static List<Person> personList = new List<Person>();
        public PersonManageService()
        {

        }

        public bool AddPerson(Person info)
        {            
            personList.Add(info);
            return true;
        }

        public bool DelPerson(int id)
        {
            var info = personList.FirstOrDefault(a => a.Id.Equals(id));
            if (info == null) return false;

            personList.Remove(info);
            return true;
        }

        public List<Person> GetAllPerson()
        {
            return personList;
        }

        public bool UpdatePerson(Person info)
        {
            var temp = personList.FirstOrDefault(a => a.Id.Equals(info.Id));
            if (info == null) return false;

            personList.Remove(temp);
            personList.Add(info);
            return true;

        }
    }
}