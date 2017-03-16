using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace HttpsDemo
{
    [ServiceContract]
    public interface IServices
    {
        [OperationContract]
        string GetTime();
        [OperationContract]
        string GetHelloInfo(string name);
        [OperationContract]
        string DeleteInfo(List<string> ids);
    }

    public class Services : IServices
    {
        [WebInvoke(Method = "PUT", UriTemplate = "/Info/Delete/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string DeleteInfo(List<string> ids)
        {
            //["123","567"]
            var str = string.Empty;
            foreach (var item in ids)
            {
                str += item + " ";
            }
            return $"删除了id:{str}的信息";
        }

        [WebGet(UriTemplate = "/Info/Get/{name}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string GetHelloInfo(string name)
        {
            return $"hello {name}";
        }

        [WebGet(UriTemplate = "/GetDateTime/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string GetTime()
        {
            return DateTime.Now.ToString();
        }
    }
}
