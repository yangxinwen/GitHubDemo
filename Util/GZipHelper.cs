using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Util
{
    public class GZipHelper
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Compression(byte[] bytes)
        {
            byte[] buff = new byte[0];
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
                compressedzipStream.Write(bytes, 0, bytes.Length);
                compressedzipStream.Close();
                buff = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }
            catch (Exception)
            {
            }
            return buff;
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Decompression(byte[] bytes)
        {
            byte[] buff = new byte[0];
            try
            {
                MemoryStream ms = new MemoryStream(bytes);
                GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
                MemoryStream outBuffer = new MemoryStream();
                byte[] block = new byte[1024];
                while (true)
                {
                    int readCount = compressedzipStream.Read(block, 0, block.Length);
                    if (readCount <= 0)
                        break;
                    else
                        outBuffer.Write(block, 0, readCount);
                }
                buff = outBuffer.ToArray();
                compressedzipStream.Close();
                ms.Close();
                ms.Dispose();
                outBuffer.Close();
                outBuffer.Dispose();
            }
            catch (Exception)
            {

            }
            return buff;
        }
        /// <summary>
        /// 压缩一个支持序列化的对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] CompressSerializeObject(object obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化对象      
                MemoryStream ms = new MemoryStream();//创建内存流对象      
                formatter.Serialize(ms, obj);//把对象序列化到内存流  
                var bytes = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return Compression(bytes);
            }
            catch (Exception)
            {
                return null;// new byte[0]();
            }
        }
        /// <summary>
        /// 解压一个byte数组并反序列化为对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object DecompressSerializeObject(byte[] bytes)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化对象      
                MemoryStream ms = new MemoryStream(Decompression(bytes));//创建内存流对象 
                var obj = formatter.Deserialize(ms);
                bytes = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
