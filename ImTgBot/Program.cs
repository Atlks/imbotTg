global using static Program;
using Newtonsoft.Json;
using prjx.libx;
using prjx.libx;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
        //string folderPath = $"{prjdir}/db/dlyrpt0812";
        //string mkd2console = GetRptToday(folderPath);
        //Print(mkd2console);

        Main2024(() =>
        {

            RcvMsgStart();

            // 设置每天 5:30 执行任务
            if (IsExistFil("c:/teststart.txt"))
                ScheduleDailyTask(11, 36, SendMessage4DailyRpt);//test


            ScheduleDailyTask(18, 55, SendMessage4DailyRpt);
            ScheduleDailyTask(20, 30, SendMessage4DailyRpt);
            ScheduleDailyTask(22, 30, SendMessage4DailyRpt);
            ScheduleDailyTask(23, 55, SendMessage4DailyRpt);

            ScheduleDailyTask(4, 00, SumupDailyRpt);

            if (IsExistFil("c:/teststart.txt"))
                ScheduleDailyTask(15, 40, SumupDailyRpt);//test
        });

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

        //------------------------today wk rpt
        // 检查消息内容是否包含 "今日工作内容"
        if (message.Text.Contains("今日工作亮点"))
        {
            try
            {
                SaveMessageToFile4Dlyrpt(message);


                Console.WriteLine("消息已保存");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }
            try
            {
                //double wrt to   uidFldMode
                SaveMessageToFile4DlyrptUidFldMd(message);
                Console.WriteLine("消息已保存 SaveMessageToFile4DlyrptUidFldMd");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }


            //------------------- 创建要发送的回复消息
            string folderPath = (BaseFolderName4dlyrptPart + GetTodayCode());
            string alreadySendUsers = GetFileNamesAsJSONFrmFldr(folderPath);
            string messageContent = $"接受到日报消息";
            //    $".\n目前已经发送的人员如下：\n{alreadySendUsers}";

            var reply = new Telegram.Bot.Types.Message
            {
                Chat = new Chat { Id = message.Chat.Id },
                Text = messageContent,
                // ReplyToMessageId = message.MessageId
            };

            // 发送回复
            try
            {
                await botClient.SendTextMessageAsync(reply.Chat.Id, messageContent, replyToMessageId: message.MessageId);
                Console.WriteLine("消息已发送");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send message: {ex.Message}");
            }
        }


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
