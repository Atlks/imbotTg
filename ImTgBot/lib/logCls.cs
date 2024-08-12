global using static prjx.libx.logCls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
 
namespace prjx.libx
{
    internal class logCls
    {
        public static void RunSetRollLogFileV2()
        {
            SetLogHr(); RunSetRollLogFile();
        }
        public static void RunSetRollLogFile()
        {
            NewThrd(() =>
            {
                //定时轮换日志文件
                while (true)
                {
                    Thread.Sleep(5 * 1000);
                    if (DateTime.Now.Minute == 1)
                    {

                        SetLogHr();
                        Thread.Sleep(70 * 1000);
                    }

                }

            });
        }

        public static TextWriter originalConsoleOutput = Console.Out;
        public static void SetLogHr()
        {
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyy-MM-dd_HH");
            StreamWriter logFile = new StreamWriter($"log1037_{formattedDate}.log", append: true);

            // Save the original Console.Out


            // Create a new composite writer that writes to both the console and the log file
            Console.SetOut(new CompositeTextWriter(originalConsoleOutput, logFile));
        }

        public static void logErr2024(object e, string funName, string logdir,object othInf)
        {
            if (e is jmp2endEx)
                return;
            try
            {
                // 创建目录
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{logdir}/{timestamp}.txt";
                File.AppendAllText(fileName, funName + "()\n eStr=>" + e.ToString());

                File.AppendAllText(fileName,"\n inf=》" + JsonConvert.SerializeObject(othInf, Formatting.Indented));
                File.AppendAllText(fileName,"\n eFmt=》" + JsonConvert.SerializeObject(e, Formatting.Indented));
            }
            catch (Exception ex)
            {
               Print(ex.ToString());
            }


        }

        /// <summary>
        /// 记录错误信息到指定的日志文件。
        /// </summary>
        /// <param name="message">错误信息。</param>
        /// <param name="filePath">日志文件路径。</param>
        public static void error_log(string message, string filePath)
        {
            try
            {
                // 确保目录存在
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 记录错误信息到文件
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                // 处理记录日志时可能出现的异常
               Print($"Failed to log error: {ex.Message}");
            }
        }

        public static void error_logV2(object messageObj, string filePath)
        {
            if (messageObj is jmp2endEx)
                return;
            try
            {
                string message=json_encode(messageObj);
                // 确保目录存在
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 记录错误信息到文件
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                // 处理记录日志时可能出现的异常
               Print($"Failed to log error: {ex.Message}");
            }
        }

        public static void logErr2025(object e, string funName, string logdir)
        {
            if (e is jmp2endEx)
                return;
            try
            {
                // 创建目录
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{logdir}/{timestamp}.txt";
                File.AppendAllText( fileName, funName + "()\n e=>" + e.ToString());


                File.AppendAllText(fileName,"\n eFmt=》" +JsonConvert.SerializeObject(e, Formatting.Indented));
            }
            catch (Exception ex)
            {
                //Newtonsoft.Json.JsonSerializationException:
               Print(ex.Message.ToString());
            }


        }
        internal static void logF(object m, string logf)
        {
            try
            {
                Mkdir4File(logf);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
             
                Print(" fileName=>" + logf);
                if (IsString(m))
                {
                    AppendTextToFile(logf, m.ToString());
                }
                else
                    AppendTextToFile(logf, json_encode(m));
            }
            catch (Exception ex)
            {
                Print(ex.ToString());
            }
        }
        static void AppendTextToFile(string filePath, string content)
        {
            // 使用 File.AppendAllText 方法将内容追加到文件
            File.AppendAllText(filePath, content + Environment.NewLine);
        }
        internal static void log(object m, string logdir)
        {
            try
            {
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{logdir}/{timestamp}.json";
               Print(" logdir=>"+logdir);
               Print(" fileName=>" + fileName);
                if (IsString(m))
                {
                    System.IO.File.WriteAllText(  fileName, m.ToString());
                }else
                System.IO.File.WriteAllText( fileName, json_encode(m));
            }
            catch (Exception ex)
            {
               Print(ex.ToString());
            }
        }

        internal static void log(string mETHOD__, object prm, object val, string logdir, string reqThreadId)
        {
            try
            {
                string timestampPx = DateTime.Now.ToString("dd_HHmmss");
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
                string fileName = $"{logdir}/{timestamp}.{reqThreadId}.txt";
               Print(" logdir=>" + logdir);
               Print(" fileName=>" + fileName);
                string msg = $"{timestampPx} {mETHOD__}({json_encode_noFmt(prm)})::{json_encode_noFmt(val)} \n"; 
                if (IsString(msg))
                {
                    // 将字符串追加到文件
                    File.AppendAllText(fileName, ToString(msg));
                }
                else
                    System.IO.File.AppendAllText(fileName, json_encode(msg));
            }
            catch (Exception ex)
            {
               Print(ex.ToString());
            }
            
        }

        private static string? ToString(object val)
        {
            if (val == null)
                return "";
            return val.ToString();
        }
    }
}
