using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Host
{
    [ServiceContract]
    public interface IPersonManageService
    {
        [OperationContract]
        bool AddPerson(Person info);
        [OperationContract]
        bool DelPerson(int id);
        [OperationContract]
        bool UpdatePerson(Person info);
        [OperationContract]
        List<Person> GetAllPerson();
    }
}