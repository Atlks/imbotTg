global using static prjx.libx.dbgCls;
using Newtonsoft.Json;
using System;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//using static prjx.biz_other;
using static prjx.clrCls;
using static prjx.libx.exCls;
using static prjx.libx.arrCls;//  prj202405.lib
using static prjx.libx.dbgCls;
using static prjx.libx.logCls;
using static prjx.libx.corex;
using static prjx.libx.db;
using static prjx.libx.filex;
using static prjx.libx.ormJSonFL;
using static prjx.libx.strCls;
using static prjx.libx.bscEncdCls;
 
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;
//using Microsoft.AspNetCore.Http.HttpResults;
using Telegram.Bot.Types.Enums;
using System.Text.Json;
namespace prjx.libx
{
    internal class dbgCls
    {

        public static string geneReqid()
        {
            // 获取主线程 ID
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("dd_HH");
            return timestamp + (Thread.CurrentThread.ManagedThreadId);
        }

        public static string ToString(object managedThreadId)
        {
            return managedThreadId.ToString();
        }

        /// <summary>
        /// 打印对象的类型和值，类似于 PHP 的 var_dump 函数。
        /// </summary>
        /// <param name="obj">要打印的对象</param>
        /// <param name="indentLevel">缩进级别，用于递归调用时控制缩进</param>
        public static void var_dump(object obj, int indentLevel = 0)
        {
            string indent = new string(' ', indentLevel * 4);

            if (obj == null)
            {
               Print($"{indent}null");
            }
            else
            {
                Type type = obj.GetType();
               Print($"{indent}{type}({GetObjectSize(obj)}) {obj}");
                indentLevel++;

                if (obj is IDictionary dictionary)
                {
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        Console.Write($"{indent}  [{entry.Key}] => ");
                        var_dump(entry.Value, indentLevel + 1);
                    }
                }
                else if (obj is IEnumerable enumerable && !(obj is string))
                {
                    foreach (var item in enumerable)
                    {
                        var_dump(item, indentLevel);
                    }
                }
                else
                {
                    if (!type.IsPrimitive && !(obj is string))
                    {
                        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            object value = property.GetValue(obj, null);
                           Print($"{indent}{property.Name}:");
                            var_dump(value, indentLevel + 1);
                        }

                        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                        {
                            object value = field.GetValue(obj);
                           Print($"{indent}{field.Name}:");
                            var_dump(value, indentLevel + 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取对象的近似大小。
        /// </summary>
        /// <param name="obj">要计算大小的对象</param>
        ///// <returns>对象的近似大小</returns>
        //private static long GetObjectSize(object obj)
        //{
        //    if (obj == null)
        //    {
        //        return 0;
        //    }

        //    using (var ms = new System.IO.MemoryStream())
        //    {
        //        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //        formatter.Serialize(ms, obj);
        //        return ms.Length;
        //    }
        //}


        /// <summary>
        /// 估算对象的近似大小。
        /// </summary>
        /// <param name="obj">要计算大小的对象</param>
        /// <returns>对象的近似大小（字节）</returns>
        private static long GetObjectSize(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            Type type = obj.GetType();
            if (type.IsPrimitive || obj is string)
            {
                return GetPrimitiveSize(obj);
            }

            long size = 0;
            if (obj is IDictionary dictionary)
            {
                foreach (DictionaryEntry entry in dictionary)
                {
                    size += GetObjectSize(entry.Key);
                    size += GetObjectSize(entry.Value);
                }
            }
            else if (obj is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    size += GetObjectSize(item);
                }
            }
            else
            {
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    size += GetObjectSize(property.GetValue(obj, null));
                }

                foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    size += GetObjectSize(field.GetValue(obj));
                }
            }

            return size;
        }

        /// <summary>
        /// 获取基本类型和字符串的大小。
        /// </summary>
        /// <param name="obj">基本类型或字符串对象</param>
        /// <returns>对象的大小（字节）</returns>
        private static long GetPrimitiveSize(object obj)
        {
            if (obj is bool)
                return sizeof(bool);
            if (obj is byte)
                return sizeof(byte);
            if (obj is sbyte)
                return sizeof(sbyte);
            if (obj is char)
                return sizeof(char);
            if (obj is decimal)
                return sizeof(decimal);
            if (obj is double)
                return sizeof(double);
            if (obj is float)
                return sizeof(float);
            if (obj is int)
                return sizeof(int);
            if (obj is uint)
                return sizeof(uint);
            if (obj is long)
                return sizeof(long);
            if (obj is ulong)
                return sizeof(ulong);
            if (obj is short)
                return sizeof(short);
            if (obj is ushort)
                return sizeof(ushort);
            if (obj is string str)
                return sizeof(char) * str.Length;

            return 0;
        }


        /// <summary>
        /// 打印对象的结构和内容，类似于 PHP 的 print_r 函数。
        /// </summary>
        /// <param name="obj">要打印的对象</param>
        /// <param name="indentLevel">缩进级别，用于递归调用时控制缩进</param>
        public static void print_r(object obj, int indentLevel = 0)
        {
            try
            {
                string indent = new string(' ', indentLevel * 4);

                if (obj == null)
                {
                   Print($"{indent}null");
                }
                else if (obj is IDictionary dictionary)
                {
                   Print($"{indent}Dictionary:");
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        Console.Write($"{indent}  [{entry.Key}] => ");
                        print_r(entry.Value, indentLevel + 1);
                    }
                }
                else if (obj is IEnumerable enumerable && !(obj is string))
                {
                   Print($"{indent}List:");
                    foreach (var item in enumerable)
                    {
                        print_r(item, indentLevel + 1);
                    }
                }
                else
                {
                   Print($"{indent}{obj}");
                }
            }
            catch (Exception e)
            {
               Print(e);
            }

        }

