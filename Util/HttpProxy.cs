using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Util
{
    /// <summary>
    /// http代理类
    /// </summary>
    public class HttpProxy
    {
        /// <summary>
        /// 返回连接http代理的客户端连接
        /// </summary>
        /// <param name="proxyAdress">代理地址</param>
        /// <param name="proxyPort">代理端口</param>
        /// <param name="destAddress">连接目标地址</param>
        /// <param name="destPort">连接目标端口</param>
        /// <param name="userName">验证用户名，若不需要则可设置为空</param>
        /// <param name="password">验证用户密码</param>
        /// <returns></returns>
        public static TcpClient ConnectToHttpProxy(string proxyAdress, ushort proxyPort, string destAddress, ushort destPort,
            string userName, string password)
        {
            //测试软件(CCProxy、SuperProxy)

            TcpClient client = new TcpClient();
            client.Connect(proxyAdress, proxyPort);
            var stream = client.GetStream();
            bool needLogin = !string.IsNullOrWhiteSpace(userName);
            string str = string.Empty;
            str = string.Format("CONNECT {0}:{1} HTTP/1.1\r\n", destAddress, destPort);
            str += string.Format("HOST: {0}:{1}\r\n", destAddress, destPort);
            str += "Proxy-Connection: Keep-Alive\r\n";

            if (needLogin)
            {
                //代理需要用户名和密码认证
                string strUserPwd = userName + ":" + password;
                str += "Proxy-Authorization: Basic " + Base64.Base64encode(strUserPwd) + "\r\n";
            }

            str += "User-Agent: Apis\r\n\r\n";  //必须得两个换号，否则代理服务不会答应
            var request = Encoding.Default.GetBytes(str);
            stream.Write(request, 0, request.Length);
            byte[] buff = new byte[255];
            var count = stream.Read(buff, 0, buff.Length);
            string result = Encoding.Default.GetString(buff, 0, count);

            //HTTP/1.0 200 Connection established 或者 HTTP/1.1 200 Connection established  连接成功返回字符串
            if (result.IndexOf(" 200 Connection") > -1)
                return client;
            else if (result.IndexOf(" 407 Unauthorized") > -1)
                throw new Exception("Proxy Unauthorized Fail");
            else throw new Exception("Proxy Connect Fail:" + result);

        }
    }
}
