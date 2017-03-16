using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

namespace Util
{
    /// <summary>
    /// json辅助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary> 
        /// 对象转换成json串 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="obj"></param> 
        /// <returns></returns> 
        public static string ObjectToJson(object obj)
        {
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            //MemoryStream stream = new MemoryStream();
            //serializer.WriteObject(stream, obj);
            //byte[] dataBytes = new byte[stream.Length];
            //stream.Position = 0;
            //stream.Read(dataBytes, 0, (int)stream.Length);
            //return Encoding.UTF8.GetString(dataBytes);
            string json = JsonConvert.SerializeObject(obj);
            return json;

        }
        /// <summary> 
        /// 从一个Json串生成对象信息 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="jsonString"></param> 
        /// <returns></returns> 
        public static T JsonToObject<T>(string jsonString)
        {
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            //MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            //return (T)serializer.ReadObject(mStream);

            var value = JsonConvert.DeserializeObject<T>(jsonString);
            return value;

        }
        /// <summary>
        /// 写入对象(转换成json)到文件中，若追加则使用行号符分隔
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <param name="isAppend"></param>
        /// <returns></returns>
        public static bool WriteFile(string path, object obj, bool isAppend = false)
        {
            if (obj == null)
                return false;
            var json = ObjectToJson(obj);
            return WriteFile(path, json, isAppend);
        }

        /// <summary>
        /// 写入json数据到文件中，若追加则使用行号符分隔
        /// </summary>
        /// <param name="path"></param>
        /// <param name="json"></param>
        /// <param name="isAppend">是否追加到文件末尾</param>
        /// <returns></returns>
        public static bool WriteFile(string path, string json, bool isAppend = false)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);

                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                if (isAppend)
                {
                    json += Environment.NewLine;
                }

                var stream = File.Open(path, isAppend ? FileMode.Append : FileMode.Truncate);
                var sw = new StreamWriter(stream);
                sw.Write(json);
                sw.Close();
                stream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
