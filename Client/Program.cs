using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //通过以下命令生成代理  svcutil  http://localhost:8080 / o:d:/ PersonManageService.cs


            var client = new PersonManageServiceClient("PersonManageServiceEndPoint");

            client.AddPerson(new Person() { Id = 1, Age = 25, Name = "test1", Sex = "Feman" });

            var list = client.GetAllPerson();
            foreach (var item in list)
            {
                Console.WriteLine(item.Name + " " + item.Sex);
            }


            Console.ReadKey();
        }
    }
}
