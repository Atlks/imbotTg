global using static prjx.libx.bscEncdCls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
// mdsj.lib.encdCls

namespace prjx.libx
{
    internal class bscEncdCls
    {   /// <summary>
        /// 
        /// </summary>
        /// <param name="objSave"></param>
        /// <returns>jobjct</returns>
        public static JObject DecodeJson(object objSave)
        {
            return (JObject)json_decodeObj(ToStr(objSave));
        }
        public static string EncryptAes(string plainText)
        {
            return ByteArrayToHex(EncryptAesRtBytearr(plainText));
        }
        public static string DecryptAes(string plainText)
        {
            return DecryptAes(ToByteArrayFrmHexstr(plainText));
        }

        public static byte[] ToByteArrayFrmHexstr(string hexString)
        {
            // 移除任何可能存在的空格
            hexString = hexString.Replace(" ", string.Empty);

            // 确保字符串长度是偶数
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid hex string length.");
            }

            // 创建字节数组
            byte[] byteArray = new byte[hexString.Length / 2];

            for (int i = 0; i < byteArray.Length; i++)
            {
                // 取出两个字符并转换为一个字节
                string hexPair = hexString.Substring(i * 2, 2);
                byteArray[i] = Convert.ToByte(hexPair, 16);
            }

            return byteArray;
        }
        public static string ByteArrayToHex(byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
        /// <summary>
        /// 解释
        //    Key 和 IV:

        //Key 是用于加密和解密的密钥，必须为 32 字节（256 位）长度的字符串。
        //IV 是初始化向量，必须为 16 字节（128 位）长度的字符串。
        //在实际使用中，密钥和 IV 应使用更安全的方式生成和存储。
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static byte[] EncryptAesRtBytearr(string plainText)
        {
            char[] IV = "1234567890123456".ToCharArray();
            char[] Key = "12345678901234561234567890123456".ToCharArray();
            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(IV);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }
        //public static string DecryptAes(string cipherText)
        //{

        //}
        public static string DecryptAes(byte[] cipherText)
        {
            char[] IV = "1234567890123456".ToCharArray();
            char[] Key = "12345678901234561234567890123456".ToCharArray();
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(IV);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        static string ComputeMd5Hash(string input)
        {
            // 使用 MD5 类来计算哈希值
            using (MD5 md5 = MD5.Create())
            {
                // 将输入字符串转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // 计算输入字节数组的哈希值
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 将字节数组转换为十六进制字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        // Base64解码
        public static string DecodeBase64(string base64)
        {
            var bytes = System.Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/'));
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        public static string escapeshellarg(string arg)
        {
            // Check if the argument contains spaces or quotes
            if (arg.Contains(" ") || arg.Contains("\"") || arg.Contains("\'"))
            {
                // Escape quotes by doubling them
                arg = arg.Replace("\"", "\\\"").Replace("'", "\\'");
                // Enclose the argument in double quotes
                arg = "\"" + arg + "\"";
            }
            return arg;
        }
        /// <summary> 们可以编写类似于 PHP 中的 unserialize 和 serialize 函数。
        /// 这两个函数通常用于将对象序列化为字符串（serialize）和将字符串反序列化为对象（unserialize）。
        /// 在 C# 中，可以使用 JSON 序列化和反序列化来实现类似的功能。下面是示例代码
        /// 将对象序列化为 JSON 格式的字符串。
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }

    
        /// <summary>
        /// Removes HTML tags from a string.
        /// </summary>
        /// <param name="html">The input string containing HTML.</param>
        /// <returns>A string with HTML tags removed.</returns>
        public static string strip_tags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }


            // Remove <script> tags and their content
            string noScript = Regex.Replace(html, "<script.*?>.*?</script>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Regular expression to match HTML tags
            string pattern = "<.*?>";

            // Replace HTML tags with an empty string
            //    string result = Regex.Replace(noScript, pattern, string.Empty);
            // Replace HTML tags with an empty string
            string result = Regex.Replace(noScript, pattern, string.Empty, RegexOptions.Singleline);

            result = RemoveExtraNewlines(result);
            result = RemoveInvisibleCharacters(result);
            result = delEmpltyLines(result);

            result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            //result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            return result;
        }

       
     
      
        /// <summary>
        /// 将 JSON 格式的字符串反序列化为对象。
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">要反序列化的 JSON 字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T unserialize<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }
        /// <summary>
        /// 将 JSON 字符串解析为动态对象，类似于 PHP 的 json_decode 函数。
        /// </summary>
        /// <param name="jsonString">要解析的 JSON 字符串</param>
        /// <returns>解析后的动态对象</returns>
        public static List<SortedList> json_decode(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<SortedList>>(jsonString);
        }
        public static string DecodeUrl(string path)
        {
            string decodedUrl = WebUtility.UrlDecode(path);
            return decodedUrl;
        }

        // 将 JObject 转换为 Hashtable
        public static string Encodeurl(string originalString)
        {
            return HttpUtility.UrlEncode(originalString);
            //    encodeJsonNofmt
        }
        public static string EncodeJsonFmt(object results)
        {
            if (results == null)
            {
                Print(" ***fun encodeJson() ,prm rslt is null..");
                return "{}"; // 如果对象为空，返回空对象字符串
            }
            try
            {
                results = CastToSerializableObjsOrSnglobj(results);
                //   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                       Formatting = Formatting.Indented
                };
                string jsonString = JsonConvert.SerializeObject(results, settings);
                return jsonString;
            }
            catch (Exception e)
            {
                //if (results is object[] argsArray)
                //    return "[]"; 
                // 检查对象是否为数组
                if (IsArray(results))
                {
                    return "[]";
                }
                // 检查对象是否为集合
                if (IsCollection(results))
                {
                    return "[]";
                }
                return "{}";
            }
        }

        public static string EncodeJson(object results)
        {
            if (results == null)
            {
                Print(" ***fun encodeJson() ,prm rslt is null..");
                return "{}"; // 如果对象为空，返回空对象字符串
            }
            try
            {
                results = CastToSerializableObjsOrSnglobj(results);
                //   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    //   Formatting = Formatting.Indented
                };
                string jsonString = JsonConvert.SerializeObject(results, settings);
                return jsonString;
            }
            catch (Exception e)
            {
                //if (results is object[] argsArray)
                //    return "[]"; 
                // 检查对象是否为数组
                if (IsArray(results))
                {
                    return "[]";
                }
                // 检查对象是否为集合
                if (IsCollection(results))
                {
                    return "[]";
                }
                return "{}";
            }
        }
        public static JsonObject json_decodeJonObj(string jsonString)
        {
            return JsonConvert.DeserializeObject<JsonObject>(jsonString);
        }

        public static object json_decodeObj(string jsonString)
        {
            return JsonConvert.DeserializeObject<object>(jsonString);
        }

        public static t json_decode<t>(string jsonString)
        {
            return JsonConvert.DeserializeObject<t>(jsonString);
        }
        public static string json_encode(object results)
        {
            //   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            //  string json = JsonConvert.SerializeObject(obj, settings);
            string jsonString = JsonConvert.SerializeObject(results, settings);
            //print(jsonString);
            return jsonString;
        }
        public static string encodeJsonNofmt(object results)
        {
            try
            {
                return json_encode_noFmt(results);
            }
            catch (Exception e)
            {
                return "[]";
            }

        }

        public static string json_encode_noFmt(object results)
        {
            //   encodeJsonNofmt
            //     encodeJson
            results = CastToSerializableObjsOrSnglobj(results);


            //   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    //   Formatting = Formatting.Indented
                };
                //  string json = JsonConvert.SerializeObject(obj, settings);
                string jsonString = JsonConvert.SerializeObject(results, settings);
                //print(jsonString);
                return jsonString;
            }
            catch (Exception e)
            {
                return "{}";
            }

        }

    }
}
