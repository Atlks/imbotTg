﻿global using static Program;
using Newtonsoft.Json;
using prjx.libx;
using prjx.libx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{





    //private static string ConcatenateKeysWithSpace(SortedList sortedList)
    //{
    //    // 使用 StringBuilder 来提高性能
    //    var sb = new System.Text.StringBuilder();

    //    foreach (DictionaryEntry entry in sortedList)
    //    {
    //        // 获取键并追加到 StringBuilder
    //        var key = entry.Key.ToString();
    //        sb.Append(key).Append(' ');
    //    }

    //    // 移除末尾多余的空格
    //    if (sb.Length > 0)
    //    {
    //        sb.Length--;  // 移除最后一个空格
    //    }

    //    return sb.ToString();
    //}


    public static void Main(string[] args)
    {

        var s = "| 连续缺失天数 | uname | uid|";
        var sss3 = Len("@dabao8998  大宝");
        //  大a宝   总宽度是5，这里计算为3需要补2byte
        //   大a宝   总宽度是7，这里计算为3需要补4byte
        var s222 = ConvertToMarkdownTableSpltLine(s);

        //string folderPath = $"{prjdir}/db/dlyrpt0812";
        //string mkd2console = GetRptToday(folderPath);
        //Print(mkd2console)


        Main2024(async () =>
        {

            rendTestAgl();
            if (IsExistFil("c:/teststart.txt"))
            {
                RptMonth();
                for (int i = 0; i < 50000; i++)
                {
                    SortedList o = new SortedList();
                    o.Add("id", i);
                    o.Add("name", "atii");
                    //    ormIni.Save2Ini(o, "us150dir");

                }
                PrintLog("fill ccache...");
                var list = ormIni.Qry("us150dir");
                PrintLog("stat...");
                //5w file need 5.5s
                  list = ormIni.Qry("us150dir");

                //8s  asy mode
                //     var list =await ormIni.QryAsync("us150dir");

                PrintLog("end...");

                // PrintObj(list.Count);
                // Print(list);
                Print("ddd");
            }

            RcvMsgStart();


         

            // 设置每天 5:30 执行任务
            if (IsExistFil("c:/teststart.txt"))
            {
                SortedList o = new SortedList();
                o.Add("id", 123);
                o.Add("name", "atii");
                ormIni.Save2Ini(o, "us150dir");

                Print("------------------------to console fmt --------");
                string folderPath = $"{prjdir}/cfg/tmp.md";
                string mkd2console = FormatMarkdown2consl(ReadAllText(folderPath));
                Print(mkd2console);
                //SleepSec(5);

                //SendMessage4DailyRpt();//send ntfy
                //SleepSec(5);
                //SumupDailyRpt();
                //SleepSec(5);
                //RptConsecutiveMissingDays();
                //SleepSec(5);

                RptMonth();
                // ScheduleDailyTask(12, 01, SendMessage4DailyRpt);//test

               // var lstMemIDs = GetGroupUserIdsAsync(ToLong(chatID, 0)).Result;
            }



            ScheduleDailyTask(18, 55, SendMessage4DailyRpt);
            //  ScheduleDailyTask(20, 30, SendMessage4DailyRpt);
            ScheduleDailyTask(21, 30, SendMessage4DailyRpt);
            ScheduleDailyTask(23, 55, SendMessage4DailyRpt);

            ScheduleDailyTask(3, 50, SumupDailyRpt);
            ScheduleDailyTask(4, 00, RptConsecutiveMissingDays);
            ScheduleDailyTask(4, 05, RptMonth);

            if (IsExistFil("c:/teststart.txt"))
            {
                ScheduleDailyTask(13, 31, SumupDailyRpt);//test

                ScheduleDailyTask(13, 32, RptConsecutiveMissingDays);//test
            }

        });

    }

    private static long ToLong(string str,long df)
    {
        try
        {
            long number = long.Parse(str);
            Console.WriteLine($"Converted number: {number}");
            return number;
        }
        catch (FormatException)
        {
            Console.WriteLine("Input string is not in a correct format.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("The number is too large or too small for a long.");
        }
        return df;
    }

    private static void rendTest2()
    {
        // 读取模板文件内容
        string template = System.IO.File.ReadAllText($"{prjdir}/cfg/tmplt1.md");
        var li = new List<dynamic>
            {
                new { uid = "001", uname = "Alice" },
                new { uid = "002", uname = "Bob" }
            };

        List<Hashtable> liht = ConvertToHashtableList(li);
        // 创建数据模型
        var data = new
        {
            title = "titlexxx",
            list = liht,
        };
        // 渲染模板
        string result = RenderTemplate4ur(template, data);
        Print(result);
    }














    // 从文件夹中获取文件名并以 JSON 格式返回






    public static void UpdtHdlr(Update update)
    {
        //  throw new NotImplementedException();
    }
    /// <summary>
    /// CmdrptHdlr
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="update"></param>
    /// <returns></returns>
    public static void CmdrptHdlr(string cmd, Update update)
    {
        SleepSec(5);//for test lev
        //    if(cmd=="rpt")
        string[] a = cmd.Split(" ");
        a = update.Message.Text.Split(" ");
        string date = a[1];
        SumupDailyRptBydate(date, update.Message.Chat.Id.ToString());
        RptConsecutiveMissingDaysBydate(date);
        RptMonth();

    }

    /// <summary>
    /// befor 5am use only
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="update"></param>
    public static void CmdrptodayHdlr(string cmd, Update update)
    {
        DelMsg(update, 1);
        SumupDailyRpt();
        RptConsecutiveMissingDays();
        RptMonth();
        
    }

    public static void CmdhlpHdlr(string cmd, Update update)
    {
        var s = "/rpt 0815\n";
        s = s + "/rptoday\n";
        s = s + "rptx 0815   rmt rpt\n ";
        Sendmsg(chatID, s);

    }

    public static void CmdrptxHdlr(string cmd, Update update)
    {
        //    if(cmd=="rpt")
        string[] a = cmd.Split(" ");
        a = update.Message.Text.Split(" ");
        string date = a[1];
        SumupDailyRptBydate(date, chatID);


    }
    public static async Task MsgHdlr(Update update)
    {

        var message = update.Message;
        if (message == null || string.IsNullOrEmpty(message.Text))
        {
            return;
        }

        //for log
        LogRcvMsgAsync(update, "MsgDir");

        await AddDlyrpt(message);


        // 创建存储消息的文件路径
        var filePath = Path.Combine(rcvmsgDir, $"message_{update.Message.MessageId}.txt");

        // 创建目录（如果不存在）
        Directory.CreateDirectory(rcvmsgDir);

        // 保存消息内容到文件
        await System.IO.File.WriteAllTextAsync(filePath, update.Message.Text);

        Console.WriteLine($"Message saved to {filePath}");
    }





    // 创建基于日期的文件夹
    //private static string CreateFolderBasedOnDate(string baseFolderName)
    //{
    //    string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseFolderName, DateTime.Now.ToString("yyyyMMdd"));
    //    Directory.CreateDirectory(folderPath);
    //    return folderPath;
    //}

    // 获取文件名的 JSON 格式字符串
    //private static string GetFileNamesAsJSON(string folderPath)
    //{
    //    var fileNames = Directory.GetFiles(folderPath).Select(Path.GetFileName);
    //    return Newtonsoft.Json.JsonConvert.SerializeObject(fileNames);
    //}

    // 模拟保存消息到文件的方法


}
