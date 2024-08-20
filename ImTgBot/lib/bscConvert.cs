global using static prjx.libx.bscConvert;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;
using System.Xml;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using System.Net.Sockets;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Web;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
//using static SqlParser.Ast.Expression;
using Microsoft.AspNetCore.WebUtilities;
using System.Dynamic;

namespace prjx.libx
{
    public class bscConvert
    {
        public static List<SortedList<string, string>> ToListFrmIotDic(SortedList liIot)
        {
            List<SortedList<string, string>> lifnl = new List<SortedList<string, string>>();
            foreach (DictionaryEntry kv in liIot)
            {
                //   SortedList<string, string> rw = kv;
                lifnl.Add((SortedList<string, string>)kv.Value);
            }

            return lifnl;
        }
        public static string ToIniTxt(SortedList<string, string> data)
        {
            // 创建ini文件内容
            StringBuilder iniContent = new StringBuilder();
            foreach (var pair in data)
            {

                iniContent.AppendLine($"{pair.Key}={pair.Value}");

            }
            string txt = iniContent.ToString();
            return txt;
        }

        public static SortedList ToIotDic(List<SortedList<string, string>> li)
        {
            SortedList liIot = new SortedList();
            foreach (SortedList<string, string> row in li)
            {
                liIot.Add(row["id"], row);
            }

            return liIot;
        }

        public static SortedList<string, string> ConvertToStringSortedList(SortedList originalList)
        {
            // 创建新的泛型 SortedList<string, string>
            SortedList<string, string> result = new SortedList<string, string>();

            // 遍历原始非泛型 SortedList
            foreach (DictionaryEntry entry in originalList)
            {
                // 将键和值转换为字符串并添加到新列表中
                string key = entry.Key.ToString();
                string value = entry.Value.ToString();
                result.Add(key, value);
            }

            return result;
        }

