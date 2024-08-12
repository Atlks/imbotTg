global using static prjx.libx.bsccode;
using mdsj.libBiz;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace prjx.libx
{
    internal class bsccode
    {
        public static HashSet<string> SplitToHashset(string input)
        { // 使用 Split 方法将字符串分割成数组
            string[] items = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // 将数组转换为 HashSet<string>
            return new HashSet<string>(items);

        }

        // 解析 <%=Print(888)%> 并提取函数和参数
        public static string[] ParseExpression(string expression)
        {
            // 提取 <%= ... %> 之间的内容
            string pattern = @"<%=([^%>]+)%>";
            var regex = new Regex(pattern);
            var match = regex.Match(expression);

            if (match.Success)
            {
                // 提取函数和参数
                string funcWithArgs = match.Groups[1].Value.Trim();

                // 解析函数名和参数
                string[] funcAndArgs = ParseFunctionAndArguments(funcWithArgs);
                return funcAndArgs;
            }

            throw new ArgumentException("无效的表达式格式。");
        }

        // 解析函数名和参数
        public static string[] ParseFunctionAndArguments(string funcWithArgs)
        {
            // 匹配函数名和参数
            string pattern = @"(\w+)\(([^)]*)\)";
            var regex = new Regex(pattern);
            var match = regex.Match(funcWithArgs);

            if (match.Success)
            {
                string functionName = match.Groups[1].Value;
                string arguments = match.Groups[2].Value;

                // 返回函数名和参数
                return new string[] { functionName, arguments };
            }

            throw new ArgumentException("无效的函数格式。");
        }
        public static string RendHtm(string f)
        {
            List<string> rztlist511 = new List<string>();
            List<string> segments222 = SplitByExpressions(f);
            foreach (string token in segments222)
            {
                if (token.StartsWith("<%="))
                {

                    string[] Fun508 = ParseExpression(token);
                    string fun = Fun508[0];
                    string arg = Fun508[1];
                    object[] objectArray = new object[] { arg };
                    object rzt = CallxTryx(fun, objectArray);
                    rztlist511.Add(ToStr(rzt));
                }
                else
                    rztlist511.Add(token);
            }
            string rzt511 = Join(rztlist511);
            return rzt511;
        }

        public static object Eval(string code)
        {
            try
            {
                var result = CSharpScript.EvaluateAsync(code, ScriptOptions.Default).Result;
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string FmtPrks(string svrPks)
        {
            if (string.IsNullOrEmpty(svrPks))
                return "\n 目前园区设置为空";
            var l = svrPks.Length;
            svrPks = svrPks.Replace("\n", ",");
            string[] items = svrPks.Split(",");
            if (items.Length == 0)
                return "\n 目前园区设置为空";
            svrPks = AddIdxToElmt(items, "\n");
            return "\n 已经设置园区:" + "\n"+svrPks;
        }
        public static double Avg(List<SortedList> list, string fieldName)
        {
            try
            {
                if (list == null || list.Count == 0)
                {
                    return 0;
                }

                var values = new List<double>();

                foreach (var sortedList in list)
                {
                    if (sortedList.ContainsKey(fieldName) && sortedList[fieldName] is double)
                    {
                        //toNumber( sortedList[fieldName]
                        values.Add(GetFieldAsNumber(sortedList, fieldName));
                    }
                }

                if (values.Count == 0)
                {
                    return 0;
                }

                return values.Average();
            }
            catch (Exception e)
            {
                PrintCatchEx("Avg", e);
                return 0;
            }

        }


     //  public static 

        public static bool Condt(Func<string, bool> fn, string ctry)
        {
            SortedList cdt1 = new SortedList();
            cdt1.Add("cdt", GetMethodName(fn) + $"({ctry})");
            PrintStr("❓?" + GetMethodName(fn) + $"({ctry})");
            PrintStr(" => ");
            bool r = fn(ctry);
            cdt1.Add("rzt", r);
            if (r)
                PrintStr("✅:)TRUE");
            else
                PrintStr("❌:(FALSE");
            PrintStr(" ,  ");
            PrintFmtAstCdt(cdt1);
            ArrayList li = (ArrayList)ifStrutsThrdloc.Value["cdts"];
            li.Add(cdt1);
            return r;
        }

        public static void PrintFmtAstCdt(SortedList tb)
        {
            Print("    " + tb["cdt"].ToString() + "=>" + tb["rzt"].ToString());
           // Print(EncodeJson(tb));
        }

        public static void iff(bool cdtRzt, Action act1)
        {
            SortedList ifAst = ifStrutsThrdloc.Value;
            SortedList tb = new SortedList();
            tb.Add("cdtRzt", cdtRzt);
            ifAst["cdtsRzt"] = cdtRzt;
            // cd1 cd2 
            PrintStr("\n❓❓❓?? IF rztIS ");
            if (cdtRzt)
            {
                Print("✅:))TRUE");
                tb.Add("CHOOSE", "THEN");
                ifAst["choose"] = "THEN";
            }
              
            else
            {
                Print("❌:((FALSE");
                tb.Add("CHOOSE", "ELSE");
                ifAst["choose"] = "ELSE";
            }
               

            if (cdtRzt)
            {
                Print("➡️➡️>THEN");
                if (act1 != null)
                    act1();

            }

            Print("\n🔚❓❓ENDIF");
            PrintAstIfStmt(tb);
        }

        public static void PrintAstIfStmt(SortedList tb)
        {
            Print(EncodeJson(tb));
        }

        public static void iff(bool cdt, Action act1, Action elseAct)
        {
            // cd1 cd2 
            PrintStr("\n❓❓❓??IF is ");
            if (cdt)
                Print("✅:))TRUE");
            else
                Print("❌:((FALSE");

            if (cdt)
            {
                Print("➡️➡️>THEN");
                if(act1!=null)
                   act1();

            }
            else
            {
                Print("☑️☑️:::ELSE"); 
                if (elseAct != null)
                    elseAct();
            }
            Print("\n🔚❓❓ENDIF");
        }
      
        public static void echo(object v)
        {
            Print(v);
        }
        public static void Jmp2endCurFun()
        {
            throw new jmp2endCurFunEx();
        }
        public static void foreach_objKey(object obj, Func<PropertyInfo, object> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(obj);

                try
                {
                    //  if(chatid== -1002206103554)
                    fun(property);
                }
                catch (Exception e)
                {
                    PrintCatchEx("foreach_hashtable", e);
                    //  print(e);
                }
            }

        }

        /// <summary>
        /// 循环的 Emoji
        //🔄🔁♻️ 
        //✅  ☑️
        /// </summary>
        /// <param name="list_aftFltr2"></param>
        /// <param name="qrystr"></param>
        /// <returns></returns>
        public static List<SortedList> SliceByPagemodeByQrystr(List<SortedList> list_aftFltr2, string qrystr)
        {
            //  Jmp2end
            SortedList qryMap = GetHashtableFromQrystr(qrystr);
            int page = GetFieldAsInt(qryMap, "page", 0);
            int pagesize = GetFieldAsInt(qryMap, "pagesize", 10);
            int start = (page - 1) * pagesize;
            List<SortedList> list_rzt = SliceX(list_aftFltr2, start, pagesize);

            //------------add col
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            foreach (var sortedList in list_rzt)
            {

                SetField938(sortedList, "pages", CalculateTotalPages(pagesize, list_aftFltr2.Count));

            }
            return list_rzt;
        }
        public static SortedList RemoveKeys(SortedList originalDictionary, string commaSeparatedKeys)
        {
            // 分割逗号分割的字符串并移除前后空白
            var keysToRemove = new HashSet<string>(commaSeparatedKeys.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);

            // 创建新的字典，只保留不在 keysToRemove 中的键值对
            SortedList newDictionary = new SortedList();
            foreach (DictionaryEntry kvp in originalDictionary)
            {
                if (!keysToRemove.Contains(kvp.Key))
                {
                    newDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            return newDictionary;
        }
        public static Dictionary<string, string> RemoveKeys(Dictionary<string, string> originalDictionary, string commaSeparatedKeys)
        {
            // 分割逗号分割的字符串并移除前后空白
            var keysToRemove = new HashSet<string>(commaSeparatedKeys.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);

            // 创建新的字典，只保留不在 keysToRemove 中的键值对
            var newDictionary = new Dictionary<string, string>(originalDictionary.Count);
            foreach (var kvp in originalDictionary)
            {
                if (!keysToRemove.Contains(kvp.Key))
                {
                    newDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            return newDictionary;
        }
        public static HashSet<string> foreach_HashSet(HashSet<string> originalSet, Func<string, string> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            HashSet<string> updatedSet = new HashSet<string>();

            foreach (string str in originalSet)
            {
                updatedSet.Add(fun(str));
            }

            return updatedSet;
        }
        public static void ForeachHashSet(HashSet<string> originalSet, Action<string> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            HashSet<string> updatedSet = new HashSet<string>();

            foreach (string str in originalSet)
            {
                try
                {
                    fun(str);
                }
                catch (Exception e)
                {
                    PrintCatchEx("foreach_HashSet", e);
                }

            }


        }
        static void FforeachProcessFilesAsyncDp(string folderPath, Func<string, Task> fileAction)
        {
            if (System.IO.Directory.Exists(folderPath))
            {
                string[] files = System.IO.Directory.GetFiles(folderPath);
                foreach (string file in files)
                {

                    fileAction(file);
                }
            }
            else
            {
                Print("The specified folder does not exist.");
            }
        }
        public static void ForeachHashtable(Hashtable chtsSess, Func<DictionaryEntry, object> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
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
        public static void ForeachHashtableFlgVer(Hashtable chtsSess, Action<DictionaryEntry> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            foreach (DictionaryEntry de in chtsSess)
            {
                //if (Convert.ToInt64(de.Key) == Program.groupId)
                //    continue;
                //  var chatid = Convert.ToInt64(de.Key);
                try
                {
                    //  if(chatid== -1002206103554)
                    fun(de);
                    if (jmp2exitFlagInThrd.Value == true)
                        break;
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


        public static void ForeachHashtable(Hashtable chtsSess, Action<DictionaryEntry> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
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

        public static bool And(bool left, bool right)
        {
            return left && right;
        }
        public static bool Or(bool left, bool right)
        {
            return left && right;
        }
        public static object Invoke(string methodName, params object[] args)
        {
            return Callx(methodName, args);
        }



        /// <summary>
        ///     PrintColoredText("This is blue text.", ConsoleColor.Blue);
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void PrintColoredText(string text, ConsoleColor color)
        {
            // 输出一个笑脸
            Console.OutputEncoding = Encoding.UTF8;
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }


        public static void NewThrd(Action act1)
        {
            // 使用 Task.Run 启动一个新的任务
            System.Threading.Tasks.Task newTask = System.Threading.Tasks.Task.Run(() =>
            {
                // callxTryJmp(task1);
                try
                {
                    act1();
                }
                catch (Exception e)
                {
                    PrintCatchEx("TaskRunNewThrd", e);
                    logErr2025(e, nameof(TaskRunNewThrd), "errlog");
                }


                //  callxTryJmp(OnMsg, update, reqThreadId);

            });
        }

        public static void TaskRunNewThrd(Action act1)
        {
            // 使用 Task.Run 启动一个新的任务
            System.Threading.Tasks.Task newTask = System.Threading.Tasks.Task.Run(() =>
            {
                // callxTryJmp(task1);
                try
                {
                    act1();
                }
                catch (Exception e)
                {
                    PrintCatchEx("TaskRunNewThrd", e);
                    logErr2025(e, nameof(TaskRunNewThrd), "errlog");
                }


                //  callxTryJmp(OnMsg, update, reqThreadId);

            });
        }
        public static void TaskRun(Func<System.Threading.Tasks.Task> value)
        {
            callAsyncNewThrdx(value);
        }
        public static void CallAsyncNewThrd(Action task1)
        {
            // 使用 Task.Run 启动一个新的任务
            System.Threading.Tasks.Task newTask = System.Threading.Tasks.Task.Run(() =>
            {
                // callxTryJmp(task1);
                try
                {
                    task1();
                }
                catch (Exception e)
                {
                    PrintCatchEx("callAsync", e);
                    logErr2025(e, nameof(callAsyncNewThrdx), "errlog");
                }


                //  callxTryJmp(OnMsg, update, reqThreadId);

            });


            //   await Task.Run(action);
        }

        public static void callAsyncNewThrdx(Func<object> task1)
        {
            // 使用 Task.Run 启动一个新的任务
            System.Threading.Tasks.Task newTask = System.Threading.Tasks.Task.Run(() =>
            {
                // callxTryJmp(task1);
                try
                {
                    task1();
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(callAsyncNewThrdx), e);
                }


                //  callxTryJmp(OnMsg, update, reqThreadId);

            });


            //   await Task.Run(action);
        }

        public static void PrintExcept(string mthdName, Exception e)
        {

            Print($"------{mthdName}() catch ex----------_");
            Print(e);
            Print($"------{mthdName}() catch ex finish----------_");
        }

        public static void PrintCatchEx(string v, Exception e)
        {
            Print($"------{v}() catch ex----------_");
            Print(e);
            Print($"------end {v}() catch ex finish----------_");
        }
        public static object Call(string authExprs, Delegate callback, params object[] args)
        {

            var __METHOD__ = callback.Method.Name;
            object o = null;
            try
            {
                o = callback.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                PrintCatchEx(__METHOD__, e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + encodeJsonNofmt(func_get_args(args)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            return o;
        }

        /*
         
         .子程序 call, 公开, 通用型
.参数 callback, 句柄型
.参数 args, 数据组
         */
        public static object Call(Delegate callback, params object[] args)
        {

            var __METHOD__ = callback.Method.Name;
            object o = null;
            try
            {
                o = callback.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                PrintCatchEx(__METHOD__, e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + encodeJsonNofmt(func_get_args(args)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            return o;
        }

        public static object Callx(bool authExpRzt, Delegate callback, params object[] args)
        {
            return CallUserFunc409(callback, args);
        }

        public static object CallxTryJmp(Delegate callback, params object[] objs)
        {

            try
            {
                return Callx(callback, objs);

            }
            catch (jmp2endEx e)
            {
                PrintCatchEx("callxTryJmp", e);
                Print("callxTryJmp  callmeth=>" + callback.Method.Name);
            }
            //catch (Exception e)
            //{
            //    print_catchEx(nameof(MsgHdlrProcess), e);
            //}
            return 0;
        }
        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="placeHold"></param>
        /// <param name="replaceStr"></param>
        /// <returns></returns>
        public static string[] Replace(string[] lines, string placeHold, string replaceStr)
        {
            if (lines == null)
            {
                return new string[0];
            }

            if (placeHold == null)
            {
                // throw new ArgumentNullException(nameof(placeHold));
            }

            if (replaceStr == null)
            {
                //  throw new ArgumentNullException(nameof(replaceStr));
            }

            // 创建一个新数组来存储替换后的结果
            string[] result = new string[lines.Length];

            // 遍历每一行进行替换
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = lines[i].Replace(placeHold, replaceStr);
            }

            return result;
        }
        public static void AddElemtStrcomma(string noTrigSrchMsgs, HashSet<string> hs11)
        {
            string[] a = noTrigSrchMsgs.Split(",");
            foreach (string s in a)
            {
                if (s.Trim().Length > 0)
                    hs11.Add(s);
            }
        }
        public static string AddElmts(string e, string strComma)
        {
            if (strComma == "")
                return e;
            return strComma + "," + e;
        }
        public static object DelElmts(string e, string strComma)
        {
            ArrayList li = ConvertToArrayList(strComma);
            li.Remove(e);
            return ConvertArrayListToCommaSeparatedString(li);
        }
        public static string DelElmt(string e, string strComma)
        {
            ArrayList li = ConvertToArrayList(strComma);
            li.Remove(e);
            return ConvertArrayListToCommaSeparatedString(li);
        }

        public static string removeDulip(string newParks)
        {
            HashSet<string> hs = new HashSet<string>();
            string[] a = newParks.Split(",");
            foreach (string pk in a)
            {
                if (pk.Trim().Length > 0)
                    hs.Add(pk.Trim().ToUpper());
            }
            return JoinWzComma(hs);
        }
        public static string JoinWzComma(HashSet<string> hashSet)
        {
            if (hashSet == null)
            {
                return "";
            }

            // 使用 string.Join 方法将 HashSet 元素连接成逗号分割的字符串
            return string.Join(",", hashSet);
        }
        public static void DelField(SortedList cfg, string key, string ddd)
        {
            if (cfg.ContainsKey(key))
                cfg.Remove(key);
        }
        public static void DelField(SortedList cfg, string key)
        {
            if (cfg.ContainsKey(key))
                cfg.Remove(key);
        }
        public static object Callx(string authExp, Delegate callback, params object[] args)
        {
            return CallUserFunc409(callback, args);
        }
        public static object Callx(Delegate callback, params object[] args)
        {
            Jmp2endCurFunFlag.Value = false;

            //  return CallUserFunc409(callback, args);
            MethodInfo method = callback.Method;
            var __METHOD__ = method.Name;
            jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(args));
            object o = null;
            try
            {
                // Delegate.DynamicInvoke
                //here dync invk slow ,need 40 ms
                //   else
                o = callback.DynamicInvoke(args);
                if(jmp2exitFlagInThrd.Value)
                {
                    PrintRet(__METHOD__, "");
                    return o;
                }
            }
            catch (jmp2endCurFunEx ee)
            {
                PrintRet(__METHOD__, "");
                return o;
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
                        jmp2exitFlagInThrd.Value = true;
                        PrintTimestamp($" Callx(Delegate) ctch ex ,mtth:{__METHOD__}");

                        dbgpad = dbgpad - 4;
                        Jmp2end(jmp2endCurFunInThrd.Value);
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

        public static object CallRetmod(Delegate callback, params object[] args)
        {

            //  return CallUserFunc409(callback, args);
            MethodInfo method = callback.Method;
            var __METHOD__ = method.Name;
            jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(args));
            object o = null;
            try
            {

                //   else
                o = callback.DynamicInvoke(args);
            }
            catch (jmp2endCurFunEx ee)
            {
                PrintRet(__METHOD__, "");
                return o;
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
                        jmp2exitFlagInThrd.Value = true;
                        PrintTimestamp($" Callx(Delegate) ctch ex ,mtth:{__METHOD__}");

                        dbgpad = dbgpad - 4;
                        Jmp2end(jmp2endCurFunInThrd.Value);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbf_share"></param>
        /// <returns></returns>
        //public static List<SortedList> GetListFrmSqltWzPersst(string dbf_share)
        //{
        //    try
        //    {
        //        var list8 = new List<SortedList>();
        //        if (!IsFileExist(dbf_share))
        //            list8 = new List<SortedList>();
        //        else
        //        {
        //            list8 = ormSqlt.qryV2(dbf_share);
        //        }
        //        //--------------定时持久化downedUrl
        //        // -------------创建一个定时器，每2秒触发一次
        //        System.Timers.Timer timer = new System.Timers.Timer(2000);
        //        timer.Elapsed += (sender, e) =>
        //        {
        //            //del dbf file todo 
        //            ormSqlt.saveMltHiPfm(list8, dbf_share);                 
        //        };
        //        timer.AutoReset = true;
        //        timer.Enabled = true;
        //        timer.Start();
        //        return list8;
        //    }
        //    catch (Exception ex)
        //    {
        //        ConsoleWriteLine($"An error occurred: {ex.Message}");
        //        return new List<SortedList>();
        //    }
        //}



        public static HashSet<string> NewSet(string f)
        {
            try
            {



                var hashSet = new HashSet<string>();
                if (!IsFileExist(f))
                    hashSet = new HashSet<string>();
                else
                {
                    string json = File.ReadAllText(f);
                    hashSet = JsonConvert.DeserializeObject<HashSet<string>>(json);


                }
                //定时持久化downedUrl
                // setHsstToF(rmvs3, "rmvs3.json");
                // 创建一个定时器，每2秒触发一次
                System.Timers.Timer timer = new System.Timers.Timer(2000);
                timer.Elapsed += (sender, e) =>
                {
                    SetHashstToFil(hashSet, f);
                };
                timer.AutoReset = true;
                timer.Enabled = true;
                timer.Start();


                return hashSet;
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"An error occurred: {ex.Message}");
                return new HashSet<string>();
            }

        }
      
     
        public static void PrintWarn(string v)
        {
            Print("!!!!****⚠️⚠️⚠️⚠️⚠️⚠️⚠️" + v);
        }
    
        public static void TryNotLgJmpEndAsync(Action value)
        {
            try
            {
                  value();
            }
            catch (jmp2endEx e)
            {

            }
            catch (Exception e)
            {
                PrintCatchEx("RequestDelegate", e);
            }
        }


        public static void CallTryAllV2( string blockName,Action value)
        {
            try
            {
                value();
            }
            catch (Exception e)
            {
                PrintCatchEx($" blk:{blockName}.callTryAll", e);
            //    PrintCatchEx("WbapiXgetlist", e);
            }
        }

        public static void callTryAll(Action value)
        {
            try
            {
                value();
            }
            catch (Exception e)
            {
                PrintCatchEx("callTryAll", e);
            }
        }

      

        public static void LoopForever()
        {
            while (true)
            {
                Thread.Sleep(500);
            }
        }
        public static string AddTimet(int secondsToAdd)
        {
            // 获取当前时间
            DateTime currentTime = DateTime.Now;

            // 计算未来时间
            DateTime futureTime = currentTime.AddSeconds(secondsToAdd);

            // 定义时间格式
            string format = "yyyy-MM-dd HH:mm:ss"; // 根据需要调整格式

            // 返回格式化后的时间字符串
            return futureTime.ToString(format);
        }

        public static void IfNotExist(bool bl, Action act)
        {
            iff(bl, act);
        }

        public static void IfExist(bool bl, Action act)
        {
            iff(bl, act);
        }

        /// <summary>
        /// sanyaosu  name,birday ,pic
        ///   Print(newToken("00799988", 3600 * 24 * 7));
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="exprtTimeSecsAftr"></param>
        /// <returns></returns>
        public static string newToken(string uid, int exprtTimeSecsAftr)
        {
            string tkExprt = AddTimet(exprtTimeSecsAftr);
            string issTime = DateTime.Now.ToString();
            string MRG = EncryptAes(uid + "." + tkExprt + "." + issTime);
            string tkOri = uid + "_" + MRG;
            return tkOri;
        }
        //public static FuncDlt1 NewDelegate308(string methodName) 
        //{
        //    // PrintTimestamp("start CreateDelegate()"+methodName);
        //    // 获取方法信息
        //    MethodInfo methodInfo = GetMethInfo(methodName);
        //    if (methodInfo == null)
        //    {
        //        throw new ArgumentException($"Method '{methodName}' not found.");
        //    }

        //    // 创建参数表达式
        //    ParameterExpression param = Expression.Parameter(typeof(string), "qrystr");

        //    // 创建方法调用表达式
        //    MethodCallExpression methodCall = Expression.Call(methodInfo, param);

        //    // 创建 Lambda 表达式
        //    Expression<Func<string, string>> lambda = Expression.Lambda<Func<string, string>>(methodCall, param);

        //    // 编译 Lambda 表达式
        //    FuncDlt1 func = lambda.Compile();
        //    // PrintTimestamp(" endfun CreateDelegate()" + methodName);
        //    return func;
        //}


        /// <summary>
        ///  // 定义方法签名


        // 使用表达式树创建委托
        // var f = CreateDelegate<Func<string, string>>(methodName);

        // 使用委托调用方法
        //   string result = f("example query");
        //  这个 pfm is ver fast..not have pefm prblm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Func<string, string> NewDelegate<T>(string methodName) where T : Delegate
        {
            // PrintTimestamp("start CreateDelegate()"+methodName);
            // 获取方法信息
            MethodInfo methodInfo = GetMethInfo(methodName);
            if (methodInfo == null)
            {
                throw new ArgumentException($"Method '{methodName}' not found.");
            }

            // 创建参数表达式
            ParameterExpression param = Expression.Parameter(typeof(string), "qrystr");

            // 创建方法调用表达式
            MethodCallExpression methodCall = Expression.Call(methodInfo, param);

            // 创建 Lambda 表达式
            Expression<Func<string, string>> lambda = Expression.Lambda<Func<string, string>>(methodCall, param);

            // 编译 Lambda 表达式
            Func<string, string> func = lambda.Compile();
            // PrintTimestamp(" endfun CreateDelegate()" + methodName);
            return func;
        }


        public static T NewDelegateV2<T>(string methodName) where T : Delegate
        {
            // PrintTimestamp("start CreateDelegate()"+methodName);
            // 获取方法信息
            MethodInfo methodInfo = GetMethInfo(methodName);
            if (methodInfo == null)
            {
                throw new ArgumentException($"Method '{methodName}' not found.");
            }

            // 确保方法信息的返回类型和参数匹配
            //if (typeof(T).GetMethod("Invoke").ReturnType != methodInfo.ReturnType ||
            //    typeof(T).GetMethod("Invoke").GetParameters().Length != methodInfo.GetParameters().Length)
            //{
            //    throw new ArgumentException("Method signature does not match delegate.");
            //}

            // 创建参数表达式
            //  ParameterExpression param = Expression.Parameter(typeof(string), "qrystr");
            // 创建参数表达式
            var parameters = methodInfo.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();

            // 创建方法调用表达式
            MethodCallExpression methodCall = Expression.Call(methodInfo, parameters);

            // 创建 Lambda 表达式
            Expression<T> lambda = Expression.Lambda<T>(methodCall, parameters);

            // 编译 Lambda 表达式
           T func = lambda.Compile();
            // PrintTimestamp(" endfun CreateDelegate()" + methodName);
            return func;
        }


        public static string ClrCommaStr(string pkrPrm)
        {
            HashSet<string> hs = GetHashsetFrmCommaStr(pkrPrm);
            return ToStrComma(hs);
        }
        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="input">要压缩的字符串</param>
        /// <returns>压缩后的 Base64 字符串</returns>
        public static string CompressString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // 将字符串转换为字节数组
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // 使用 GZip 压缩字节数组
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(inputBytes, 0, inputBytes.Length);
                }

                // 将压缩后的字节数组转换为 Base64 字符串
                return System.Convert.ToBase64String(outputStream.ToArray());
            }
        }
        public static void PrintObj(JArray btns)
        {
            Print(EncodeJsonFmt(btns));
        }
        public static void Jmp2end(string levFn)
        {
            throw new jmp2endEx("🛑JMP2END from " + levFn + " ,GOTO END🛑🛑🛑.. ");
        }

        public static void Jmp2endDep()
        {
            // jmp2exitFlag = true;
            throw new jmp2endEx();
        }


        public static object CallxTryx(string methodName, params object[] args)
        {
            //  Print(" fun CallxTryx()" + methodName);
            var __METHOD__ = methodName;
            PrintCallFunArgs(methodName, dbgCls.func_get_args(args));
            jmp2endCurFunInThrd.Value = __METHOD__;
            var methodInfo = GetMethInfo(methodName);

            if (methodInfo == null)
            {
                Print("!!!$$waring  .methodinfo is null");
                PrintRetx(__METHOD__, "");
                return null;
            }


           // var delegateType = typeof(Func<string, List<SortedList>>);
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

        public static object CallRetvMode(string methodName, params object[] args)
        {
            PrintTimestamp(" enter fun Callx()" + methodName);
            var __METHOD__ = methodName; jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(methodName, func_get_args(args));
            var argsMkdFmt = ConvertToMarkdownTable(args);
            Print(argsMkdFmt);

            MethodInfo? methodInfo = GetMethInfo(methodName);

            if (methodInfo == null)
            {
                Print("......$$waring  .methodinfo is null");
                PrintRetx(__METHOD__, "");
                return null;
            }


            //      var delegateType = typeof(Func<string, List<SortedList>>);
            //  var delegateMethod = methodInfo.CreateDelegate(delegateType);

            // 假设你想要执行 YourMethodName 方法
            //   object[] args = { };
            object result = null;
            try
            {
                result = methodInfo.Invoke(null, args);

            }
            catch (jmp2endEx e)
            {
                return e;
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("jmp2endEx"))
                {
                    jmp2exitFlagInThrd.Value = true;
                    dbgpad = dbgpad - 4;
                    PrintTimestamp($"  Callx(str) catch jmp2endEx mthd:{methodName}");
                    return 0;
                    //   throw e.InnerException;
                    //   Jmp2end();
                }

                PrintExcept("call", e);
            }


            PrintRetx(__METHOD__, result);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }

        public static object Callx(string methodName, params object[] args)
        {
            PrintTimestamp(" enter fun Callx()" + methodName);
            var __METHOD__ = methodName; jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(methodName, func_get_args(args));
            var argsMkdFmt = ConvertToMarkdownTable(args);
            Print(argsMkdFmt);

            PrintTimestamp("  before  GetMethInfo()" + methodName);
            MethodInfo? methodInfo = GetMethInfo(methodName);
            PrintTimestamp("  end  GetMethInfo()" + methodName);
            if (methodInfo == null)
            {
                Print("!!!$$waring  .methodinfo is null");
                PrintRetx(__METHOD__, "");
                PrintTimestamp(" end fun Callx()" + methodName);
                return null;
            }


            //      var delegateType = typeof(Func<string, List<SortedList>>);
            //  var delegateMethod = methodInfo.CreateDelegate(delegateType);

            // 假设你想要执行 YourMethodName 方法
            //   object[] args = { };
            object result = null;
            try
            {
                result = methodInfo.Invoke(null, args);

            }
            catch (jmp2endEx e)
            {
                return e;
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("jmp2endEx"))
                {
                    jmp2exitFlagInThrd.Value = true;
                    dbgpad = dbgpad - 4;
                    PrintTimestamp($"  Callx(str) catch jmp2endEx mthd:{methodName}");
                    throw e.InnerException;
                    //   Jmp2end();
                }

                PrintExcept("call", e);
            }


            PrintRetx(__METHOD__, result);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }

        public static List<t> Append<t>(List<t> list2, List<t> list1)
        {
            //List<t> result = new List<t>();

            //// 获取最长列表的长度
            //int maxLength = Math.Max(list1.Count, list2.Count);

            //// 遍历并合并列表
            ////for (int i = 0; i < maxLength; i++)
            ////{
            //for (int i = 0; i < list1.Count; i++)
            //{
            //    result.Add(list1[i]);
            //}
            //here binxin todo
            for (int i = 0; i < list2.Count; i++)
            {
                list1.Add(list2[i]);
            }
            //}

            return list1;
        }


        public static void CallAsAsyncTaskRun(Action act)
        {
            try
            {
                act();
            }
            catch (Exception e)
            {
                PrintCatchEx(nameof(CallAsAsyncTaskRun), e);
            }
        }

       
   }
}
