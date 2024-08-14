global using static libx.TmrTask;
using prjx.libx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libx
{
    internal class TmrTask
    {    // 定时任务调度器
        public static void ScheduleDailyTask(int hour, int minute, Delegate task)
        {
            var __METHOD__ = nameof(ScheduleDailyTask);
            NewThrd(() =>
            {

                PrintCallFunArgs(__METHOD__, func_get_args(hour, minute, task.Method.Name));

                CreateFolderBasedOnDate(BaseFolderName4dlyrptPart);

                while (true)
                {
                    DateTime now = DateTime.Now;
                    DateTime nextRun = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

                    // 如果当前时间已经超过计划时间，则将任务安排在第二天
                    if (now > nextRun)
                    {
                        nextRun = nextRun.AddDays(1);
                    }

                    TimeSpan duration = nextRun - now;

                    PrintLog("等待到下一个计划时间 " + duration);
                    // 等待到下一个计划时间
                    Sleep(duration);
                    //   await Task.Delay(duration);

                    // 执行任务
                    task.DynamicInvoke();
                    SleepSec(30);
                    //   task();
                }

            });

            SleepSec(1.5);
            PrintRet(__METHOD__, "");
        }


    }
}
