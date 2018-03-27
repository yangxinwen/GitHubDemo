using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NPoco;
using NPocoDemo.Model;

namespace NPocoDemo
{
    class Program
    {
        private static string _connStr = "Data Source =.; Initial Catalog = EFDemo; Integrated Security = False; User ID = sa; Password = 123456;";

        static void Main(string[] args)
        {
            Add();
            Search();
            Update();
            Delete();
            ExecTran(false);
            ExecTran(true);
            Console.ReadLine();
        }
        private static void Add()
        {
            var dbContext = NPocoHelper.GetNewInstance(_connStr);
            var result = dbContext.Insert<User1>(new User1() { LoginCode = "test01", Name = "test01", Password = "test01" });
            Console.WriteLine("add result:" + result);
        }

        private static void Search()
        {
            var dbContext = NPocoHelper.GetNewInstance(_connStr);
            var result = dbContext.FirstOrDefault<User1>("select * from User1 where LoginCode='test01'");
            Console.WriteLine("Name:" + result.Name);
        }

        private static void Update()
        {
            var dbContext = NPocoHelper.GetNewInstance(_connStr);
            var result = dbContext.First<User1>("where LoginCode='test01'");
            result.Password = "newTest01";
            dbContext.Update<User1>(result, (a) => a.Password);
            result = dbContext.First<User1>("where LoginCode='test01'");
            Console.WriteLine("password:" + result.Password);
        }


        private static void Delete()
        {
            var dbContext = NPocoHelper.GetNewInstance(_connStr);
            var result = dbContext.FirstOrDefault<User1>("select * from User1 where LoginCode='test01'");
            var count = dbContext.Delete(result);
            Console.WriteLine("Count:" + count);
        }






        private static void ExecTran(bool isSuccess)
        {
            var dbContext = NPocoHelper.GetNewInstance(_connStr);
            var newUser = new User1() { LoginCode = "test02", Name = "test02", Password = "test02" };

            //var tran=dbContext.GetTransaction();
            dbContext.BeginTransaction();


            var result = dbContext.Insert<User1>(newUser);
            newUser.LoginCode = "test0201";
            result = dbContext.Insert<User1>(newUser);

            if (isSuccess)
                dbContext.CompleteTransaction();
            else
                dbContext.AbortTransaction();


            var user = dbContext.FirstOrDefault<User1>("select * from User1 where LoginCode=@LoginCode", newUser);

            if (user == null)
                Console.WriteLine("User 未添加成功");
            else
            {
                Console.WriteLine("User 添加成功");
                //清理数据
                dbContext.Delete(user);
                dbContext.Delete<User1>("where LoginCode='test02'");
            }




        }












    }
}