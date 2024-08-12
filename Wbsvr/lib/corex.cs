global using static prjx.libx.corex;
using prjx.libx;
//using Microsoft.CodeAnalysis.CSharp.Scripting;
//using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Win32;
//using NAudio.Wave;
//using Nethereum.Model;


//using Mono.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using static SqlParser.Ast.DataType;

namespace prjx.libx
{


    //prj202405.lib.corex
    internal class corex
    {
        public static object call2025(string methodName, params object[] args)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> typeList = assemblies
                            .SelectMany(assembly => assembly.GetTypes());
            IEnumerable<MethodInfo> methodss = typeList
                            .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public));
            var methodInfo = methodss
                .FirstOrDefault(method =>
                    method.Name == methodName
                  );

            if (methodInfo == null) return null;

            var delegateType = typeof(Func<string, List<SortedList>>);
            //  var delegateMethod = methodInfo.CreateDelegate(delegateType);

            // 假设你想要执行 YourMethodName 方法
            //   object[] args = { };
            var result = methodInfo.Invoke(null, args);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }

        public static object GetFunc(string methodName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> typeList = assemblies
                            .SelectMany(assembly => assembly.GetTypes());
            IEnumerable<MethodInfo> methodss = typeList
                            .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public));
            var methodInfo = methodss
                .FirstOrDefault(method =>
                    method.Name == methodName
                  );

            if (methodInfo == null) return null;

            var delegateType = typeof(Func<string, List<SortedList>>);
            //  var delegateMethod = methodInfo.CreateDelegate(delegateType);

            // 假设你想要执行 YourMethodName 方法
            object[] args = { };
            var result = methodInfo.Invoke(null, args);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }

        static string GetFuncName(Delegate del)
        {
            // 获取委托的类型
            Type type = del.GetType();

            // 检查类型是否为 Func<...>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Func<,>).GetGenericTypeDefinition())
            {
                // 获取目标方法的信息
                var methodInfo = del.Method;

                // 返回方法的名称
                return methodInfo.Name;
            }

            return "Not a Func type";
        }

        public static void foreach_hashtable2025(Hashtable chtsSess, Func<DictionaryEntry, object> fun)
        {
            foreach (DictionaryEntry de in chtsSess)
            {
                //if (Convert.ToInt64(de.Key) == Program.groupId)
                //    continue;
                //  var chatid = Convert.ToInt64(de.Key);
                try
                {
                    //  if(chatid== -1002206103554)
                    fun(de);
                }
                catch (Exception e)
                {
                    PrintCatchEx("foreach_hashtable", e);
                    //  print(e);
                }
            }
        }

        public static void tts(string txt)
        {
            if (txt == null)
                return;
            var pty = "D:\\PycharmProjects\\pythonProject\\.venv\\Scripts\\python.exe";
            SortedList prm = new SortedList();
            prm.Add("txt", txt);
         string mp3=   call_exe_retStr(pty, "D:\\0prj\\mdsj\\mdsjprj\\libBiz\\ttsScrpt.py", prm);
            print_varDump("tts", "mp3", mp3);
          //  playMp3V2(mp3);
        }
    
        public static string prjdir = @"../../../";

        public static void ExecuteAfterDelay(int millisecondsDelay, Action action)
        {
            System.Threading.Tasks.Task.Delay(millisecondsDelay).ContinueWith(_ =>
            {


                CallUserFunc409(action, []);
            }


                );
        }

        //public static object 运行(string 代码)
        //{
        //    try
        //    {
        //        var 结果 = CSharpScript.EvaluateAsync(代码, ScriptOptions.Default).Result;
        //        return 结果;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        public static SortedList parse_str_qrystr(string urlqryStr)
        {

            // 解析查询字符串为字典
            NameValueCollection queryString = HttpUtility.ParseQueryString(urlqryStr);
            var QueryHashtb = new System.Collections.Generic.Dictionary<string, string>();

            // 将解析结果存入字典
            foreach (string key in queryString.AllKeys)
            {
                QueryHashtb.Add(key, queryString[key]);
            }



            // 创建一个 SortedList 并初始化大小
            SortedList sortedList = new SortedList(QueryHashtb.Count);

            // 将 Dictionary 中的项添加到 SortedList 中
            foreach (var pair in QueryHashtb)
            {
                sortedList.Add(pair.Key, pair.Value.ToString());
            }

            return sortedList;
        }
      

        public static string soluPath = "";
        public static string execpath = "";

        //D:\0prj\mdsj\WindowsFormsApp1\sqltnode\qry.js
        public static string call_exe_Pstr(String exec, string scriptPath, string arguments)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), exec, scriptPath, arguments));

            // Create a new process to run the Node.js script
            Process process = new Process();
         //   process.StartInfo.
            process.StartInfo.FileName = exec;
       // D:\0prj\mdsj\mdsjprj >

        process.StartInfo.Arguments = $"\"{scriptPath}\" \"{arguments}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8; // 设置标准输出编码

            process.StartInfo.StandardErrorEncoding = Encoding.UTF8;  // 设置标准错误输出编码
            // Capture the output from the process
            string output;
            string errorOutput = "";

            try
            {
                process.Start();

                // Read the standard output and error output streams
                using (StreamReader outputReader = process.StandardOutput)
                {
                    output = outputReader.ReadToEnd();
                }
                using (StreamReader errorReader = process.StandardError)
                {
                    errorOutput = errorReader.ReadToEnd();
                }

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                output = $"An error occurred while executing the Node.js script: {ex.Message}";
            }

            // If there is any error output, append it to the main output
            if (!string.IsNullOrEmpty(errorOutput))
            {
                output += Environment.NewLine + "Error output: " + errorOutput;
            }
            dbgCls.PrintRet(__METHOD__, output);
            return output;
        }


        public static SortedList DictionaryToSortedList<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            // 创建一个 SortedList
            SortedList sortedList = new SortedList();

            // 将 Dictionary 中的键值对添加到 SortedList 中
            foreach (KeyValuePair<TKey, TValue> pair in dictionary.OrderBy(p => p.Key))
            {
                sortedList.Add(pair.Key.ToString(), pair.Value);
            }

            return sortedList;
        }
        public static SortedList ObjectToSortedList(object obj)
        {
            SortedList sortedList = new SortedList();

            // 获取对象的所有属性
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                // 获取属性名和值
                string propertyName = property.Name;
                object propertyValue = property.GetValue(obj);

                // 将属性名和值添加到SortedList
                sortedList.Add(propertyName, propertyValue);
            }

            return sortedList;
        }
    }
}