        public static List<Hashtable> ConvertToHashtableList(List<dynamic> dynamicList)
        {
            var hashtableList = new List<Hashtable>();

            foreach (var item in dynamicList)
            {
                var hashtable = new Hashtable();
                var expando = item as ExpandoObject;

                if (expando != null)
                {
                    foreach (var kvp in expando)
                    {
                        hashtable[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    var type = item.GetType();
                    foreach (var property in type.GetProperties())
                    {
                        hashtable[property.Name] = property.GetValue(item);
                    }
                }

                hashtableList.Add(hashtable);
            }

            return hashtableList;
        }

        public static string GetRealPath(string folder)
        {
            try
            {
                // 获取文件夹的绝对路径
                string realPath = Path.GetFullPath(folder);
                return realPath;
            }
            catch (Exception ex)
            {
                // 输出错误信息
                Console.WriteLine($"Error getting real path: {ex.Message}");
                return folder;
            }
        }
        public static Dictionary<string, StringValues> ToDic941(string qerystr)
        {
            if (!qerystr.StartsWith("http"))
            {
                //    var uri = new Uri("https://t.me/" + qerystr);  uri.Query
                var parameters = QueryHelpers.ParseQuery(qerystr);
                return parameters;
            }
            return QueryHelpers.ParseQuery(qerystr); ;
        }

        public static SortedList ToSortedListFrmQrystr(string queryString)
        {
            // 使用 HttpUtility.ParseQueryString 解析查询字符串
            NameValueCollection queryParameters = HttpUtility.ParseQueryString(queryString);

            // 创建一个新的 SortedList
            SortedList sortedList = new SortedList();

            // 将解析后的查询字符串参数添加到 SortedList 中
            foreach (string key in queryParameters)
            {
                string k = key.Trim();
                //     key = key.Trim();
                sortedList.Add(k, queryParameters[key]);
            }

            return sortedList;
        }
        public static SortedList CopyHashtableToSortedList(Hashtable hashtable, SortedList sortedList)
        {
            // 创建一个新的 SortedList
            // SortedList sortedList = new SortedList();

            // 遍历 Hashtable 并将每个键值对添加到 SortedList
            foreach (DictionaryEntry entry in hashtable)
            {
                try
                {
                    SetField(sortedList, entry.Key.ToString(), entry.Value);
                    // sortedList.Add(entry.Key, entry.Value);
                }
                catch (Exception e)
                {
                    PrintExcept("CopyHashtableToSortedList", e);
                }

            }

            return sortedList;
        }


        public static string ToStrComma(HashSet<string> hashSet)
        {
            // 使用 string.Join 方法将 HashSet 中的元素连接成一个逗号分隔的字符串
            return string.Join(",", hashSet);
        }

      

        public static HashSet<string> ConvertCommaSeparatedStringToHashSet(string input)
        {
            // 使用 string.Split 方法将逗号分隔的字符串拆分成数组
            string[] items = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // 创建 HashSet 并将数组元素添加到其中
            return new HashSet<string>(items);
        }


        public static string ConvertToSimplifiedChinese(string traditionalChinese)
        {
            //  ChineseConverter
            Dictionary<char, char> traditionalToSimplifiedMap = new Dictionary<char, char>
        {
            {'繁', '繁'},
            {'體', '体'},
            {'中', '中'},
            {'文', '文'}
            // 这里需要添加所有的繁体到简体的映射
        };

            char[] simplifiedChars = new char[traditionalChinese.Length];
            for (int i = 0; i < traditionalChinese.Length; i++)
            {
                char ch = traditionalChinese[i];
                simplifiedChars[i] = traditionalToSimplifiedMap.ContainsKey(ch) ? traditionalToSimplifiedMap[ch] : ch;
            }

            return new string(simplifiedChars);
        }

        public static SortedList CastJObjectToSortedList(JObject jObject)
        {
            var sortedList = new SortedList();

            foreach (var property in jObject.Properties())
            {
                // 递归处理嵌套的 JObject
                if (property.Value.Type == JTokenType.Object)
                {
                    sortedList.Add(property.Name, ConvertJObjectToSortedList((JObject)property.Value));
                }
                else
                {
                    sortedList.Add(property.Name, property.Value.ToObject<object>());
                }
            }

            return sortedList;
        }


        public static SortedList castKeyToEnName(SortedList sortedList, SortedList<string, string> transmap)
        {
            //todo chg to prm.each   binxin api
            SortedList map3 = new SortedList();
            // 循环遍历每一个键
            foreach (object key in sortedList.Keys)
            {
                //if (key.ToString() == "Searchs")
                //    Print("dbg433");
                //add all cn key
                var Cnkey = key;
                var val = sortedList[key];
                SetField938(map3, Cnkey.ToString(), val);

                //add all eng key
                var keyEng = LoadFieldDefEmpty(transmap, Cnkey);
                if (keyEng == "")
                    keyEng = Cnkey.ToString();
                SetField938(map3, keyEng, val);
                //chg int fmt
                if (IsNumeric((val)))
                {
                    double objSave = ConvertStringToNumber(val);
                    SetField938(map3, keyEng, objSave);
                }

                //   Console.WriteLine($"Key: {key}, Value: {sortedList[key]}");

            }

            return map3;
        }



        public static Dictionary<string, string> ToDictionary(Hashtable hashtable)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (DictionaryEntry entry in hashtable)
            {
                if (entry.Key is string key && entry.Value is string value)
                {
                    dictionary.Add(key, value);
                }
                else
                {
                    // 处理非字符串键值对的情况，例如抛出异常或进行类型转换
                    throw new InvalidCastException("Hashtable中的键或值不是字符串类型");
                }
            }
            return dictionary;
        }


        /// <summary>
        /// rpls eng byaodian fuhaor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string CastToEnglishCharPunctuation(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // 中文标点符号与英文标点符号的对应关系
            var punctuationMap = new Dictionary<string, string>
        {
            { "，", "," },
            { "。", "." },
            { "！", "!" },
            { "？", "?" },
            { "；", ";" },
            { "：", ":" },
            { "（", "(" },
            { "）", ")" },
            { "【", "[" },
            { "】", "]" },
            { "‘", "'" },
            { "’", "'" },
            { "“", "\"" },
            { "”", "\"" },
            { "—", "-" }
            // 可以继续添加更多的标点符号替换
        };

            // 替换中文标点符号为英文标点符号
            foreach (var entry in punctuationMap)
            {
                input = input.Replace(entry.Key, entry.Value);
            }

            return input;
        }
        public static string CastToPathReal(string path)
        {
            path = path.Replace("//", "/"); path = path.Replace("//", "/");

            path = DecodeUrl(path);
            return path;
        }