        /**
         * 
         * 
         * 在 C# 中，我们可以编写一个类似于 PHP 中的 error_reporting 函数来控制错误报告级别。
         * 尽管 C# 中没有与 PHP 中的错误报告级别完全相同的概念，
         * 但我们可以利用 C# 的异常处理机制来实现类似的功能。下面是一个示例：
         * 
         */
        /// <summary>
        /// 设置错误报告级别。0 表示禁用错误报告，其他值表示启用。
        /// </summary>
        /// <param name="level">错误报告级别</param>
        public static void error_reporting(int level)
        {
            if (level == 0)
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    // 禁用错误报告时，什么也不做
                };
            }
            else
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    Exception e = (Exception)args.ExceptionObject;
                   Print($"An error occurred: {e.Message}");
                };
            }
        }

        public static object[] newEmptyObjectArray()
        {
            return new object[0];
        }

        //todo should thrd rlt thrdlocal
        public static int dbgpad = 0;
        public static object[] FilterNonSerializableObjects(object[] inputArray)
        {
            List<object> filteredList = new List<object>();

            foreach (var obj in inputArray)
            {
                // 过滤掉 HttpRequest 和 HttpResponse 类型的对象
                if (!(obj is HttpRequest) && !(obj is HttpResponse))
                {
                    filteredList.Add(obj);
                }
            }

            return filteredList.ToArray();
        }

        /// <summary>
        /// 性能有限 only smple mode
        /// </summary>
        /// <param name="METHOD__"></param>
        /// <param name="func_get_args"></param>
        public static void PrintCallFunArgsFast(string METHOD__, object func_get_args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            PrintTimestamp("PrintCallFunArgsFast");

           
            dbgpad = dbgpad + 4;
            var msglog = "";
            try
            {
                msglog = RepeatMnyChr(dbgpad) + "### FUN " + METHOD__ + "((" + encodeJsonFast(func_get_args) + "))";
                // array_push($GLOBALS['dbg'],$logmsg   );
            }
            catch (Exception e)
            {
                msglog = str_repeat(" ", dbgpad) + " FUN " + METHOD__ + "( )";
                logErr2025(e, "print_ret", "errdirSysMeth");
            }
            //  print("\n\n\n" + msglog + "");
            PrintColoredText("\n\n\n" + msglog + "", ConsoleColor.Green);
            PrintTimestamp(" end PrintCallFunArgsFast");
        }

        /// <summary>
        /// smpe le obj ..no trans to 
        /// System.Text.Json：是 .NET Core 3.0 及以后的版本中引入的 JSON 序列化库，相比于 Newtonsoft.Json（Json.NET），它通常具有更高的性能。
        /// </summary>
        /// <param name="func_get_args"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string encodeJsonFast(object results)
        {
            PrintTimestamp("encodeJsonFast");
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //   Formatting = Formatting.Indented
            };
            //  string json = JsonConvert.SerializeObject(obj, settings);
            string jsonString = JsonConvert.SerializeObject(results, settings);
            // 使用 System.Text.Json 进行序列化 更很快

            
            var options = new JsonSerializerOptions
            {
                //todo 还是报错
                // 适用于 .NET 8 的新配置方式
                TypeInfoResolver = new JsonSerializerOptions().TypeInfoResolver
            };
         //   string jsonString = System.Text.Json.JsonSerializer.Serialize(results, options);
            PrintTimestamp(" end encodeJsonFast");
            return jsonString;
        }

        /* //if($GLOBALS['dbg_show']==false)
            //    return;
            //  $GLOBALS['dbgpad']=$GLOBALS['dbgpad']+4;
         * @param string $METHOD__
         * @return void
         */
        public static void PrintCallFunArgs(string METHOD__, object func_get_args)
        {
            Console.OutputEncoding = Encoding.UTF8;
           PrintTimestamp();

            // 判断 func_get_args 是否为 object[] 数组
            //if (func_get_args is object[] argsArray)
            //{
            //    // 获取第一个元素
            //    var firstArg = argsArray.Length > 0 ? argsArray[0] : null;

            //    // 判断第一个元素是否为 HttpRequest 类型
            //    if (firstArg is HttpRequest)
            //    {
            //        dbgArgs = newEmptyObjectArray();
            //        // 打印 "ok"
            //        Console.WriteLine("ok");
            //    }
            //}
            dbgpad = dbgpad + 4;
            var msglog = "";
            try
            {
                msglog = RepeatMnyChr(  dbgpad) + "### FUN " + METHOD__ + "((" + json_encode_noFmt(func_get_args) + "))";
                // array_push($GLOBALS['dbg'],$logmsg   );
            }
            catch ( Exception e)
            {
                msglog = str_repeat(" ", dbgpad) + " FUN " + METHOD__ + "( )";
                logErr2025(e, "print_ret", "errdirSysMeth");
            }
         //  print("\n\n\n" + msglog + "");
            PrintColoredText("\n\n\n" + msglog + "", ConsoleColor.Green);
        }

        public static void print_varDump(string METHOD__, string vname, object val)
        {
            //if($GLOBALS['dbg_show']==false)
            //    return;
            var msglog = str_repeat(" ", dbgpad + 3) + "" + METHOD__ + $"():: {vname}=>{json_encode_noFmt( val)}";
            // array_push($GLOBALS['dbg'],        $msg);
           Print(msglog + "");

        }
        public static void print_varDump(string METHOD__, string vname, string val)
        {
            //if($GLOBALS['dbg_show']==false)
            //    return;
            var msglog = str_repeat(" ", dbgpad + 3) + "" + METHOD__ + $"():: {vname}=>{val}";
            // array_push($GLOBALS['dbg'],        $msg);
           Print(msglog + "");

        }
        public static void PrintRetx(string mETHOD__, object? result)
        {
            if ("WbapiXgetlist".Equals(mETHOD__))
            {
                Print(".dbg.");
            }

            if (IsStr(result))
            {
                PrintRet(mETHOD__, Left(result, 200));
                return;
            }
            //try
            //{

            //}cat
            if (result is System.Collections.IList)
            {
                IList lst = (IList)result;
                try
                {

                    Print("lst.size=>" + lst.Count);
                    if (lst.Count > 0)
                        PrintRet(mETHOD__, lst[0]);
                    else
                        PrintRet(mETHOD__, "list.size=0");
                }
                catch (Exception e)
                {
                    PrintExcept("print_ret_ex", e);
                    PrintRet(mETHOD__, "lst.size=>" + lst.Count);
                }

            }
            else
                PrintRet(mETHOD__, result);
        }

        public static void PrintRet(object mETHOD__, object results)
        {
          //  PrintRetx
            try
            {
                if (dbgpad == 0)
                    dbgpad = 1;
                var msglog = str_repeat("💰", dbgpad) + " ENDFUN " + mETHOD__ + "():: ret=>" + json_encode_noFmt(results);
              // print(msglog + "");
                PrintColoredText(msglog, ConsoleColor.DarkGreen);
            }
            catch (Exception e)
            {
                var msglog = str_repeat(" ", dbgpad) + " ENDFUN " + mETHOD__ + "():: ret=>";
               Print(msglog + "");
                logErr2025(e, "print_ret", "errdirSysMeth");
            }

            //    array_push($GLOBALS['dbg'], $msglog);
            dbgpad = dbgpad - 4;
        }

        //public static object array_slice(object arr_rzt, int v1, int v2)
        //{
        //    return arr_rzt;
        //}


        /// <summary>
        /// 类似于 PHP 中的 func_get_args 函数，输出传递给函数的所有参数。
        /// </summary>
        /// <param name="args">参数数组</param>
        public static object func_get_args(params object[] args)
        {
            // 输出每个参数的值
            //print("Arguments:");
            //foreach (object arg in args)
            //{
            //   print(arg);
            //}
            //  return paramValues;
            return args;
        }

        public static object func_get_args4async(params object[] paramValues)
        {
            // 获取当前方法
            // MethodBase method = new StackFrame(1).GetMethod();


            return paramValues;
            // 序列化为 JSON 字符串
            //  return JsonConvert.SerializeObject(parameterValues, Formatting.Indented);
        }

        /**
         * 
         * 在C#中，由于其类型系统的不同，无法直接实现类似于PHP的func_get_args函数，因为C#的方法参数必须在编译时指定类型。
         * 但是，我们可以使用可变参数（params）来实现一个类似的功能。以
         * 
         */
        public static object func_get_args(MethodBase method, params object[] paramValues)
        {
            return paramValues;
            // 获取当前方法
            // MethodBase method = new StackFrame(1).GetMethod();

            // 序列化为 JSON 字符串
            //  return JsonConvert.SerializeObject(parameterValues, Formatting.Indented);
        }

        public static object func_get_argsDetao(MethodBase method, params object[] paramValues)
        {
            return paramValues;
            // 获取当前方法
            // MethodBase method = new StackFrame(1).GetMethod();

            // 获取当前方法的参数
            ParameterInfo[] parameters = method.GetParameters();


            // 检查参数数量是否匹配
            if (parameters.Length != paramValues.Length)
            {
                //  throw new ArgumentException("Parameter count does not match.");
            }

            // 获取当前方法的参数值
            // 将参数名称和值配对
            var parameterValues = parameters.Select<ParameterInfo, object>(
                (ParameterInfo p, int index) =>
                {
                    try
                    {
                        return new
                        {
                            p = p.Name,
                            v = paramValues[index]
                        };
                    }
                    catch (Exception e)
                    {
                        Hashtable hash = new Hashtable();
                        hash.Add("func_get_args.p1.method_Name", method.Name);
                        hash.Add("func_get_args.p2.prm", paramValues);

                        logErr2024(e, "func_get_args", "errlogDir2024", hash);
                        print_r(e);
                        return new
                        {
                            p = "pxx",
                            v = "vxxx"
                        };
                    }

                }

            ).ToList();
            return parameterValues;
            // 序列化为 JSON 字符串
            //  return JsonConvert.SerializeObject(parameterValues, Formatting.Indented);
        }

        static string GetCurrentMethodParametersJson()
        {
            // 获取当前方法
            MethodBase method = new StackFrame(1).GetMethod();

            // 获取当前方法的参数
            ParameterInfo[] parameters = method.GetParameters();

            // 获取当前方法的参数值
            var parameterValues = parameters.Select(p =>
            {
                // 使用反射获取参数值
                object value = p.RawDefaultValue; // 默认值
                if (value == DBNull.Value) value = null; // 如果是数据库的 NULL 值

                return new { p.Name, Value = value };
            }).ToList();

            // 序列化为 JSON 字符串
            return JsonConvert.SerializeObject(parameterValues, Formatting.Indented);
        }
        public static string str_repeatV2(string str, int count)
        {
            if (count < 0)
                count = 0;
            char[] charArray = str.ToCharArray();
            return new string(charArray[0], count);
        }
        public static string str_repeat(string emoji, int count)
        {
            //if (count < 0)
            //    count = 0;
            //return new string('*', count);
            return GenerateEmojis(count, emoji);
        }

        //for afaast perf 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string RepeatMnyChr(int count)
        {
            if (count < 0)
                count = 0;

            if (count <= 0)
            {
                return string.Empty;
            }

            // 使用StringBuilder提高性能
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append("💸");
            }
            return sb.ToString();
         //   return GenerateEmojis(count, "💸");
             //   new string('', count);
        }

        public static string GenerateEmojis(int count, string emoji)
        {
            if (count <= 0)
            {
                return string.Empty;
            }

            // 使用StringBuilder提高性能
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(emoji);
            }
            return sb.ToString();
        }

      
    }
}
//$GLOBALS['dbg']=[];

//$GLOBALS['dbg_show']=true;





