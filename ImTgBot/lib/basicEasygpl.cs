global using static prjx.libx.basicEasygpl;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class basicEasygpl
    {

        /*

 .子程序 call, 公开, 通用型
.参数 callback, 句柄型
.参数 args, 数据组
 */
        public static object 调用(Delegate 函数, params object[] 参数组)
        {
            调用API(函数, 参数组);
         SortedList st=   新建哈希表();
            string s= 子文本截取("abc", 1);
           var __METHOD__ = 函数.Method.Name;
            object o = null;
            try
            {
                o = 函数.DynamicInvoke(参数组);
            }
            catch(jmp2endEx e2)
            {
                throw e2;
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("jmp2endEx"))
                {
                    Jmp2endDep();
                }
                PrintCatchEx(__METHOD__, e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + encodeJsonNofmt(func_get_args(参数组)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            return o;
        }
        public static void 等待异步任务执行完毕(System.Threading.Tasks.Task 结果task)
        {
            结果task.Wait();
        }
        public static void 设置响应内容类型和编码(HttpResponse http上下文, string 内容类型和编码)
        {
            http上下文.ContentType = 内容类型和编码;
        }
        public static void 设置响应内容类型和编码(HttpContext http上下文, string 内容类型和编码)
        {
            http上下文.Response.ContentType = 内容类型和编码;
        }
        public static void 发送响应(object 输出结果, HttpResponse http上下文)
        {
            http上下文.WriteAsync(输出结果.ToString(), Encoding.UTF8).GetAwaiter().GetResult(); ;

        }
        public static void 发送响应(object 输出结果, HttpContext http上下文)
        {
            http上下文.Response.WriteAsync(输出结果.ToString(), Encoding.UTF8).GetAwaiter().GetResult(); ;

        }
        public static bool 字符串结尾为真(string 路径, string 扩展名)
        {
            return 路径.ToUpper().Trim().EndsWith("." + 扩展名.Trim().ToUpper());
        }
        public static bool 路径包含扩展名结尾(string 路径, string 扩展名)
        {
            return 路径.ToUpper().Trim().EndsWith("." + 扩展名.Trim().ToUpper());
        }
        public static void 发送响应_资源不存在2(HttpResponse HTTP响应对象)
        {
            const string 提示 = "file not find文件没有找到";
            HTTP响应对象.StatusCode = (int)HttpStatusCode.NotFound;
            StreamWriter writer = new StreamWriter(HTTP响应对象.Body);
            writer.Write(提示);
        }
        public static void 发送响应_资源不存在(HttpResponse HTTP响应对象)
        {
            const string 提示 = "file not find文件没有找到";
            HTTP响应对象.StatusCode = (int)HttpStatusCode.NotFound;
            StreamWriter writer = new StreamWriter(HTTP响应对象.Body);
            writer.Write(提示);
        }
        public static string 格式化路径(string 路径)
        {
            return CastNormalizePath(路径);

        }
        public static bool 文件有扩展名(string 路径)
        {
            string 文件扩展名 = Path.GetExtension(路径);
          //  string 文件路径 = $"{web根目录}{路径}";
         //   文件路径 = 格式化路径(文件路径);
            if (文件扩展名 == "")
                return false;
            else
                return true;
        }
        public static bool 文件存在(string 文件路径)
        {
            return ExistFil(文件路径);
        }
        public static bool 文件不存在(string 文件路径)
        {
            return !ExistFil(文件路径);
        }
        public static string[] 拆分(object key)
        {
            return key.ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
        public static void 跳转到结束()
        {
            // jmp2exitFlag = true;
            throw new jmp2endEx();
        }
        public static void 遍历数组(string[] chtsSess, Action<string> fun)
        {
            foreach (string de in chtsSess)
            {
                //if (Convert.ToInt64(de.Key) == Program.groupId)
                //    continue;
                //  var chatid = Convert.ToInt64(de.Key);
                try
                {
                    //  if(chatid== -1002206103554)
                    fun(de);
                }
                catch (jmp2endEx e2)
                {
                    throw e2;
                }
                catch (Exception e)
                {
                    if (e.ToString().Contains("jmp2endEx"))
                    {
                        Jmp2endDep();
                    }
                    PrintCatchEx("foreach_hashtable", e);
                    //  print(e);
                }
            }
        }

        public static void 遍历哈希表(Hashtable chtsSess, Action<DictionaryEntry> fun)
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
                catch (jmp2endEx e2)
                {
                    throw e2;
                }
                catch (Exception e)
                {
                    if (e.ToString().Contains("jmp2endEx"))
                    {
                        Jmp2endDep();
                    }
                    PrintCatchEx("foreach_hashtable", e);
                    //  print(e);
                }
            }
        }

        public static string 读入文本(string f)
        {
            return System.IO.File.ReadAllText(f);
        }
        public static string 解码URL(string path)
        {
            string decodedUrl = WebUtility.UrlDecode(path);
            return decodedUrl;
        }
        public static string 子文本截取(string v1, int v2)
        {
            return Substring(v1, v2);
        }
        public static object 调用(string methodName, params object[] args)
        {

            var __METHOD__ = methodName;
            PrintCallFunArgs(methodName, func_get_args(args));

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Print("assemblies.Len=>" + assemblies.Length);
            IEnumerable<Type> typeList = assemblies
                            .SelectMany(assembly => assembly.GetTypes());
            Print("typeList.Len=>" + typeList.Count());
            IEnumerable<MethodInfo> methodss = typeList
                            .SelectMany(type => type.GetMethods());  //BindingFlags.Static| BindingFlags.Public
            Print("methodss.Len=>" + methodss.Count());
            var methodInfo = methodss
                .FirstOrDefault(method =>
                    method.Name == methodName
                  );

            if (methodInfo == null)
            {
                Print("......$$waring  .methodinfo is null");
                PrintRetx(__METHOD__, "");
                return null;
            }


            var delegateType = typeof(Func<string, List<SortedList>>);
            //  var delegateMethod = methodInfo.CreateDelegate(delegateType);

            // 假设你想要执行 YourMethodName 方法
            //   object[] args = { };
            object result = null;
            try
            {
                result = methodInfo.Invoke(null, args);

            }
            catch (Exception e)
            {
                PrintExcept(nameof(CallxTryx), e);
            }


            PrintRetx(__METHOD__, result);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }


        public static SortedList 新建哈希表()
        {
            return new SortedList  ();
        }
        public static Hashtable 新建哈希表hashtb()
        {
            return new Hashtable();
        }

        public static void 调用API(Delegate 函数, object[] 参数组)
        {
            throw new NotImplementedException();
        }

        public static void  调试输出(object obj)
        {
           Print(json文本(obj));
        }
        private static string 参数类型(object obj)
        {
           
            return obj.GetType().ToString();
        }
        private static bool 是否字符串(object obj)
        {
            if (取类型名(obj) == "string")
                return true;
            else
                return false;
        }

        private static string 取类型名(object obj)
        {
            return obj.GetType().ToString();
        }

        private static string json编码(object obj)
        {
            return json_encode_noFmt(obj);
        }
        private static string json解码(object obj)
        {
            return json_encode_noFmt(obj);
        }

        private static string json文本(object obj)
        {
            return json_encode_noFmt(obj);
        }


        private static string 到文本(object obj)
        {
            return obj.ToString();
        }
        private static int 取长度(string obj)
        {
            return obj.ToString().Length;
        }
        private static int 取文本长度(string obj)
        {
            return obj.ToString().Length;
        }

        private static string 取成员文本值(object obj)
        {
            return obj.ToString();
        }
        


    }
}