        //  WriteAllText
        //我们首先使用 System.IO.Path.GetInvalidFileNameChars 方法获取操作系统支持的非法文件名字符数组
        /*
         
         我们遍历输入的文本，并检查每个字符是否是非法字符。如果字符是非法字符，则使用 HttpUtility.UrlEncode 方法对字符进行 URL 编码，然后将编码后的结果添加到结果字符串中。最后，返回处理后的结果字符串。
         
         */
        public static string ConvertToValidFileName2024(string input)
        {
            // URL 编码非法字符
            string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
            StringBuilder encodedBuilder = new StringBuilder();
            foreach (char c in input)
            {
                if (invalidChars.Contains(c))
                {
                    // 如果字符为非法字符，则使用 URL 编码替换
                    string encoded = HttpUtility.UrlEncode(c.ToString());
                    encodedBuilder.Append(encoded);
                }
                else
                {
                    // 如果字符为合法字符，则直接添加到结果中
                    encodedBuilder.Append(c);
                }
            }
            return encodedBuilder.ToString();
        }

        public static InputFileStream ToFilStrm(string videoFilePath)
        {
            var mp3Stream = System.IO.File.Open(videoFilePath, FileMode.Open);
            var inputOnlineFile = InputFile.FromStream(mp3Stream);
            return inputOnlineFile;
        }

        public static string ToSqlPrmMode(string rzt)
        {
            return rzt.Replace(" ", ",");
        }
        public static string CastToParksByCtry(string ctry)
        {
            try
            {
                //---ctry code mode
                if (IsExistFil($"{prjdir}/cfg/{ctry}_pks.txt"))
                {
                    return ReadAllText($"{prjdir}/cfg/{ctry}_pks.txt").Trim();
                }
                //ctry cn name mode
                //  string f119 = $"{prjdir}/webroot/国家.json";
                string f119 = $"{prjdir}/cfg/ctrycode.ini";
                Hashtable ht = GetHashtabFromIniFl(f119);
                string ctrycode = ht[ctry].ToString();
                string f119314 = $"{prjdir}/cfg/mmr_pks.txt";
                return ReadAllText(f119314).Trim();

            }
            catch (Exception e)
            {
                PrintExcept("CastToParksByCtry", e);
                return "";
            }

        }

        public static string ToStrFromHashset(HashSet<string> hashSet)
        {
            return string.Join(" ", hashSet);
        }

        public static string CastToParksByCity(string city)
        {
            //---ctry code mode
            if (IsExistFil($"{prjdir}/cfg/{city}_pks.txt"))
            {
                return ReadAllText($"{prjdir}/cfg/{city}_pks.txt").Trim();
            }
            string f119 = $"{prjdir}/webroot/国家.json";
            return GetParkNamesFromJson(ReadAllText(f119), city);
        }
        public static string CastHashtableToQuerystringNoEncodeurl(Dictionary<string, StringValues> sortedList)
        {
            if (sortedList == null)
            {
                return "";
            }

            var queryString = "";
            foreach (var entry in sortedList)
            {
                if (queryString.Length > 0)
                {
                    queryString += "&";
                }

                // URL encode the key and value to handle special characters
                //string key = Uri.EscapeDataString(entry.Key.ToString());
                //string value = Uri.EscapeDataString(entry.Value.ToString());

                queryString += $"{entry.Key.ToString()}={entry.Value.ToString()}";
            }

            return queryString;
        }

        /// <summary>
        /// no encode url part
        /// </summary>
        /// <param name="sortedList"></param>
        /// <returns></returns>
        public static string ToQrystr(SortedList sortedList)
        {
            if (sortedList == null)
            {
                return "";
            }

            var queryString = "";
            foreach (DictionaryEntry entry in sortedList)
            {
                if (queryString.Length > 0)
                {
                    queryString += "&";
                }

                // URL encode the key and value to handle special characters
                //string key = Uri.EscapeDataString(entry.Key.ToString());
                //string value = Uri.EscapeDataString(entry.Value.ToString());

                queryString += $"{entry.Key.ToString()}={entry.Value.ToString()}";
            }

            return queryString;
        }

        public static string CastHashtableToQuerystringNoEncodeurl(SortedList sortedList)
        {
            if (sortedList == null)
            {
                return "";
            }

            var queryString = "";
            foreach (DictionaryEntry entry in sortedList)
            {
                if (queryString.Length > 0)
                {
                    queryString += "&";
                }

                // URL encode the key and value to handle special characters
                //string key = Uri.EscapeDataString(entry.Key.ToString());
                //string value = Uri.EscapeDataString(entry.Value.ToString());

                queryString += $"{entry.Key.ToString()}={entry.Value.ToString()}";
            }

            return queryString;
        }

