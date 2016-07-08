using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WCFClientTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WCFServiceProxy.PersonManageServiceClient();
            client.AddPerson(new WCFServiceProxy.Person() { Id = 1, Age = 25, Name = "test1", Sex = "Feman" });

            var list = client.GetAllPerson();
            foreach (var item in list)
            {
                Console.WriteLine(item.Name+" "+item.Sex);
            }


            Console.ReadKey();
        }
    }
}
