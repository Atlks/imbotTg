global using static prjx.libx.bytecdExt;
using libx;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prjx.libx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace prjx.libx
{
    internal class bytecdExt
    {
    

   

        public static HashSet<string> RemoveWordsStartingWithDigit(HashSet<string> words)
        {
            // 使用 LINQ 和正则表达式过滤掉数字开头的单词
            return new HashSet<string>(words.Where(word => !Regex.IsMatch(word, @"^\d")));
        }
        public static HashSet<string> RemoveElementsWithShortLength(HashSet<string> hashSet)
        {
            // 使用 LINQ 过滤掉长度小于 3 的元素
            return new HashSet<string>(hashSet.Where(word => word.Length >= 3));
        }
    


        public static string JoinStringsWithNewlines(List<string> stringList)
        {

            if (stringList == null || stringList.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach (string item in stringList)
            {
                builder.Append(item);
                builder.AppendLine();
            }

            // Remove the last newline character
            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

       
        
        public static void foreach_listStr(List<string> li, Action<string> value)
        {
            foreach (string line in li)
            {
                value(line);
            }
        }
        public static void ConsoleWriteLine(string v)
        {
            System.Console.WriteLine(v);
        }


     
        public static void WaitTaskExecFinish(System.Threading.Tasks.Task 结果task)
        {
            结果task.Wait();
        }


        public static string SubStr(string queryString, int n)
        {
            // int n
            int len = queryString.Length;
            if (queryString == "")
                return "";
        //    queryString.Substring(2);
            return queryString.Substring(n);
        }

        public static void SetRespContentTypeNencode
            (HttpResponse http上下文, string 内容类型和编码)
        {
            http上下文.ContentType = 内容类型和编码;
        }
        public static void SendResp(object 输出结果, HttpResponse http上下文)
        {
            http上下文.WriteAsync(ToStr(输出结果), Encoding.UTF8).GetAwaiter().GetResult(); ;

        }
        public static void SendResp(object 输出结果, string 内容类型和编码,HttpResponse http上下文)
        {
            if (输出结果 == null)
                输出结果 = "";
            http上下文.ContentType = 内容类型和编码;
           
            http上下文.WriteAsync(输出结果.ToString(), Encoding.UTF8).GetAwaiter().GetResult(); ;

        }
    
        public static void SendRespsJJJresNotExist404(HttpResponse HTTP响应对象)
        {
            const string 提示 = "file not find文件没有找到";
            HTTP响应对象.StatusCode = (int)HttpStatusCode.NotFound;
            StreamWriter writer = new StreamWriter(HTTP响应对象.Body);
            writer.Write(提示);
        }
        public static string 格式化路径2(string 路径)
        {
            return CastNormalizePath(路径);

        }
  
      
        /*
         这些指令将参数加载到堆栈上，然后可以使用其他指令来处理参数，例如调用 GetType 方法获取其类型。

在实际编写IL代码时，可以结合使用 ldarg 指令加载参数，然后调用 call 指令来调用方法，最后使用 callvirt 指令来调用 GetType 方法。
         */
       //todo
        //        十 Adam 大鱼 刘洋 汤姆, [9/7/2024 下午 11:28]
        //"ToLower",
        //  "Replace",
        //  "Trim",

        //十 Adam 大鱼 刘洋 汤姆, [9/7/2024 下午 11:28]
        //        ToUpper

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:28]
        //ToSimple

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:29]
        //Join

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:29]
        //Remove

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:30]
        //TryGetValue

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:30]
        //ToList

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:30]
        //SerializeObject

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:31]
        //"Sort",
        //  "CompareTo",

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:31]
        //Exists

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:32]
        //Run

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:32]
        //log

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:32]
        //StartsWith

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:32]
        //endwith

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:33]
        //Match

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:33]
        //Parse

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:34]
        //Insert

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:34]
        //"AddDocument",

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:34]
        //GetFiles

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:34]
        //Execute

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:35]
        //ToTraditional

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:35]
        //ReadAsStringAsync

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:35]
        //"Value<JArray>",

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:36]
        //GetStringAsync

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:36]
        //Count

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:36]
        //AddDays

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:36]
        //Find

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:37]
        //ElementAt

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:37]
        //IsWindowVisible

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:38]
        //NewPageAsync

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:38]
        //newx

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:38]
        //Exit

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:39]
        //LoadHtml

        //十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:39]
        //SetValue
        //public static List<SortedList> foreach_Sqlt(string sqltFl, Func<SortedList, SortedList> fun)
        //{
        //    List<SortedList> li = new List<SortedList>();


        //    List<SortedList> liFrmFl = ormSqlt.qryV2(sqltFl);
        //    foreach (SortedList rw in liFrmFl)
        //    {
        //        li.Add(fun(rw));
        //    }

        //    ormSqlt.saveMltHiPfm(li, sqltFl);

        //    return li;
        //}

        /// <summary>
        ///  merge mode
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopySortedList(SortedList source, SortedList destination)
        {
            foreach (DictionaryEntry entry in source)
            {
                if (destination.ContainsKey(entry.Key))
                    destination[entry.Key] = entry.Value;
                else
                    destination.Add(entry.Key, entry.Value);

            }
        }

      
        
        public static List<object> foreach_hstbEs(List<SortedList> list2, Func<SortedList, object> act)
        {
            List<object> listRzt = new List<object>();
            foreach (SortedList rw in list2)
            {
                try
                {
                    listRzt.Add(act(rw));
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(foreach_hstbEs), e);
                }

            }
            return listRzt;
        }
        void loopForever()
        {
            while (true)
            {
                Print(DateTime.Now);
                Thread.Sleep(5000);
            }
        }

     


        public static int Count(object collection)
        {
            return CountLen(collection);
        }
        public static List<SortedList> ForList(string Blkname, List<SortedList> list, Action<SortedList> act)
        {
            // List<Hashtable> listRzt = new List<object>();
            foreach (SortedList rw in list)
            {
                try
                {
                    act(rw);
                }
                catch (Exception e)
                {
                    PrintCatchEx(Blkname, e);
                }

            }
            return list;
        }

        public static List<Hashtable> ForList(List<Hashtable> list, Action<Hashtable> act)
        {
            // List<Hashtable> listRzt = new List<object>();
            foreach (Hashtable rw in list)
            {
                try
                {
                    act(rw);
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(foreach_listHstb), e);
                }

            }
            return list;
        }

        public static List<Hashtable> foreach_listHstb(List<Hashtable> list, Action<Hashtable> act)
        {
            // List<Hashtable> listRzt = new List<object>();
            foreach (Hashtable rw in list)
            {
                try
                {
                    act(rw);
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(foreach_listHstb), e);
                }

            }
            return list;
        }

      
        public static string Join(List<string> list)
        {
            var separator = "";
            return string.Join(separator, list);
        }
        public static void Run136()
        {

        }
        public static void Exec()
        {

        }
        public static void file_rw()
        {

        }
        public static void str_join()
        {

        }


        public static void ForeachHashtableEs(List<SortedList> list2, Action<SortedList> act)
        {
            foreach (SortedList rw in list2)
            {
                try
                {
                    act(rw);
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(foreach_hstbEs), e);
                }

            }
        }
     

        public static string Substring(string queryString, int v)
        {
            if (queryString == "")
                return "";
            return queryString.Substring(v);
        }

       
   
        public static List<SortedList> SliceX<SortedList>(List<SortedList> list, int start, int length)
        {
            if (start < 0) start = 0;
            if (start >= list.Count) return new List<SortedList>();

            if (length < 0) length = 0;
            if (start + length > list.Count) length = list.Count - start;

            return list.GetRange(start, length);
        }
        public static void ForDicKeys(Dictionary<string, string> dictionary, Func<string,bool> keyAction)
        {
            foreach (var key in dictionary.Keys)
            {
             bool continueBool=   keyAction(key);
                if (continueBool == false) ;
                break;
            }
        }

        public static void foreach_DictionaryKeys(Dictionary<string, string> dictionary, Action<string> keyAction)
        {
            foreach (var key in dictionary.Keys)
            {
                keyAction(key);
            }
        }



        public static void PrintStr(object v)
        {
            System.Console.Write(v);
        }
        public static string MMM(object v)
        {
            return "MMM"+v;
        }
        public static void Print(object v)
        {
            System.Console.WriteLine(v);
        }

        /// <summary>
        /// overwrt mode,,,note append mode
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopySortedListCloneMode(SortedList source, SortedList destination)
        {
            // 清空目标 SortedList 以确保它是空的
            destination.Clear();

            // 复制数据
            foreach (DictionaryEntry entry in source)
            {
                destination.Add(entry.Key, entry.Value);
            }
        }

        public static void PrintLog(object v)
        {
            DateTime now = DateTime.Now;
            // Format with milliseconds
            string formattedDate = now.ToString("MMdd HHmmss.fff");
            System.Console.WriteLine($"{formattedDate} "+v);
        }
        public static void print(string format, object arg0)
        {
            System.Console.WriteLine(format, arg0);
        }

      
        public static int len(object obj)
        {
            if (IsString(obj))
                return obj.ToString().Length;
            if (obj is System.Collections.IList list)
            {
                return list.Count;
            }

            if (obj is SortedList sortedList)
            {
                return sortedList.Count;
            }
            return 0;

        }


    }
}