        public static string CastHashtableToQuerystring(SortedList sortedList)
        {
            if (sortedList == null)
            {
                throw new ArgumentNullException(nameof(sortedList));
            }

            var queryString = "";
            foreach (DictionaryEntry entry in sortedList)
            {
                if (queryString.Length > 0)
                {
                    queryString += "&";
                }

                // URL encode the key and value to handle special characters
                string key = Uri.EscapeDataString(entry.Key.ToString());
                string value = Uri.EscapeDataString(entry.Value.ToString());

                queryString += $"{key}={value}";
            }

            return queryString;
        }


        public static void CastVal2hashtable(SortedList list)
        {
            // 创建一个临时的 ArrayList 来存储键
            ArrayList keys = new ArrayList(list.Keys);

            // 遍历每个键并更新值
            foreach (string key in keys)
            {
                string value = (string)list[key];
                list.Remove(key);
                list.Add(key, castUrlQueryString2hashtable(value));
                //    list[key] = castUrlQueryString2hashtable(value); ;
            }
        }

        public static void ConvertKeysToLowercase(Hashtable hashtable)
        {
            Hashtable newHashtable = new Hashtable();

            foreach (DictionaryEntry entry in hashtable)
            {
                string lowercaseKey = entry.Key.ToString().ToLower(); // 将键转换为小写
                newHashtable[lowercaseKey] = entry.Value; // 保持值不变，添加到新的 Hashtable 中
            }

            // 清空原始 Hashtable 并将新的键值对复制回去
            hashtable.Clear();
            foreach (DictionaryEntry entry in newHashtable)
            {
                hashtable[entry.Key] = entry.Value;
            }
        }
        //我们首先使用 System.IO.Path.GetInvalidFileNameChars 方法获取操作系统支持的非法文件名字符数组
        /*
         
         我们遍历输入的文本，并检查每个字符是否是非法字符。如果字符是非法字符，则使用 HttpUtility.UrlEncode 方法对字符进行 URL 编码，然后将编码后的结果添加到结果字符串中。最后，返回处理后的结果字符串。
         
         */
        public static string ConvertToValidFileName(string input)
        {
            // URL 编码非法字符
            string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
            invalidChars = invalidChars + "/"+"\\"+"|"+ "\"";//syege he shwao inhao
                invalidChars = invalidChars + "*:?<>&";
            StringBuilder encodedBuilder = new StringBuilder();
            foreach (char c in input)
            {
                if (invalidChars.Contains(c))
                {
                    // 如果字符为非法字符，则使用 URL 编码替换
                    string encoded = HttpUtility.UrlEncode(c.ToString());
                    encodedBuilder.Append(encoded);
                }
                else
                {
                    // 如果字符为合法字符，则直接添加到结果中
                    encodedBuilder.Append(c);
                }
            }
            return encodedBuilder.ToString();
        }

        public static SortedList ToSortedListFrmJson(string json)
        {
            // 解析 JSON 字符串为 JObject
            JObject jObject = JObject.Parse(json);

            // 创建一个新的 SortedList
            SortedList sortedList = new SortedList();

            // 将 JObject 中的所有键值对添加到 SortedList
            foreach (JProperty property in jObject.Properties())
            {
                // 将 JToken 转换为 .NET 类型
                object value = property.Value;//.ToObject<object>();
                sortedList.Add(property.Name, value);
            }

            return sortedList;
        }
        public static SortedList<string, object> ConvertJObjectToSortedList(JObject jObject)
        {
            var sortedList = new SortedList<string, object>();

            foreach (var property in jObject.Properties())
            {
                // 递归处理嵌套的 JObject
                if (property.Value.Type == JTokenType.Object)
                {
                    sortedList.Add(property.Name, ConvertJObjectToSortedList((JObject)property.Value));
                }
                else
                {
                    sortedList.Add(property.Name, property.Value.ToObject<object>());
                }
            }

            return sortedList;
        }


