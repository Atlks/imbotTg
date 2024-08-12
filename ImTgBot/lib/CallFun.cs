global using static prjx.libx.CallFun;

using prjx.libx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static prjx.libx.afrmwk;
using static prjx.libx.util;
//using static libx.storeEngr4Nodesqlt;
//using static prjx.timerCls;
//using static prjx.biz_other;
using static prjx.clrCls;
using static libx.qryEngrParser;
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
using static prjx.libx.net_http;
using static prjx.libx.util;
//using static prjx.libBiz.tgBiz;
using static prjx.libx.afrmwk;


//using static SqlParser.Ast.DataType;

//using static SqlParser.Ast.CharacterLength;
//using static prjx.libx.avClas;
using static prjx.libx.dtime;
//using static prjx.libx.fulltxtSrch;
using static prjx.libx.tglib;

namespace prjx.libx
{
    internal class CallFun
    {
        public static string call_exe_retStr(string exePath, string scriptPath, SortedList prm)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), scriptPath, prm));

            string timestamp2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            Directory.CreateDirectory("prmDir");
            File.WriteAllText($"prmDir/prm{timestamp2}.txt", json_encode(prm));
            string prm_fileAbs = GetAbsolutePath($"prmDir/prm{timestamp2}.txt");

           Print(prm_fileAbs);
            string str = call_exe_Pstr(exePath, scriptPath, prm_fileAbs);

            print_varDump(__METHOD__, $"call_exe_Pstr.retRaw", str);
            string marker = "----------marker----------";
            str = SubstrAfterMarker(str, marker);
            str = str.Trim();
            dbgCls.PrintRet(__METHOD__, str);
            return str;
        }


        public static string call_exec_RetList(string execpath, string scriptPath, SortedList prm, string outputDir)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), scriptPath, prm));

            string timestamp2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            Directory.CreateDirectory("prmDir");
            File.WriteAllText($"prmDir/prm{timestamp2}.txt", json_encode(prm));

            string prm_fileAbs = GetAbsolutePath($"prmDir/prm{timestamp2}.txt");


            string str = call_exe_Pstr(execpath, scriptPath, prm_fileAbs);
            string marker = "----------qryrzt----------";
            string strAft = SubstrAfterMarker(str, marker);
            strAft = strAft.Trim();
            string prjDir = @"../../";
            string txt = File.ReadAllText(outputDir + "/" + strAft);
            dbgCls.PrintRet(__METHOD__, txt);
            return txt;
        }

        internal static string callPhp(string scriptPath, SortedList prm)
        {
            throw new NotImplementedException();
        }
        /**
      * // 示例用法
     Action<int, int> add = (a, b) =>print($"Sum: {a + b}");
     object[] args = { 10, 20 };
     
     // 调用委托并传递参数数组
     call_user_func_array(add, args);

     // 通过方法名称调用静态方法并传递参数数组
     call_user_func_array("PrintMessage", args);
      * 
      * 
      * 
      * 
      */
        /// <summary>
        /// 调用指定的委托或方法，并传递参数数组作为参数。
        /// </summary>
        /// <param name="callback">要调用的委托或方法</param>
        /// <param name="args">参数数组</param>
        public static object call_user_func_array(Delegate callback, object[] args)
        {
            return callback.DynamicInvoke(args);
        }
        public static object CallUserFunc409(Delegate callback, params object[] args)
        {
            var __METHOD__ = callback.Method.Name;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(args));
            object o = null;
            try
            {

                //   else
                o = callback.DynamicInvoke(args);
            }
            catch (jmp2endEx e1)
            {
                throw e1;
            }
            catch (Exception e)
            {
                if (e is System.Reflection.TargetInvocationException)
                {
                    if (e.ToString().Contains("jmp2endEx"))
                    {
                        PrintTimestamp($" CallUserFunc409() ctch ex ,mtth:{__METHOD__}");
                        PrintRet(__METHOD__, 0); Jmp2endDep();
                    }

                }
               Print($"---catch ex----call mtdh:{__METHOD__}  prm:{json_encode_noFmt(func_get_args(args))}");
               Print(e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + json_encode_noFmt(func_get_args(args)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            //    call
            if (o != null)
                PrintRet(__METHOD__, o);
            else
                PrintRet(__METHOD__, 0);
            return o;

        }
        public static void call_user_func(string className, string methodName, object[] parameters)
        {
            try
            {
                // 获取当前程序集中所有的类型
                var type = Assembly.GetExecutingAssembly().GetType(className);
                if (type == null)
                {
                   Print($"找不到类 '{className}'。");
                    return;
                }

                // 获取参数类型数组
                Type[] paramTypes = parameters != null ? System.Array.ConvertAll(parameters, p => p.GetType()) : Type.EmptyTypes;

                // 获取方法信息，尝试先找静态方法
                var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);

                // 如果没有找到静态方法，再尝试找实例方法
                if (method == null)
                {
                    method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
                }

                if (method == null)
                {
                   Print($"找不到方法 '{methodName}' 或参数不匹配。");
                    return;
                }

                // 如果是静态方法，直接调用
                if (method.IsStatic)
                {
                    method.Invoke(null, parameters);
                }
                else
                {
                    // 如果是实例方法，需要先创建实例
                    var instance = Activator.CreateInstance(type);
                    method.Invoke(instance, parameters);
                }
            }
            catch (Exception ex)
            {
               Print($"调用方法时发生错误：{ex.Message}");
            }
        }

        public static void call_user_func_array(string classAndMethod, object[] parameters)
        {
            // 分割类名和方法名
            var parts = classAndMethod.Split('.');
            if (parts.Length != 2)
            {
               Print("参数格式不正确，请使用 'ClassName.MethodName' 格式。");
                return;
            }

            string className = parts[0];
            string methodName = parts[1];

            try
            {
                // 获取当前程序集中所有的类型
                var type = Assembly.GetExecutingAssembly().GetType(className);
                if (type == null)
                {
                   Print($"找不到类 '{className}'。");
                    return;
                }

                // 获取参数类型数组
                Type[] paramTypes = parameters != null ? System.Array.ConvertAll(parameters, p => p.GetType()) : Type.EmptyTypes;

                // 获取静态方法信息
                var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, null, paramTypes, null);
                if (method == null)
                {
                   Print($"找不到方法 '{methodName}' 或参数不匹配。");
                    return;
                }

                // 调用静态方法
                method.Invoke(null, parameters);
            }
            catch (Exception ex)
            {
               Print($"调用方法时发生错误：{ex.Message}");
            }
        }
        /// <summary>
        /// 检查某个类中是否存在指定名称的方法。
        /// </summary>
        /// <param name="type">要检查的类的类型</param>
        /// <param name="methodName">要检查的方法名称</param>
        /// <returns>如果存在具有指定名称的方法，则返回 true；否则返回 false</returns>
        public static bool function_exists(Type type, string methodName)
        {
            // 获取该类型的所有公共方法
            MethodInfo[] methods = type.GetMethods();

            // 检查是否存在指定名称的方法
            foreach (MethodInfo method in methods)
            {
                if (method.Name == methodName)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
