global using static prjx.libx.bscGet;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prjx.libx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;

namespace prjx.libx
{
    internal class bscGet
    {
        public static HashSet<string> GetKeysAsHashSet(SortedList sortedList)
        {
            // 使用 HashSet 来存储并返回键集合
            return new HashSet<string>(sortedList.Keys.Cast<string>());
        }
        public static string GetListKeys(SortedList sortedList)
        {
            //  sortedList.Keys
            HashSet<string> keys = new HashSet<string>();
            // 使用 StringBuilder 来提高性能
            var sb = new System.Text.StringBuilder();

            foreach (DictionaryEntry entry in sortedList)
            {
                // 获取键并追加到 StringBuilder
                var key = entry.Key.ToString();
                sb.Append(key).Append(' ');
            }

            // 移除末尾多余的空格
            if (sb.Length > 0)
            {
                sb.Length--;  // 移除最后一个空格
            }

            return sb.ToString();
        }

        public static SortedList NewSortedListFrmIni(string v)
        {
            return GetSortedlistFrmIni(v);
        }

        public static SortedList GetSortedlistFrmIni(string filePath)
        {
            var sortedList = new SortedList();

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("INI file not found", filePath);
            }

            var lines = System.IO.File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // 跳过空行和注释行
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";"))
                {
                    continue;
                }

                // 处理键值对
                var keyValue = trimmedLine.Split('=', 2);
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();

                    sortedList[key] = value;
                }
            }

            return sortedList;
        }

        public static string GetFileNamesAsJSONFrmFldr(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);
            var fileNames = Array.ConvertAll(files, Path.GetFileName);
            return JsonConvert.SerializeObject(fileNames, Formatting.Indented);
        }
        public static List<Dictionary<string, string>> GetListFrmIniFL(string dbf)
        {
            return ormIni.qryToDic(dbf);
        }
        public static List<SortedList> GetListDicFrmIniFL(string dbf)
        {
            return ormIni.qryV2(dbf);
        }

        public static Dictionary<string, string> GetDicFromUrl(string url136)
        {
            // 找到 '@' 符号的位置
            int atIndex = url136.IndexOf('@');
            if (atIndex == -1)
            {
                Console.WriteLine("Invalid URL format.");
                return new Dictionary<string, string>();
            }

            // 分割出用户名和密码部分
            string userInfo = url136.Substring(0, atIndex);
            string hostPort = url136.Substring(atIndex + 1);

            // 找到 ':' 符号的位置
            int colonIndex = userInfo.IndexOf(':');
            if (colonIndex == -1)
            {
                Console.WriteLine("Invalid user info format.");
                return new Dictionary<string, string>();
            }

            // 提取用户名和密码
            string username = userInfo.Substring(0, colonIndex);
            string password = userInfo.Substring(colonIndex + 1);

            // 提取主机和端口
            colonIndex = hostPort.IndexOf(':');
            if (colonIndex == -1)
            {
                Console.WriteLine("Invalid host and port format.");
                return new Dictionary<string, string>();
            }

            string host = hostPort.Substring(0, colonIndex);
            string port = hostPort.Substring(colonIndex + 1);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("host", host);
            dic.Add("u", username);
            dic.Add("pwd", password);
            return dic;
        }

        public static void SetElmt(List<SortedList> li127, SortedList SortedList1)
        {
            SortedList obj = GetSortedlist(li127, SortedList1["id"].ToString());

            if (obj == null)
                li127.Add(SortedList1);
            else
            {
                obj.Clear();
                CopySortedListCloneMode(SortedList1, obj);
            }
        }

        public static SortedList GetSortedlist(List<SortedList> li127, string v)
        {
            foreach (SortedList obj in li127)
            {
                if (GetField(obj, "id") == v)
                    return obj;
            }
            return null;
        }

        public static object GetField(SortedList  map, string v)
        {
            if (map == null)
                return "";
            if (map.ContainsKey(v))
                return map[v];
            return "";

        }
        public static string GetField(Dictionary<string, string> map, string v)
        {
            if (map == null)
                return "";
            if (map.ContainsKey(v))
                return map[v];
            return "";

        }
        public static void wrtLgTypeDate(string logdir, object o)
        {
            // 创建目录
            System.IO.Directory.CreateDirectory(logdir);
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"{logdir}/{timestamp}.json";
            file_put_contents(fileName, json_encode(o), false);

        }
        public static SortedList<string, string> ReadJsonFileToSortedList(string filePath)
        {
            // 创建一个新的 SortedList 来存储结果
            SortedList<string, string> sortedList = new SortedList<string, string>();

            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            // 读取文件的内容
            string jsonContent = System.IO.File.ReadAllText(filePath);

            // 解析 JSON 数据为字典
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);

            // 将字典数据添加到 SortedList 中
            foreach (var kvp in data)
            {
                sortedList[kvp.Key] = kvp.Value;
            }

            return sortedList;
        }
        public static string GetElmt(string[] array, int index)
        {
            if (index < 0 || index >= array.Length)
            {
                return "";
            }

            string element = array[index];

            return element?.Trim().ToUpper();
            //   return   array[index].Trim().ToUpper();
        }
        public static void SetField938(SortedList SortedList1_iot, string key, object objSave)
        {
            if (SortedList1_iot.ContainsKey(key))
            {
                //remove moshi 更好，因为可能不同的类型 原来的
                SortedList1_iot.Remove(key.ToString());
            }
            SortedList1_iot.Add(key, objSave);
        }
        static List<string> getListFrmJsonFil(string filePath)
        {
            List<string> jsonStringList = new List<string>();

            try
            {
                // 读取 JSON 文件内容
                string jsonString = System.IO.File.ReadAllText(filePath);
                // Configure JsonSerializerOptions to allow reflection-based serialization
                var options = new JsonSerializerOptions
                {
                    // Enable reflection-based serialization
                    //   TypeNameHandling = TypeNameHandling.All, // or TypeNameHandling.Auto
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };
                // 将 JSON 字符串解析为 List<string>
                jsonStringList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(jsonString, options);
            }
            catch (FileNotFoundException)
            {
                ConsoleWriteLine($"File not found: {filePath}");
            }
            catch (System.Text.Json.JsonException)
            {
                ConsoleWriteLine($"Invalid JSON format in file: {filePath}");
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"Error reading JSON file: {ex.Message}");
            }

            return jsonStringList;
        }

        public static string ldfld544(Dictionary<string, string> parse_str1, string fld)
        {
            if (parse_str1.ContainsKey(fld))
                return parse_str1[fld];
            else
                return "";
        }
        public static string LoadFieldTryGetValue(Dictionary<string, StringValues> whereExprsObj, string fld)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(fld))
                return whereExprsObj[fld];
            else
                return null;

            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}

        }
        internal static string ldfld_TryGetValueDfEmpy(Dictionary<string, StringValues> whereExprsObj, string k)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            try
            {
                return whereExprsObj[k];

            }
            catch (Exception ex)
            {
                return null;
            }
            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}

        }
        public static object LoadField(SortedList hashobj, string fld, object dfval)
        {
            try
            {
                if (hashobj.ContainsKey(fld))
                    return hashobj[fld];
                else
                    return dfval;
            }
            catch (Exception e)
            {
                return dfval;
            }


        }
        internal static string LoadFieldDefEmpty(SortedList<string, string> row, object fldx)
        {
            if (fldx == null)
                return "";
            string fld = fldx.ToString();
            if (row.ContainsKey(fld))
            {
                if (row[fld] == null)
                    return "";
                return row[fld].ToString();
            }
            else
                return "";
            //if (row[fld] == null)
            //    return "";
            //return row[fld].ToString();
        }

        internal static string LoadFieldDefEmpty(SortedList row, string fld)
        {
            if (row.ContainsKey(fld))
            {
                if (row[fld] == null)
                    return "";
                return row[fld].ToString();
            }
            else
                return "";
            //if (row[fld] == null)
            //    return "";
            //return row[fld].ToString();
        }

        public static List<T> GetRdmList<T>(List<T> results)
        {
            List<T> results22;
            Random rng = new();

            results22 = [.. results.OrderBy(x => rng.Next())];
            return results22;
        }



        internal static string LoadFieldTryGetValueAsStrDefNull(SortedList whereExprsObj, string fld)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(fld))
                if (whereExprsObj[fld] == null)
                    return null;
                else
                    return whereExprsObj[fld].ToString();
            else
                return null;

            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}

        }

        public static object ldfld(List<Dictionary<string, object>> lst, string fld, string v2)
        {
            if (lst.Count > 0)
            {
                Dictionary<string, object> d = lst[0];
                if (d.ContainsKey(fld))
                    return d[fld];

                else
                    return v2;


            }
            return v2;
        }


        public static void SetIdProperties(ArrayList arrayList)
        {
            foreach (var item in arrayList)
            {
                SortedList sortedList1 = (SortedList)item;
                sortedList1.Add("id", sortedList1["Guid"]);

            }
        }

        public static string ldfld_TryGetValueAsStrDfEmpty(SortedList whereExprsObj, string fld)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(fld))
                if (whereExprsObj[fld] == null)
                    return "";
                else
                    return whereExprsObj[fld].ToString();
            else
                return "";
        }

        internal static void SetFieldReplaceKeyV(SortedList obj, string fld, object v)
        {
            if (fld == null)
                return;
            if (obj.ContainsKey(fld))
                obj[fld] = v;
            else
                obj.Add(fld, v);
        }


        public static string LoadFieldAsStr(Dictionary<string, string> parse_str1, string fld)
        {
            if (parse_str1.ContainsKey(fld))
                return parse_str1[fld];
            else
                return "";
        }
        public static void SetField<t>(SortedList<string, t> cfg, string f, object v)
        {
            if (cfg.ContainsKey(f))
                cfg.Remove(f);

            cfg.Add(f, (t)v);
        }

        public static void SetField(SortedList cfg, string f, object v)
        {
            if (cfg.ContainsKey(f))
                cfg.Remove(f);

            cfg.Add(f, v);
        }

        public static string GetKeysCommaSeparated(SortedList list)
        {
            // 检查输入参数是否为 null
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            // 创建一个 StringBuilder 来构建结果字符串
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (var key in list.Keys)
            {
                // 将每个键添加到 StringBuilder 中，并附加逗号
                sb.Append(key).Append(",");
            }

            // 移除最后一个多余的逗号
            if (sb.Length > 0)
            {
                sb.Length--; // 或者 sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        public static string GetExtension(string url)
        {
            try
            {
                var ext = Path.GetExtension(url);
                ext = ext.ToLower().Trim();
                return ext;
            }
            catch (Exception e)
            {
                return "";
            }

        }


        public static string GetFieldAsStr10(Hashtable cfgDic, string v)
        {
            if (cfgDic.ContainsKey(v))
                return ToStr(cfgDic[v]);
            return "";
        }

      
        public static object GetUuid()
        {
            Guid newGuid = Guid.NewGuid();
            return newGuid.ToString();
        }

        public static void SetFieldAddRplsKeyV(SortedList<string, object> listIot, string? key, object objSave)
        {
            if (listIot.ContainsKey(key))
                listIot.Remove(key);
            
                listIot.Add(key, objSave);
        }
        public static void SetFieldAddRplsKeyV(Dictionary<string,object> listIot, string? key, object objSave)
        {
            if (listIot.ContainsKey(key))
                listIot[key] = objSave;
            else
                listIot.Add(key, objSave);
        }
        public static void SetFieldAddRplsKeyV(SortedList listIot, string? key, object objSave)
        {
            if (listIot.ContainsKey(key))
                listIot[key] = objSave;
            else
                listIot.Add(key, objSave);
        }


        public static SortedList<string, string> ReadIniFileToSortedList(string filePath)
        {
            // 创建一个新的 SortedList 来存储结果
            SortedList<string, string> sortedList = new SortedList<string, string>();

            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            // 读取文件的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            // 遍历每一行
            foreach (var line in lines)
            {
                // 跳过空行和注释行
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith(";") || line.TrimStart().StartsWith("#"))
                {
                    continue;
                }

                // 找到等号的位置
                int equalsIndex = line.IndexOf('=');
                if (equalsIndex > 0)
                {
                    // 提取键和值
                    string key = line.Substring(0, equalsIndex).Trim();
                    string value = line.Substring(equalsIndex + 1).Trim();

                    // 将键值对添加到 SortedList 中
                    sortedList[key] = value;
                }
            }

            return sortedList;
        }
        public static SortedList<string, MethodInfo> methodss445;
        public static IEnumerable<MethodInfo> methodss546;
        public static MethodInfo? GetMethInfo(string methodName)
        {
            Print(" fun GetMethInfo()" + methodName);
            if (methodss546 == null)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Print("assemblies.Len=>" + assemblies.Length);
                IEnumerable<Type> typeList = assemblies
                                .SelectMany(assembly => assembly.GetTypes());
                Print("typeList.Len=>" + typeList.Count());
                IEnumerable<MethodInfo> methodss = typeList
                                .SelectMany(type => type.GetMethods());  //BindingFlags.Static| BindingFlags.Public
                Print("methodss.Len=>" + methodss.Count());
                methodss546 = methodss;
                methodss445 = new();
                foreach (MethodInfo method in methodss)
                {
                    SetField(methodss445, method.Name, method);
                    //     File.AppendAllTextAsync("ms510.txt", method.Name + "\n");
                }

                // WriteAllText("meth504.json", methodss445);WbapiXgetlist
            }
            if ("WbapiXgetlist" == methodName)
                Print("dbg244");
            var methodInfo = GetField(methodss445, methodName, null);
            //pef  FirstOrDefault perf not god
            //var methodInfo2 = methodss546
            //    .FirstOrDefault(method =>
            //        method.Name == methodName
            //      );
            return (MethodInfo?)methodInfo;
        }

        public static MethodInfo GetField(SortedList<string, MethodInfo> methodss445, string key_methodName, object def)
        {
            if (methodss445.ContainsKey(key_methodName))
                return methodss445[key_methodName];
            return (MethodInfo)def;
        }

        public static string[] ReadAllLines(string filePath)
        {
            return System.IO.File.ReadAllLines(filePath);
        }

        public static string GetCmdV2(string? v)
        {
            if (string.IsNullOrEmpty(v)) return "";
            if (v.StartsWith("/"))
            {
                v = v.Replace("@" + botname, "");
                string s = v.ToString().Substring(1);
                string[] a = s.Split(" ");
                return a[0];
            }

            else
                return "";
        }

        public static string GetCmdFun(string? v)
        {
            if (string.IsNullOrEmpty(v)) return "";
            if (v.StartsWith("/"))
            {
                v = v.Replace("@" + botname, "");
                return v.ToString().Substring(1);
            }

            else
                return "";
        }

        public static string GetParksByCity(string city)
        {
            //妙园区4data.txt

            HashSet<string> hs = GetHashsetFromFilTxt($"{prjdir}/cfg_cmd/{city}园区4data.txt");

            return JoinWzComma(hs);
        }
        public static Hashtable GetHashtabFromIniFl(string v)
        {
            return ReadIniFile(v);
        }

        public static Hashtable ReadIniFile(string filePath)
        {
            Hashtable iniData = new Hashtable();

            // 按行读取 INI 文件内容
            string[] lines = ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // 忽略空行和注释行
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                    continue;

                // 处理键值对行
                int equalIndex = trimmedLine.IndexOf('=');
                if (equalIndex > 0)
                {
                    string key = trimmedLine.Substring(0, equalIndex).Trim();
                    string value = trimmedLine.Substring(equalIndex + 1).Trim();
                    iniData[key] = value;
                }
            }

            return iniData;
        }
      
        
        public static SortedList<string, string> LoadSortedListFromIni(string filePath)
        {
            var result = new SortedList<string, string>(StringComparer.OrdinalIgnoreCase);

            // 读取文件所有行
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // 跳过空行和注释行
                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // 处理键值对
                int equalsIndex = trimmedLine.IndexOf('=');
                if (equalsIndex > 0)
                {
                    string key = trimmedLine.Substring(0, equalsIndex).Trim();
                    string value = trimmedLine.Substring(equalsIndex + 1).Trim();
                    result[key] = value;
                }
            }

            return result;
        }
        public static Dictionary<string, string> ldDicFromQrystr(string queryString)
        {
            return ConvertToDictionary(queryString);
        }

        public static string ReadAllText(string f)
        {
            if (IsNotExistFil(f))
                return "";
            return System.IO.File.ReadAllText(f);
        }
        public static List<SortedList> ReadAsListHashtable(string f)
        {
            //   File
            return json_decode(System.IO.File.ReadAllText(f));
        }

        public static string GetStr(string? v)
        {
            if (string.IsNullOrEmpty(v)) return "";

            return v;
        }
        public static HashSet<string> GetHashsetFromFilTxt(string v)
        {
            return ReadTextFileToHashSet(v);
        }


        public static HashSet<string> GetHashsetEmojiCmn()
        {
            char[] a = ReadFileAsCharArray($"{prjdir}/db/commonEmoji.txt");

            HashSet<string> hs = ConvertCharsToHashSet(a); hs.Remove("\n");
            hs.Remove(" "); hs.Remove("\t"); hs.Remove("\r");
            return hs;
        }



        private static HashSet<string> GetHashsetCharsFromFilTxt(string v)
        {
            throw new NotImplementedException();
        }
        public static string GetParksByCountry(string ctry)
        {
            Hashtable ctrys = GetHashtabFromIniFl($"{prjdir}/cfg_cmd/国家代码.ini");
            //MMR_pks.txt
            var ctryCode = ctrys[ctry];
            HashSet<string> hs = GetHashsetFromFilTxt($"{prjdir}/cfg_cmd/{ctryCode}_pks.txt");

            return JoinWzComma(hs);
        }

        public static string GetMethodFullName(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            // 获取声明类的全名（包含命名空间）
            string declaringTypeFullName = methodInfo.DeclaringType.FullName;

            // 获取方法名
            string methodName = methodInfo.Name;

            // 获取参数信息并构建参数列表
            var parameters = methodInfo.GetParameters();
            string parameterList = string.Join(", ", System.Array.ConvertAll(parameters, p => p.ParameterType.Name + " " + p.Name));

            // 拼接完整的方法名
            return $"{declaringTypeFullName}.{methodName}({parameterList})";
        }
        /// <summary>
        /// 读取指定路径的文本文件，并返回字符数组
        /// </summary>
        /// <param name="filePath">文本文件的路径</param>
        /// <returns>文件内容的字符数组</returns>
        public static char[] ReadFileAsCharArray(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("文件路径不能为空", nameof(filePath));
            }

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的文件未找到", filePath);
            }

            // 读取文件内容
            string fileContent = System.IO.File.ReadAllText(filePath);

            // 将内容转换为字符数组并返回
            return fileContent.ToCharArray();
        }



        public static HashSet<string> ReadTextFileToHashSet(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return new HashSet<string>();
            }

            var hashSet = new HashSet<string>();
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Trim whitespace and remove empty lines
                    line = line.Trim();
                    if (line.Length > 0)
                    {
                        hashSet.Add(line);
                    }
                }
            }

            return hashSet;
        }
        public static string SetField4qrystr(string qrystr1, string k, object v)
        {
            SortedList qryMap = GetHashtableFromQrystr(qrystr1);
            SetField(qryMap, k, v);
            return CastHashtableToQuerystringNoEncodeurl(qryMap);
        }

        public static string GetField4qrystr(string qrystr1, string k, object v)
        {
            SortedList qryMap = GetHashtableFromQrystr(qrystr1);
            return GetFieldAsStrDep(qryMap, k);
        }

        public static string DelField4qrystr(string qrystr1, string k)
        {
            SortedList qryMap = GetHashtableFromQrystr(qrystr1);
            qryMap.Remove(k);
            return CastHashtableToQuerystringNoEncodeurl(qryMap);
        }

        public static string SetKv(string qrystr1, string k, object v)
        {
            SortedList qryMap = GetHashtableFromQrystr(qrystr1);
            SetField(qryMap, k, v);
            return CastHashtableToQuerystringNoEncodeurl(qryMap);
        }
        public static string DelKeys(string ks, string qrystr)
        {
            SortedList qryMap = GetHashtableFromQrystr(qrystr);
            HashSet<string> hs = GetHashsetFromStr(ks);
            foreach (string k in hs)
            {
                qryMap.Remove(k);
            }
            qryMap.Remove("");
            return CastHashtableToQuerystringNoEncodeurl(qryMap);
        }

        private static HashSet<string> GetHashsetFromStr(string ks)
        {
            string[] stringArray = ks.Split(" ");
            // 使用 HashSet 的构造函数将数组转换为 HashSet
            HashSet<string> resultSet = new HashSet<string>(stringArray);
            return resultSet;
        }

        public static HashSet<string> GetHashsetFrmCommaStr(string pkrPrm)
        {
            return ConvertCommaSeparatedStringToHashSet(pkrPrm);
        }
      
        public static List<SortedList> GetListFltrByQrystr(string fromDdataDir, object shares, string qrtStr4Srch)
        {
            PrintTimestamp(" start fun GetListFltrByQrystr");
            Func<SortedList, bool> whereFun = CastQrystr2FltrCdtFun(qrtStr4Srch);
            List<SortedList> list = GetListFltr(fromDdataDir, ToStr(shares), whereFun);
            PrintTimestamp(" end fun GetListFltrByQrystr");
            return list;
        }

        public static string GetParkNamesFromJson(string jsonData, string parentName)
        {
            // 解析 JSON 数据
            JArray jsonArray = JArray.Parse(jsonData);

            // 查找并返回园区名称
            return FindParkNames(jsonArray, parentName);
        }

        public static string FindParkNames(JArray jsonArray, string parentName)
        {
            foreach (var item in jsonArray)
            {
                // 查找目标父级名称
                if (item["name"]?.ToString() == parentName)
                {
                    // 获取子节点并提取园区名称
                    return ExtractParkNames(item);
                }

                // 递归查找子节点
                if (item["children"] != null)
                {
                    string result = FindParkNames((JArray)item["children"], parentName);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }

            return string.Empty;
        }

        public static string ExtractParkNames(JToken parent)
        {
            List<string> parkNames = new List<string>();

            if (parent["children"] != null)
            {
                foreach (var child in parent["children"])
                {
                    string name = child["name"]?.ToString();
                    if (!string.IsNullOrEmpty(name))
                    {
                        parkNames.Add(name);
                    }
                }
            }

            return string.Join(" ", parkNames);
        }

        public static SortedList GetHashset
            (string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static SortedList LoadHashtable(string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static SortedList ReadAsHashtable(string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static object ReadAsObj(string f)
        {
            return json_decodeObj(System.IO.File.ReadAllText(f));
        }
        public static JsonObject ReadAsJson(string f)
        {
            return json_decodeJonObj(System.IO.File.ReadAllText(f));
        }

        public static HashSet<string> LoadHashstWordsFromFile(string filePath)
        {
            var words = new HashSet<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 拆分行中的单词，按空格和回车拆分
                        var splitWords = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in splitWords)
                        {
                            var word1 = word.Trim();
                            words.Add(word1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Print("Error reading file: " + ex.Message);
            }

            return words;
        }

        public static int GetFieldAsInt526(Dictionary<string, string> map, string v1, int v2)
        {
            if (map.ContainsKey(v1))
                return ToInt(map[v1]);
            return v2;
        }

        public static Dictionary<string, string> GetDicFromIni(string v)
        {
            Hashtable li = GetHashtabFromIniFl(v);
            return ToDictionary(li);
        }

        public static Dictionary<string, string> GetDicFromIniV2(string v)
        {
            Hashtable li = GetHashtabFromIniFl(v);
            return ToDictionary(li);
        }

        public static object GetField(SortedList row, string fld, object dfv)
        {
            if (row.ContainsKey(fld))
                return row[fld];
            return dfv;
        }

        public static object GetField(SortedList<string,object> row, string fld, object dfv)
        {
            if (row.ContainsKey(fld))
                return row[fld];
            return dfv;
        }

        public static object GetField(object Obj, string fld, object defVal)
        {
            var objtype = Gettype(Obj);
            if (Obj is SortedList)
            {
                return LoadFieldFrmStlst((SortedList)Obj, fld, defVal.ToString());
            }
            else
            {
                return LoadField233(Obj, fld, defVal);
            }
        }
        public static object getFld(object Obj, string fld, object defVal)
        {

            if (Obj is SortedList)
            {
                return LoadFieldFrmStlst((SortedList)Obj, fld, defVal.ToString());
            }
            else
            {
                return LoadField233(Obj, fld, defVal);
            }
        }
        public static string LoadFieldAsStr(object obj, string fld, object defVal)
        {
            return LoadField233(obj, fld, "").ToString();
        }

        public static object LoadField(Hashtable hstb, string fld, object defVal)
        {
            if (hstb.ContainsKey(fld))
                return hstb[fld];
            return defVal;
        }

        //public static HashSet<string> LoadHashsetFrmFJsonDecd(string v)
        //{
        //    return (ReadFileToHashSet(v));
        //}
        public static HashSet<string> ReadFileToHashSet(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                HashSet<string> hashSet = JsonConvert.DeserializeObject<HashSet<string>>(json);
                return hashSet;
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"An error occurred: {ex.Message}");
                return new HashSet<string>();
            }
        }

        public static HashSet<string> LoadHashsetReadFileLinesToHashSet(string filePath)
        {
            HashSet<string> lines = new HashSet<string>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim().Length > 0)
                            lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The file could not be read: {e.Message}");
            }

            return lines;
        }

        public static void setFld(object Obj, string fld, object v)
        {
            if (Obj is SortedList)
            {
                SetField938((SortedList)Obj, fld, v);
            }
            else
            {
                SetProperty(Obj, fld, v);
            }
        }

        public static string GetFieldAsStr(Hashtable sortedList, string key)
        {
            if (sortedList.ContainsKey(key))
                return sortedList[key].ToString();
            //      var obj = GetField(sortedList, key, "");
            //   return ToStr(obj);
            return "";
        }

        public static string GetFieldAsStr(Dictionary<string, string> sortedList, string key)
        {
            if(sortedList.ContainsKey(key))
                return ToStr(sortedList[key]);
            return "";
        }

        //dep
        public static string GetOriginalMethodName()
        {
            var stackTrace = new StackTrace(true);
            for (int i = 1; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(1);
                var method = frame.GetMethod();

                if (method.DeclaringType?.Name.EndsWith("MoveNext") == false)
                {
                    return method.DeclaringType?.FullName;
                }
            }
            return "Unknown";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetFunNameCurrent()
        {
            // 获取调用栈   StackTrace(bool fNeedFileInfo)
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(1); // 获取调用者的栈帧
            MethodBase MethodBase1 = frame.GetMethod();
            var name = MethodBase1.Name;
            //--process async fun name
            if (name.EndsWith("MoveNext"))
            {
                //MethodBase.DeclaringType 属性返回声明方法的类型（类）的 Type 对象。
                //但是这里估计会根据方法生成一个动态类，所以命名类似的
                // mdsj.lib.JsonStore+<ListFromDirJsonsAsync>d__4
                return ToStrWzDef( MethodBase1.DeclaringType?.FullName,"funxx");
            }

            //nml method name
            return name;
        }

     

        //  sortedList
        //todo bsc fun must smple so in foreach is fast..
        public static string GetFieldAsStrDep(SortedList sortedList, string key)
        {
            var obj = GetField(sortedList, key, "");
            return ToStr(obj);
        }
        public static double GetFieldAsNumber(SortedList sortedList, string fieldName)
        {
            var obj = GetField(sortedList, fieldName, 0);
            return ToNumber(obj);
        }

        public static List<SortedList> GetListHashtableFromJsonFil(string dbf)
        {


            var list = new List<SortedList>();
            if (IsNotExistFil(dbf))
                return list;
            try
            {
                string json = File.ReadAllText(dbf);
                JArray jsonArray = JArray.Parse(json);

                foreach (JObject obj in jsonArray)
                {
                    var sortedList = new SortedList();

                    foreach (var property in obj.Properties())
                    {
                        sortedList.Add(property.Name, property.Value.ToObject<object>());
                    }

                    list.Add(sortedList);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The file could not be read: {e.Message}");
            }

            return list;
        }

        public static SortedList GetHashtableFromQrystr(string queryString)
        {
            return ToSortedListFrmQrystr(queryString);
        }
        public static string GetMethodName(Delegate del)
        {
            // 使用反射获取方法信息
            var methodInfo = del.Method;
            return methodInfo.Name;
        }

        public static string GetFunFromPathUrl(string path)
        {


            path = path.Replace("//", "/");
            path = path.Replace("//", "/");
            path = path.Substring(1);
            path = path.Replace("/", "");
            return path;
        }
        public static void WriteObj(string f, object obj)
        {

            WriteAllText(f, json_encode(obj));
        }

        public static void WriteAllText(string f, object update)
        {
            var __METHOD__ = nameof(WriteAllText);
            PrintCallFunArgs(__METHOD__, func_get_args(update, f));

            try
            {
                Mkdir4File(f);
                if (IsStr(update))
                {
                    System.IO.File.WriteAllText(f, (update.ToString()));
                    return;
                }

                System.IO.File.WriteAllText(f, json_encode(update));
            }
            catch (Exception e)
            {
                PrintExcept("WriteAllText", e);
            }
            PrintRet(__METHOD__, 0);

        }

        public static void WriteAllText218(string f, object txt)
        {
            Print($" fun WriteAllText {f}");
            try
            {
                if (IsStr(txt))
                {
                    WriteAllText(f, txt.ToString());
                }
                else
                    WriteAllText(f, EncodeJson(txt));
            }
            catch (Exception e)
            {
                PrintCatchEx("WriteAllText", e);
            }

        }
        public static Dictionary<string, string> GetDicFromQrtstr(string qrystr)
        {
            return ToDictionaryFrmQrystr(qrystr);
        }
        public static void WriteAllText(string f, string txt)
        {
            //   Print($" fun WriteAllText {f}");
            Mkdir4File(f);
            try
            {
                System.IO.File.WriteAllText(f, txt);
            }
            catch (Exception e)
            {
                PrintCatchEx("WriteAllText", e);
            }

        }

        public static void SetHashstToFil(HashSet<string> downedUrl, string v)
        {
            WriteAllText(v, EncodeJson(downedUrl));
        }
        public static void SetProperty(object obj, string prop, object v)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(prop);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, v);
            }
            else
            {
                Print("The object does not have a writable 'Name' property.");
            }
        }

        public static HashSet<string> LoadHashset(string input)
        {
            // 分割字符串并转换为 HashSet
            string[] elements = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> stringSet = new HashSet<string>(elements);

            return stringSet;
        }
        public static HashSet<string> LoadHashsetFrmFL(string f)
        {
            return LoadHashset(ReadAllText(f));
        }

        public static object LoadField233(object obj, string fld, object defVal)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(fld);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                return propertyInfo.GetValue(obj);
            }
            return defVal;
        }

        public static Hashtable LoadHashtableFromQrystrDep(string queryString)
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

        public static List<string> ReadFileToLines(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public static List<string> loadLstWdsFrmDataDirHtml(string FolderPath)
        {

            object v = Callx(ExtractWordsFromFilesHtml, FolderPath);
            HashSet<string> weds = (HashSet<string>)v;
            weds = RemoveElementsContainingNumbers(weds);
            var wds = ConvertAndSortHashSet(weds);
            WriteAllText("word7000dep.json", wds);
            return wds;

        }

        public static List<string> GetListFrmFil(string v)
        {
            return ReadFileToLines(v);
        }
        public static string LoadfieldDfempty(Dictionary<string, StringValues> whereexprsobj, string v)
        {
            var x = LoadField232(ConvertToStringDictionary(whereexprsobj), v);
            return x;
        }
        public static string Gettype(object obj)
        {
            return obj.GetType().ToString();
        }

        public static int GetFieldAsInt147(Hashtable dafenObj, string fld, int df)
        {
            try
            {
                if (dafenObj.ContainsKey(fld))
                    return ToInt(dafenObj[fld].ToString());
                return df;
            }
            catch (Exception e)
            {
                return df;
            }

        }
        public static int GetFieldAsInt(Hashtable dafenObj, string fld, int df)
        {
            try
            {
                var obj = GetField(dafenObj, fld, df);
                return ToInt(obj);
            }
            catch (Exception e)
            {
                PrintCatchEx(nameof(GetFieldAsInt), e);
                return df;
            }

        }

        public static int GetFieldAsInt(SortedList dafenObj, string fld, int df)
        {
            try
            {
                var obj = GetField(dafenObj, fld, df);
                return ToInt(obj);
            }
            catch (Exception e)
            {
                PrintCatchEx(nameof(GetFieldAsInt), e);
                return df;
            }

        }

        public static List<SortedList> GetHashtabEsFrmDbf(string dbFileName)
        {


            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, func_get_args(MethodBase.GetCurrentMethod(), dbFileName));

            if (!File.Exists(dbFileName))
                File.WriteAllText(dbFileName, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            string txt = File.ReadAllText(dbFileName);
            if (txt.Trim().Length == 0)
                txt = "[]";
            var list = JsonConvert.DeserializeObject<List<SortedList>>(txt);

            //   ArrayList list = (ArrayList)JsonConvert.DeserializeObject(File.ReadAllText(dbFileName));

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
            PrintRet(MethodBase.GetCurrentMethod().Name, ArrSlice(list, 0, 1));

            return list;
        }


        public static string GetFld(JObject? jo, string fld, string v2)
        {
            // 获取 chat.type 属性
            JToken chatTypeToken = jo.SelectToken(fld);

            if (chatTypeToken != null)
            {
                string chatType = chatTypeToken.ToString();
                return chatType;
                // print("chat.type: " + chatType);
            }
            else
            {
                return v2;
            }
        }
        public static SortedList<string, string> LdHstbEsFrmJsonFile(string v)
        {
            return ReadJsonFileToSortedList(v);
        }
        public static Dictionary<string, string> LoadDic4qryCdtn(string qrystr)
        {
            var filters2 = ldDicFromQrystr(qrystr);
            var filters = RemoveKeys(filters2, "page limit pagesize from");
            return filters;
        }
        public static object LoadFieldDfemp(SortedList row, string v)
        {
            return LoadFieldFrmStlst(row, v, "");
        }

        public static SortedList<string, string> LoadHashtabEsFrmIni(string v)
        {
            return ReadIniFileToSortedList(v);
        }
        public static List<Hashtable> getListFrmDir(string directoryPath)
        {
            List<Hashtable> fileList = new List<Hashtable>();

            // 获取目录中所有 JSON 文件的路径
            string[] jsonFiles = Directory.GetFiles(directoryPath, "*.json");

            foreach (string filePath in jsonFiles)
            {
                try
                {
                    // 读取文件内容
                    string jsonContent = ReadAllText(filePath);

                    // 解析 JSON 文件为 JObject
                    JObject jsonObject = JObject.Parse(jsonContent);

                    // 转换为 Hashtable
                    Hashtable hashtable = ToHashtable(jsonObject);
                    hashtable.Add("fname", Path.GetFileName(filePath));
                    hashtable.Add("fpath", filePath);
                    // 获取文件名（不包括扩展名）
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    hashtable.Add("basename", fileNameWithoutExtension);
                    // 添加到 List 中
                    fileList.Add(hashtable);
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine($"Error reading or parsing file {filePath}: {ex.Message}");
                }
            }

            return fileList;
        }
        //public static string GetOSVersion()
        //{
        //    OperatingSystem os = Environment.op;
        //    return $"{os.Platform} {os.Version}";
        //}

        public static string GetOSVersion()
        {
            // 获取操作系统版本信息
            Version version = Environment.OSVersion.Version;
            string platform = Environment.OSVersion.Platform.ToString();

            // 构建操作系统版本信息字符串
            string osVersion = $"OS: {platform}, Version: {version.Major}.{version.Minor}.{version.Build}";

            return osVersion;
        }

        public static SortedList GetDicFromJson(string jsonstr)
        {
            return ToSortedListFrmJson(jsonstr);
        }

     
        public static string GetFieldAsStr(SortedList row, string fld, string dfv = "")
        {

            if (!row.ContainsKey(fld))
                return dfv;


            object? v = row[fld];
            if (v == null)
                return dfv;

            return v.ToString();




        }
        public static string GetFieldAsStr1037(SortedList row, string fld, string dfv = "")
        {

            if (row.ContainsKey(fld))
                return row[fld].ToString();
            return dfv;
        }
        public static object LoadFieldFrmStlst(SortedList row, string fld, string dfv)
        {
            if (row.ContainsKey(fld))
                return row[fld];
            return dfv;
        }



        public static string LoadField232(Dictionary<string, string> whereExprsObj, string fld)
        {
            if (whereExprsObj.ContainsKey(fld))
                return whereExprsObj[fld];
            return "";
        }
    }
}