        /// <summary>
        /// Converts a URL query string into a Dictionary<string, string>.
        /// </summary>
        /// <param name="queryString">The query string to convert.</param>
        /// <returns>A dictionary containing the key-value pairs from the query string.</returns>
        public static Dictionary<string, string> ToDictionaryFrmQrystr(string queryString)
        {
            var result = new Dictionary<string, string>();

            // Ensure the query string is in a valid format
            if (string.IsNullOrEmpty(queryString))
            {
                return result;
            }

            // Use HttpUtility to parse the query string
            var nameValueCollection = HttpUtility.ParseQueryString(queryString);

            foreach (string key in nameValueCollection.AllKeys)
            {
                if (key != null)
                {
                    result[key] = nameValueCollection[key];
                }
            }

            return result;
        }

        public static int ToInt146(object obj)
        {
            return System.Convert.ToInt32(obj);
        }
        public static Dictionary<string, string> ConvertToStringDictionary(Dictionary<string, StringValues> input)
        {
            var result = new Dictionary<string, string>();

            foreach (var kvp in input)
            {
                result[kvp.Key] = kvp.Value.ToString();
            }

            return result;
        }
        public static SortedList ConvertFormToSortedList(IFormCollection formCollection)
        {
            SortedList sortedList = new SortedList();

            foreach (string key in formCollection.Keys)
            {
                if (key != null)
                {
                    sortedList.Add(key, formCollection[key][0]);
                }
            }

            return sortedList;
        }

        public static List<object> CastToSerializableObjs(List<object> list)
        {
            List<object> listRzt = new List<object>();

            foreach (var obj in list)
            {

                listRzt.Add(CastToSerializableObj(obj));

            }
            return listRzt;
        }

        public static List<object> ConvertArrayToList(object obj)
        {
            // 创建一个新的 List 用于存储元素
            List<object> list = new List<object>();

            // 检查对象是否为数组
            if (obj is System.Array array)
            {
                // 循环遍历数组的每个元素并添加到 List 中
                foreach (var element in array)
                {
                    list.Add(element);
                }
            }

            return list;
        }
        public static List<object> ConvertCollectionToList(object obj)
        {
            // 创建一个新的 List 用于存储元素
            List<object> list = new List<object>();

            // 检查对象是否为集合 (实现了 IEnumerable 接口)
            if (obj is IEnumerable collection)
            {
                // 循环遍历集合的每个元素并添加到 List 中
                foreach (var element in collection)
                {
                    list.Add(element);
                }
            }

            return list;
        }

        public static object CastToSerializableObj(object obj)
        {
            if (obj is HttpRequest)
            {
                //     var req= 
                var req = (HttpRequest)obj;
                //Hashtable hs = new Hashtable();
                //hs.Add("name",nameof(HttpRequest));
                //hs.Add("url",req.Path+req.QueryString);
                //   maybe qrystr

                return $"@HttpRequest:  {req.Scheme}://{req.Host}" + DecodeUrl(req.Path) + req.QueryString;
            }
            if (obj is HttpResponse)
            {
                return nameof(HttpResponse);
            }
            // 过滤掉各种会导致序列化 JSON 出错的对象类型
            if (obj is HttpContext)
                return false;
            if (obj is DbConnection)
                return false;
            if (obj is Stream || obj is StreamReader || obj is StreamWriter)
                return false;
            if (obj is NetworkStream || obj is TcpClient || obj is TcpListener)
                return false;
            if (obj is Delegate)
                return false;
            if (obj is IntPtr || obj is UIntPtr)
                return false;

            return obj;
        }

