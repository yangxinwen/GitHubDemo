using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HttpsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new System.ServiceModel.ServiceHost(typeof(HttpsDemo.Services));
            host.Open();
            var ports = string.Empty;
            foreach (var item in host.BaseAddresses)
            {
                ports += item.Port+"  ";
            }

            Console.WriteLine($"服务已启动,监听端口:{ports} 回车结束");
            Console.ReadLine();
        }
    }
}
