global using static prjx.libx.arrCls;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections;
using Microsoft.Extensions.Primitives;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace prjx.libx
{
    public class arrCls
    {

        public static void UpdtElmtSqlmode(List<SortedList> li127, SortedList SortedList1)
        {
            SortedList obj = GetSortedlist(li127, SortedList1["id"].ToString());

            if (obj == null)
                li127.Add(SortedList1);
            else
            {
                CopySortedList(SortedList1, obj);
            }
        }
        public static void arr_cut()
        {
            // 定义两个 HashSet
            HashSet<string> set1 = new HashSet<string> { "apple", "banana", "cherry", "date" };
            HashSet<string> set2 = new HashSet<string> { "banana" };



            // 从 set1 中移除 set2 中的元素
            set1.ExceptWith(set2);

            // 打印减法操作后的集合
            ConsoleWriteLine("Set 1 after subtraction:");
        }

        public static List<SortedList> TransltKey(ArrayList lst458, string TransFfilePath)
        {
            SortedList<string, string> transmap = LoadSortedListFromIni(TransFfilePath);


            //trans key
            List<SortedList> list_rzt_fmt = new List<SortedList>();
            foreach (SortedList sortedList in lst458)
            {
                SortedList map3 = new SortedList();
                // 循环遍历每一个键
                foreach (DictionaryEntry kvp in sortedList)
                {
                    var key = kvp.Key;
                    if (key.ToString() == "Searchs")
                        Print("dbg2441");
                    //add all cn key
                    var Cnkey = key;
                    var val = GetField(sortedList, key.ToString(), "");
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
                list_rzt_fmt.Add(map3);
            }

            return list_rzt_fmt;
        }

        public static HashSet<string> RemoveElementsContainingNumbers(HashSet<string> hashSet)
        {
            // 使用 LINQ 和正则表达式过滤掉包含数字的元素
            return new HashSet<string>(hashSet.Where(word => !Regex.IsMatch(word, @"\d")));
        }

        public static SortedList<string, string> MergeSortedLists(SortedList<string, string> list1, SortedList<string, string> list2)
        {
            // 创建一个新的 SortedList 来存储合并后的结果
            SortedList<string, string> mergedList = new SortedList<string, string>();

            // 添加第一个 SortedList 的所有元素
            foreach (var kvp in list1)
            {
                mergedList[kvp.Key] = kvp.Value;
            }

            // 添加第二个 SortedList 的所有元素，如果键已存在，则覆盖其值
            foreach (var kvp in list2)
            {
                mergedList[kvp.Key] = kvp.Value;
            }

            return mergedList;
        }
        public static void CleanupSortedListValuesStartWzAlphbt(SortedList<string, string> sortedList)
        {
            // 创建一个列表，存储需要移除的键
            HashSet<string> keysToRemove = new HashSet<string>();

            // 遍历 SortedList，找出以字母开头的值
            foreach (var kvp in sortedList)
            {
                if (string.IsNullOrEmpty(kvp.Value))
                {
                    keysToRemove.Add(kvp.Key); return;

                }

                char c = kvp.Value[0];
                if (!string.IsNullOrEmpty(kvp.Value) && IsEnglishLetter(c))
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            // 移除找到的键
            foreach (var key in keysToRemove)
            {
                sortedList.Remove(key);
            }
        }
        public static List<string> ReadJsonFileToList(string filePath)
        {
            List<string> jsonStringList = new List<string>();

            try
            {
                // 读取 JSON 文件内容
                string jsonString = System.IO.File.ReadAllText(filePath);

                // 使用 Newtonsoft.Json 库将 JSON 字符串反序列化为 List<string>
                jsonStringList = JsonConvert.DeserializeObject<List<string>>(jsonString);
            }
            catch (FileNotFoundException)
            {
                ConsoleWriteLine($"File not found: {filePath}");
            }
            catch (Newtonsoft.Json.JsonException)
            {
                ConsoleWriteLine($"Invalid JSON format in file: {filePath}");
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"Error reading JSON file: {ex.Message}");
            }

            return jsonStringList;
        }

        public static string Join(string[] words, string v)
        {
            return string.Join(v, words);
        }

        /// <summary>
        /// 给每个逗号分隔的单词前面加上感叹号
        /// </summary>
        /// <param name="nowPks">逗号分隔的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string AddCharFrontToElmt(string e, string nowPks)
        {
            // 分割输入字符串
            string[] words = nowPks.Split(',');

            // 给每个单词前面加上感叹号
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = e + words[i];
            }

            // 使用逗号将单词连接起来
            return string.Join(",", words);
        }

        public static HashSet<string> FilterUrlsEndwithHtm(HashSet<string> urls)
        {
            return new HashSet<string>(urls.Where(url => url.EndsWith(".html", StringComparison.OrdinalIgnoreCase) || url.EndsWith(".htm", StringComparison.OrdinalIgnoreCase)));
        }


        public static void CleanupSortedListKeysLenLessthan3(SortedList<string, string> sortedList)
        {
            // 创建一个列表，存储需要移除的键
            List<string> keysToRemove = new List<string>();

            // 遍历 SortedList，找出长度小于 4 的键
            foreach (var key in sortedList.Keys)
            {
                if (key.Length < 3)
                {
                    keysToRemove.Add(key);
                }
            }

            // 移除找到的键
            foreach (var key in keysToRemove)
            {
                sortedList.Remove(key);
            }
        }

        public static SortedList ArrReverseSortedList(SortedList originalList)
        {
            SortedList reversedList = new SortedList();

            foreach (DictionaryEntry entry in originalList)
            {
                var value = entry.Value.ToString();
                string[] a = value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string w in a)
                {
                    reversedList.Add(w.Trim().ToUpper(), entry.Key);
                }

            }

            return reversedList;
        }
        public static List<SortedList> ArrFltr(List<SortedList> rows, Func<SortedList, bool> whereFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintTimestamp($" start fun  {__METHOD__}()");
            dbgCls.PrintCallFunArgs(__METHOD__, func_get_args("someRows"));
            List<SortedList> rows_rzt4srch = new List<SortedList>();
            foreach (SortedList row in rows)
            {
                try
                {
                    if (whereFun == null)
                        rows_rzt4srch.Add(row);
                    else if (whereFun(row))
                    {
                        rows_rzt4srch.Add(row);
                    }
                }
                catch (Exception e)
                {
                    Print(e);

                    logErr2024(e, "whereFun", "errlog", null);
                    //  return false;
                }


            }




            PrintRet(__METHOD__, rows_rzt4srch.Count);

            PrintTimestamp($" end fun  {__METHOD__}()");
            return rows_rzt4srch;

        }


        public static List<SortedList> ArrFltrV2(List<SortedList> list, Func<SortedList, bool> fn)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintTimestamp($" start fun  {__METHOD__}()");
            if (fn == null)
                return list;
            List<SortedList> list22 = new List<SortedList>();


            // 使用 Parallel.ForEach 进行并行处理
            Parallel.ForEach(list, rw =>
            {
                try
                {
                    if (fn(rw))
                        list22.Add(rw);
                }
                catch (Exception e)
                {
                    Print(e);

                    logErr2024(e, "whereFun", "errlog", null);
                    //  return false;
                }
            });

            //foreach (SortedList rw in list)
            //{

            //}
            PrintTimestamp($" endfun  {__METHOD__}()");
            return list22;
        }
        public static void transfmVal(SortedList list, Func<string, object> fun)
        {
            // 创建一个临时的 ArrayList 来存储键
            ArrayList keys = new ArrayList(list.Keys);

            // 遍历每个键并更新值
            foreach (string key in keys)
            {
                string value = (string)list[key];
                list.Remove(key);
                list.Add(key, fun(value));
            }
        }



        public static void RemoveWordsFromHashSet(HashSet<string> words, string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            string[] wordsToRemove = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in wordsToRemove)
            {
                if (word.Trim().Length > 0)
                    words.Remove(word);
            }
        }
        public static HashSet<string> MergeArrayWithHashSet(string s, HashSet<string> set)
        {
            string[] array = s.Split(" ");
            // 创建一个新的 HashSet<string>，包含 HashSet<string> 的所有元素
            HashSet<string> resultSet = new HashSet<string>(set);

            // 将字符串数组中的所有元素添加到新的 HashSet<string> 中
            foreach (string item in array)
            {
                resultSet.Add(item);
            }

            return resultSet;
        }


        public static object ArrSlice<t>(List<t> inputList, int startIdx, int length)
        {
            //  List<Dictionary
            // 确保 length 不超过列表的长度
            if (length > inputList.Count)
            {
                length = inputList.Count;
            }

            // 使用 GetRange 方法来截取列表的前 length 个元素
            var rtz = inputList.GetRange(startIdx, length);
            return rtz;
        }

        public static object ArrSlice(List<Dictionary<string, object>> inputList, int startIdx, int length)
        {
            //  List<Dictionary
            // 确保 length 不超过列表的长度
            if (length > inputList.Count)
            {
                length = inputList.Count;
            }

            // 使用 GetRange 方法来截取列表的前 length 个元素
            var rtz = inputList.GetRange(startIdx, length);
            return rtz;
        }
        public static object ArrSlice(ArrayList inputList, int startIdx, int length)
        {
            //  List<Dictionary
            // 确保 length 不超过列表的长度
            if (length > inputList.Count)
            {
                length = inputList.Count;
            }

            // 使用 GetRange 方法来截取列表的前 length 个元素
            ArrayList rtz = inputList.GetRange(startIdx, length);
            return rtz;
        }

        public static List<SortedList> ArrSlice(List<SortedList> inputList, int startIdx, int length)
        {
            // 确保 length 不超过列表的长度
            if (length > inputList.Count)
            {
                length = inputList.Count;
            }

            // 使用 GetRange 方法来截取列表的前 length 个元素
            List<SortedList> rtz = inputList.GetRange(startIdx, length);
            return rtz;
            //  return arr_rzt;
        }

        /**
         * // 使用 Filter 函数筛选出 age 大于 23 的元素
        var filteredList = Filter(list, sl => (int)sl["age"] > 23).ToList();
         * 
         */
        public static IEnumerable<SortedList> arrar_filter(IEnumerable<SortedList> source, Func<SortedList, bool> predicate)
        {
            // 使用 LINQ 的 Where 方法筛选元素
            return source.Where(predicate);
        }

        static List<SortedList> sort(List<SortedList> list, string key)
        {
            // 使用 List 的 Sort 方法进行排序
            list.Sort((x, y) => Comparer.Default.Compare(x[key], y[key]));
            return list;
        }

        static IEnumerable<SortedList> array_map(IEnumerable<SortedList> source, Func<SortedList, SortedList> mapFunction)
        {
            // 使用 LINQ 的 Select 方法对每个元素应用 mapFunction
            return source.Select(mapFunction);
        }


        internal static void map_add(SortedList map, string idClmName, object item)
        {

            //   SortedList itemx = (SortedList)item;
            try
            {

                map.Add(idClmName, item);
            }
            catch (Exception e)
            {
                Print(e.Message);

            }
        }


        //internal static ArrayList dedulip(ArrayList List1, string idClmName)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}

        //internal static ArrayList dedulipV2(ArrayList List1, string idClmName)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}
        //internal static ArrayList dedulip(ArrayList List1, string idClmName)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}

        //internal static ArrayList dedulipV2(ArrayList List1, string idClmName)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}
        //public static ArrayList dedulip(ArrayList List1)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}
        public static HashSet<string> ArrMerge(HashSet<string> list1, HashSet<string> list2)
        {
            return MergeHashSets(list1, list2);
        }

        public static string[] RemoveShortWords(string[] words)
        {
            List<string> result = new List<string>();
            foreach (string word in words)
            {
                if (word.Length >= 2)
                {
                    result.Add(word);
                }
            }
            return result.ToArray();
        }


        public static List<t> array_merge<t>(List<t> list1, List<t> list2)
        {
            List<t> result = new List<t>();

            // 获取最长列表的长度
            int maxLength = Math.Max(list1.Count, list2.Count);

            // 遍历并合并列表
            //for (int i = 0; i < maxLength; i++)
            //{
            for (int i = 0; i < list1.Count; i++)
            {
                result.Add(list1[i]);
            }

            for (int i = 0; i < list2.Count; i++)
            {
                result.Add(list2[i]);
            }
            //}

            return result;
        }

        //public static List<t> MergeLists<t>(List<t> list1, List<t> list2)
        //{
        //    List<t> result = new List<t>();

        //    // 获取最长列表的长度
        //    int maxLength = Math.Max(list1.Count, list2.Count);

        //    // 遍历并合并列表
        //    //for (int i = 0; i < maxLength; i++)
        //    //{
        //    for (int i = 0; i < list1.Count; i++)
        //    {
        //        result.Add(list1[i]);
        //    }

        //    for (int i = 0; i < list2.Count; i++)
        //    {
        //        result.Add(list2[i]);
        //    }
        //    //}

        //    return result;
        //}


        //private static void findd()
        //{
        //    long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        //    var users_txt = System.IO.File.ReadAllText("db.json");

        //    showSpanTime(timestamp, "readFile");

        //    ////JsonSerializerSettings settings = new JsonSerializerSettings();
        //    ////settings.DefaultValueHandling = DefaultValueHandling.Ignore;
        //    JArray rows = JsonConvert.DeserializeObject<JArray>(users_txt);
        //    showSpanTime(timestamp, "delzobj");
        //    var results = (from jo in rows
        //                   where jo.Value<int>("key") == 10
        //                   select jo).ToList();


        //   print(JsonConvert.SerializeObject(results));


        //    string showtitle = "spatime(ms):";
        //    showSpanTime(timestamp, showtitle);

        //}



        //internal static List<InlineKeyboardButton[]> dedulip4inlnKbdBtnArr(List<InlineKeyboardButton[]> List1, string idClmName)
        //{
        //    //  ArrayList list = new ArrayList(List1);
        //    SortedList listIot = db.lst2IOT4inlKbdBtnArr(List1, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    List<InlineKeyboardButton[]> saveList_hpmod = db.lstFrmIot4inlnKbdBtn(listIot);
        //    return saveList_hpmod;
        //}

        internal static void Increment(SortedList<string, int> ordMap, string? fld)
        {
            if (!ordMap.ContainsKey(fld))
                ordMap.Add(fld, 1);
            else
            {
                ordMap[fld] = ordMap[fld] + 1;
            }
        }


        public static void CopyPropSortedListToMerchant(SortedList sortedList, Merchant merchant)
        {
            Type merchantType = typeof(Merchant);
            foreach (DictionaryEntry entry in sortedList)
            {
                try
                {


                    string key = entry.Key.ToString();
                    PropertyInfo property = merchantType.GetProperty(key);
                    if (property != null && property.CanWrite)
                    {
                        try
                        {
                            property.SetValue(merchant, Convert.ChangeType(entry.Value, property.PropertyType));
                        }
                        catch (Exception e)
                        {
                            PrintCatchEx(nameof(CopyPropSortedListToMerchant), e);
                        }

                    }
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(CopyPropSortedListToMerchant), e);
                }
            }
        }



        /// <summary>
        /// 计算集合的长度。
        /// </summary>
        /// <param name="collection">集合对象。</param>
        /// <returns>集合的长度。</returns>
        public static int CountLen(object collection)
        {
            if (collection == null)
            {
                return 0;
            }

            if (collection is ICollection col)
            {
                return col.Count;
            }

            if (collection is IEnumerable enumerable)
            {
                int count = 0;
                foreach (var item in enumerable)
                {
                    count++;
                }
                return count;
            }

            throw new ArgumentException("Unsupported collection type", nameof(collection));
        }
        public static SortedList CopyToOldSortedList(SortedList newList, SortedList oldList)
        {
            // 创建一个新的 SortedList
            // SortedList newList = new SortedList();

            // 遍历旧的 SortedList 并将每个键值对复制到新的 SortedList
            foreach (DictionaryEntry newx in newList)
            {
                if (newx.Key != null)
                    SetFieldAddRplsKeyV(oldList, newx.Key.ToString(), newx.Value);
                //   newList.Add(entry.Key, entry.Value);
            }

            return newList;
        }
        //internal static void Stfld_addRplsKeyV(SortedList SortedList1_iot, string key, SortedList objSave)
        //{
        //    if (SortedList1_iot.ContainsKey(key))
        //    {
        //        SortedList1_iot.Remove(key.ToString());
        //    }
        //    SortedList1_iot.Add(key, objSave);
        //}



        public static Dictionary<string, StringValues> CopySortedListToDictionary(SortedList sortedList)
        {
            Dictionary<string, StringValues> dictionary = new Dictionary<string, StringValues>();

            foreach (DictionaryEntry entry in sortedList)
            {
                string key = entry.Key as string;
                string value = entry.Value as string;

                if (key != null && value != null)
                {
                    dictionary[key] = new StringValues(value);
                }
            }

            return dictionary;
        }



        public static HashSet<string> ArrRemove(HashSet<string> hashSet2, string v)
        {
            string[] a = v.Split(" ");
            foreach (string wd in a)
            {
                hashSet2.Remove(wd);
            }

            return hashSet2;
        }

        public static HashSet<string> MergeHashSets(HashSet<string> set1, HashSet<string> set2)
        {
            HashSet<string> resultSet = new HashSet<string>(set1);
            resultSet.UnionWith(set2);
            return resultSet;
        }

        internal static HashSet<string> AddElmts2hashset(HashSet<string> set, string txtWds)
        {
            string[] a = txtWds.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string wd1 in a)
            {
                var wd = wd1.Trim().ToUpper();
                if (wd.Length > 0)
                    set.Add(wd);
            }
            return set;
        }

        //internal static int getRowVal(object s1, string v1, int v2)
        //{
        //    throw new NotImplementedException();
        //}        //internal static int getRowVal(object s1, string v1, int v2)
        //{
        //    throw new NotImplementedException();
        //}
    }
}