        public static string ToStr(object val)
        {

            if (val == null)
                return "";
            if (val is bool boolVal)
            {
                return boolVal ? "TRUE" : "FALSE";
            }
            //  // 对象是 long 类型，转换为字符串
            return val.ToString();
        }
        public static double ToDouble(object obj, double def)
        {
            if (obj == null)
            {
                return def;
            }

            if (obj is double)
            {
                return (double)obj;
            }
            if (obj is float)
            {
                return System.Convert.ToDouble((float)obj);
            }
            if (obj is int)
            {
                return System.Convert.ToDouble((int)obj);
            }
            if (obj is long)
            {
                return System.Convert.ToDouble((long)obj);
            }
            if (obj is decimal)
            {
                return System.Convert.ToDouble((decimal)obj);
            }
            if (obj is string)
            {
                if (double.TryParse((string)obj, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    return result;
                }
            }

            return def;
        }
        public static List<string> ConvertAndSortHashSet(HashSet<string> hashSet)
        {
            // 将 HashSet 转换为 List
            List<string> list = new List<string>(hashSet);

            // 对 List 进行排序
            list.Sort();

            return list;
        }

        /// <summary>
        ///   str def sply by space
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string castToJsonArrstr(string str)
        {
            str = str.Replace("/", " "); str = str.Replace(",", " ");
            string[] a = str.Split(" ");
            string originalString = EncodeJson(a);
            return originalString;
        }
        public static string castToJsonArrstrFrmCommastr(string str)
        {
            str = str.Replace("/", ",");
            string[] a = str.Split(",");
            string originalString = EncodeJson(a);
            return originalString;
        }
        public static string ToStrWzDef(string? fullName, string v)
        {
            if (fullName == null)
                return v;
            return fullName;
        }

        public static Hashtable ConvertArrayToHashtable(string[][] array)
        {
            var hashtable = new Hashtable();
            try
            {
                foreach (var pair in array)
                {
                    if (pair.Length == 2)
                    {
                        var key = pair[0];
                        var value = pair[1];
                        string orival = GetFieldAsStr(hashtable, key);
                        if(orival=="")
                        {
                            hashtable[key] = value;
                        }else
                        {
                            hashtable[key] = orival + "," + value;
                        }
                      
                    }
                }
            }catch(Exception e)
            {
                PrintExcept("ConvertArrayToHashtable", e);
            }
          

            return hashtable;
        }

        public static object CastToSerializableObjsOrSnglobj(object inputArray)
        {
            //todo 
            // if sortedlist hashtable ne?? dic 
            if (IsStr(inputArray))
                return inputArray;
            if (!IsArrOrColl(inputArray))
                return CastToSerializableObj(inputArray);
            // 创建一个新的 List 用于存储元素
            List<object> list = new List<object>();
            if (IsArrOrColl(inputArray))
            {
                list = CastArrCollToList(inputArray);
            }
            List<object> listRzt = CastToSerializableObjs(list);
            return listRzt;
        }

        public static string CastNormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "";
                //   throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            // Replace all forward slashes with backslashes
            string normalizedPath = path.Replace('/', '\\');

            // Remove redundant backslashes
            normalizedPath = Regex.Replace(normalizedPath, @"\\+", "\\");

            return normalizedPath;
        }
        private static List<object> CastArrCollToList(object inputArray)
        {
            List<object> list = new List<object>();
            // 检查对象是否为数组
            if (IsArray(inputArray))
            {
                List<object> li1 = ConvertArrayToList(inputArray);
                //   list.AddRange(li1);
                return li1;
            }
            // 检查对象是否为集合
            if (IsCollection(inputArray))
            {
                List<object> li1 = ConvertCollectionToList(inputArray);
                //   list.AddRange(li1);
                return li1;
            }
            return list;
        }


        public static string ToString(object o)
        {
            if (o == null)
                return "";
            // The default for an object is to return the fully qualified name of the class.
            return o.ToString();
        }
        public static Hashtable ToHashtable(JObject jsonObject)
        {
            Hashtable hashtable = new Hashtable();

            foreach (var property in jsonObject.Properties())
            {
                hashtable[property.Name] = property.Value.ToObject<object>();
            }

            return hashtable;
        }
     

        /// <summary>
        /// 将逗号分割的字符串转换为 ArrayList
        /// </summary>
        /// <param name="commaSeparatedString">逗号分割的字符串</param>
        /// <returns>转换后的 ArrayList</returns>
        public static ArrayList ConvertToArrayList(string commaSeparatedString)
        {
            if (string.IsNullOrEmpty(commaSeparatedString))
            {
                throw new ArgumentException("输入字符串不能为空", nameof(commaSeparatedString));
            }

            // 按逗号分割字符串
            string[] stringArray = commaSeparatedString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // 创建 ArrayList 并将字符串数组添加到其中
            ArrayList arrayList = new ArrayList(stringArray);

            return arrayList;
        }

        /// <summary>
        /// 将 ArrayList 转换为逗号分割的字符串
        /// </summary>
        /// <param name="arrayList">待转换的 ArrayList</param>
        /// <returns>逗号分割的字符串</returns>
        public static string ConvertArrayListToCommaSeparatedString(ArrayList arrayList)
        {
            if (arrayList == null)
            {
                return "";
            }

            // 将 ArrayList 中的元素转换为字符串
            var stringList = arrayList.Cast<object>()
                                      .Select(obj => obj?.ToString())
                                      .ToList();

            // 使用逗号连接字符串
            return string.Join(",", stringList);
        }

  

