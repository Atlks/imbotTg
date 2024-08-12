using prjx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    public class exCls
    {

        /// <summary>
        /// 设置全局异常处理程序。
        /// </summary>
        /// <param name="handler">异常处理程序。</param>
        public static void set_exception_handler(Action<Exception> handler)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                handler(e.ExceptionObject as Exception);
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                handler(e.Exception);
                e.SetObserved();
            };
        }

        /// <summary>
        /// 异常处理程序。
        /// </summary>
        /// <param name="ex">捕获的异常。</param>
        public static void HandleException(Exception ex)
        {
            // 在这里处理异常，例如记录日志或显示错误信息
            Print("捕获到未处理的异常:");
            Print($"消息: {ex.Message}");
            Print($"堆栈跟踪: {ex.StackTrace}");
        }

        public static void set_error_handler()
        {
            AppDomain.CurrentDomain.UnhandledException += exCls.CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += exCls.TaskScheduler_UnobservedTaskException;
        }

        // set_error_handler
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Print("FUN CurrentDomain_UnhandledException()");
                Print("捕获未处理的同步异常：");
                Print(((Exception)e.ExceptionObject).Message);
                // 这里可以记录日志或执行其他处理
                logCls.logErr2025((Exception)e.ExceptionObject, "CurrentDomain_UnhandledException", "errlog");
                Print("END FUN CurrentDomain_UnhandledException()");

                // 延迟启动一个新的线程
                new System.Threading.Thread(() =>
                {
                    // 恢复应用程序逻辑
                    //print("Application is recovering...");
                  //  Program.Main(null);
                    // Restart or recover logic here
                }).Start();
            }
            catch (jmp2endEx e1)
            {

            }
            catch (Exception ex)
            {
                Print(ex.ToString());
                Print("END FUN CurrentDomain_UnhandledException()");
            }

        }

        public static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                Print("\n\nFUN TaskScheduler_UnobservedTaskException()");
                Print("捕获未处理的异步异常：");
                Print("sender=》 " + sender);
                Print("emsg=>" + e.Exception.Message);

                // 解析堆栈跟踪，获取出错的异步函数名称
                if (e.Exception.StackTrace != null)
                    foreach (var stackFrame in e.Exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                    {
                        Print(stackFrame);
                        if (stackFrame.Contains("ThrowExceptionAsync"))
                        {
                            Print($"出错的异步函数: {stackFrame.Trim()}");
                            break;
                        }
                    }
                // 这里可以记录日志或执行其他处理
                e.SetObserved(); // 标记异常已观察到，防止程序崩溃   // 阻止异常传播

                Hashtable hashtable = new Hashtable();
                hashtable.Add("sender", sender);
                hashtable.Add("UnobservedTaskExceptionEventArgs", e);
                logCls.logErr2025(hashtable, "TaskScheduler_UnobservedTaskException", "errAsyncDir");
                //// 延迟启动一个新的线程
                //new System.Threading.Thread(() =>
                //{
                //    // 恢复应用程序逻辑
                //    //print("Application is recovering...");
                //    Program.Main(null);
                //    // Restart or recover logic here
                //}).Start();
                logCls.logErr2025((Exception)e.Exception, "TaskScheduler_UnobservedTaskException", "errlog");
                ConsoleMy.print("END FUN TaskScheduler_UnobservedTaskException()");

            }
            catch (jmp2endEx e1)
            {

            }
            catch (Exception ex)
            {
                ConsoleMy.print(ex.ToString());
                ConsoleMy.print("END FUN TaskScheduler_UnobservedTaskException()");
            }
        }
    }
}