        public static Hashtable ToLower(Hashtable dic)
        {
            ConvertKeysToLowercase(dic);
            return dic;
        }
        /// <summary>
        /// 将 char 数组中的每个字符转换为字符串并放入 HashSet<string>
        /// </summary>
        /// <param name="charArray">字符数组</param>
        /// <returns>包含字符的 HashSet</returns>
        public static HashSet<string> ConvertCharsToHashSet(char[] charArray)
        {
            if (charArray == null)
            {
                throw new ArgumentNullException(nameof(charArray));
            }

            // 创建一个 HashSet<string> 用于存储字符的字符串表示
            HashSet<string> resultSet = new HashSet<string>();

            // 遍历字符数组并将每个字符转换为字符串
            foreach (char ch in charArray)
            {
                // 将字符转换为字符串并添加到 HashSet
                resultSet.Add(ch.ToString());
            }

            return resultSet;
        }
        public static int ToInt(object obj)
        {
            return System.Convert.ToInt32(obj);
        }
        public static Dictionary<string, string> ConvertToDictionary(string queryString)
        {
            var dictionary = new Dictionary<string, string>();

            // Use HttpUtility to parse the query string
            var queryParams = HttpUtility.ParseQueryString(queryString);

            foreach (string key in queryParams)
            {
                dictionary[key] = queryParams[key];
            }

            return dictionary;
        }
        public static string ConvertToMarkdownTable(object arr2)
        {
            object[] arr = (object[])arr2;
            StringBuilder sb = new StringBuilder();

            // 添加表头
            sb.AppendLine("| prm\t|Value\t|");
            sb.AppendLine("|-------|-------|");

            // 添加表格行
            for (int i = 0; i < arr.Length; i++)
            {
                object obj = arr[i];
                sb.AppendLine($"| {i}\t|{EncodeJson(obj)}\t|");
            }

            return sb.ToString();
        }

        public static Hashtable ConvertToHashtable(string queryString)
        {
            var hashtable = new Hashtable();

            // Use HttpUtility to parse the query string
            NameValueCollection queryParams = HttpUtility.ParseQueryString(queryString);

            foreach (string key in queryParams)
            {
                hashtable[key] = queryParams[key];
            }

            return hashtable;
        }

        public static string CastToStr(object args)
        {
            return args.ToString();
        }

        public static double ConvertStringToNumber(object str2)
        {
            try
            {
                string str = ToStr(str2);

                return (double.Parse(str));
            }
            catch (Exception e)
            {
                return 0;
            }


        }

        public static void ConvertXmlToHtml(string xmlFilePath, string htmlFilePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<head><title>XML Documentation</title></head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h1>api文档</h1>");

            using (var reader = XmlReader.Create(xmlFilePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "member")
                    {
                        string name = reader.GetAttribute("name");
                        sb.AppendLine($"<h2>{name}</h2>");

                        while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "member"))
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string elementName = reader.Name;
                                if (elementName == "example")
                                {
                                    reader.Read(); // Move to the text node or CDATA
                                    if (reader.NodeType == XmlNodeType.CDATA || reader.NodeType == XmlNodeType.Text)
                                    {
                                        string exampleText = reader.Value;
                                        sb.AppendLine($"<p><strong>范例:</strong> {exampleText}</p>");
                                    }
                                }
                                else
                                {//summar
                                    string prmname = reader.GetAttribute("name");
                                    SortedList stlst = new SortedList();
                                    stlst.Add("summary", "功能");

                                    stlst.Add("returns", "返回值");
                                    stlst.Add("param", "----参数");
                                    reader.Read(); // Move to the text node

                                    //  if (reader.NodeType == XmlNodeType.Text)
                                    {
                                        string text = reader.Value;

                                        sb.AppendLine($"<p><strong>{stlst[elementName] + "  " + prmname}:</strong> {text}</p>");
                                    }
                                }

                            }
                        }
                    }
                }
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            System.IO.File.WriteAllText(htmlFilePath, sb.ToString());
        }
        public static string ConvertXmlToJson(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);
            return JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented, true);
        }
    }
}
