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
    private const string BaseFolderName4dlyrptPart = "../../../db/dlyrpt";


    private static HashSet<string> GetKeysAsHashSet(SortedList sortedList)
    {
        // 使用 HashSet 来存储并返回键集合
        return new HashSet<string>(sortedList.Keys.Cast<string>());
    }
    private static string GetListKeys(SortedList sortedList)
    {
      //  sortedList.Keys
        HashSet<string> keys = new HashSet<string>();
        // 使用 StringBuilder 来提高性能
        var sb = new System.Text.StringBuilder();

        foreach (DictionaryEntry entry in sortedList)
        {
            // 获取键并追加到 StringBuilder
            var key = entry.Key.ToString();
            sb.Append(key).Append(' ');
        }

        // 移除末尾多余的空格
        if (sb.Length > 0)
        {
            sb.Length--;  // 移除最后一个空格
        }

        return sb.ToString();
    }

    private static string ConcatenateKeysWithSpace(SortedList sortedList)
    {
        // 使用 StringBuilder 来提高性能
        var sb = new System.Text.StringBuilder();

        foreach (DictionaryEntry entry in sortedList)
        {
            // 获取键并追加到 StringBuilder
            var key = entry.Key.ToString();
            sb.Append(key).Append(' ');
        }

        // 移除末尾多余的空格
        if (sb.Length > 0)
        {
            sb.Length--;  // 移除最后一个空格
        }

        return sb.ToString();
    }


    private static string FormatSortedListToMarkdown(SortedList sortedList)
    {
        var sb = new StringBuilder();

        // 添加表头
        sb.AppendLine("| uid      | name     | demo|");
        sb.AppendLine("|----------|-----------|--------|");

        // 遍历 SortedList 并添加到 Markdown 表格
        foreach (DictionaryEntry entry in sortedList)
        {
            string key = (string)entry.Key;
            string value = (string)entry.Value;
            sb.AppendLine($"| {key,-8} | {value,-9} | |");
        }

        return sb.ToString();
    }
    public static async Task Main(string[] args)
    {
        string folderPath = $"{prjdir}/db/dlyrpt0812";
        string mkd2console = GetRptToday(folderPath);
        Print(mkd2console);

        //     string req = "st finished HTTP/1.0 GET http://lianxin.co/api/getlist?page=1&pagesize=20&%E5%9B%AD%E5%8C%BA=&%E5%9B%BD%E5%AE%B6=%E5%8D%B0%E5%BA%A6%E5%B0%BC%E8%A5%BF%E4%BA%9A,%E6%B3%B0%E5%9B%BD,%E7%BC%85%E7%94%B8&%E5%9F%8E%E5%B8%82=";
        Evtboot(() =>
        {
            //   botClient = botClient;



        });
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
        LoopForever();
    }

    private static string GetRptToday(string folderPath)
    {
        string alreadySendUsers = GetFileNamesAsJSONFrmFldr(folderPath);
        SortedList inimap = NewSortedListFrmIni($"{prjdir}/cfg/mem.ini");
        HashSet<string> ids = GetKeysAsHashSet(inimap);
        //ConcatenateKeysWithSpace(inimap);
        //     search usrs  where uid not in files
        HashSet<string> noExistUid = SubtractFrmFilist(ids, alreadySendUsers);
        SortedList noRptUsers = FltWhrKeyIn(inimap, noExistUid);
        string mkd = FormatSortedListToMarkdown(noRptUsers);
        Print(mkd);

        string mkd2console =
             FormatAndPrintMarkdownTable(mkd);
        return mkd2console;
    }

    private static SortedList FltWhrKeyIn(SortedList inimap, HashSet<string> noExistUid)
    {
        // 创建新的 SortedList 来存储过滤后的键值对
        var filteredList = new SortedList();

        // 遍历 inimap 的所有键
        foreach (DictionaryEntry entry in inimap)
        {
            string key = (string)entry.Key;

            // 如果键在 noExistUid 中，添加到新的 SortedList
            if (noExistUid.Contains(key))
            {
                filteredList.Add(key, entry.Value);
            }
        }

        return filteredList;
    }

    private static HashSet<string> SubtractFrmFilist(HashSet<string> ids, string jsonArray)
    {
        // 将 JSON 数组字符串转换为 List<string>
        var userList = JsonConvert.DeserializeObject<List<string>>(jsonArray);
        // 创建一个新的列表来存储过滤后的结果
        var filteredList_ids = new HashSet<string>();
       // string[] a = ids.Split(" ");

        foreach (var flstr in userList)
        {
            foreach (string id in ids)
            {
                // 检查用户是否包含指定字符串
                if (flstr.Contains(id))
                {
                    // 不包含指定字符串的用户添加到过滤列表
                    filteredList_ids.Add(id);
                }
            }

        }
        return Subtract(ids, filteredList_ids);

    }
    private static string SubtractFrmFilist(string ids, string jsonArray)
    {
        // 将 JSON 数组字符串转换为 List<string>
        var userList = JsonConvert.DeserializeObject<List<string>>(jsonArray);
        // 创建一个新的列表来存储过滤后的结果
        var filteredList_ids = new List<string>();
        string[] a = ids.Split(" ");

        foreach (var flstr in userList)
        {
            foreach(string id in a)
            {
                // 检查用户是否包含指定字符串
                if (flstr.Contains( ids))
                {
                    // 不包含指定字符串的用户添加到过滤列表
                    filteredList_ids.Add(id);
                }
            }
            
        }
        return Subtract(ids, filteredList_ids);

    }
    private static HashSet<string> Subtract(HashSet<string> ids, HashSet<string> filteredList_ids)
    {
        return Difference<string>(ids, filteredList_ids);
    }
    private static HashSet<T> Difference<T>(HashSet<T> set1, HashSet<T> set2)
    {
        // 创建 set1 的副本以保持原始集合不变
        var resultSet = new HashSet<T>(set1);

        // 从 resultSet 中移除 set2 中的元素
        resultSet.ExceptWith(set2);

        return resultSet;
    }
    private static string Subtract(string ids, List<string> filteredList_ids)
    {
        // 将空格分割的 ids 字符串转换为列表
        var idList = ids.Split(' ').ToList();

        // 创建一个 HashSet 来加快过滤操作
        var filteredSet = new HashSet<string>(filteredList_ids);

        // 过滤掉在 filteredListIds 中的 ID
        var resultList = idList.Where(id => !filteredSet.Contains(id)).ToList();

        // 将结果列表转换为以空格分隔的字符串
        return string.Join(" ", resultList);
    }

    private static bool IsContainsAny(string flstr, string ids)
    {
        throw new NotImplementedException();
    }

    private static SortedList NewSortedListFrmIni(string v)
    {
        return GetSortedlistFrmIni(v);
    }

    private static SortedList GetSortedlistFrmIni(string filePath)
    {
        var sortedList = new SortedList();

        if (!System.IO.File.Exists(filePath))
        {
            throw new FileNotFoundException("INI file not found", filePath);
        }

        var lines = System.IO.File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            // 跳过空行和注释行
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";"))
            {
                continue;
            }

            // 处理键值对
            var keyValue = trimmedLine.Split('=', 2);
            if (keyValue.Length == 2)
            {
                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();

                sortedList[key] = value;
            }
        }

        return sortedList;
    }

    // 定时任务调度器
    static void ScheduleDailyTask(int hour, int minute, Delegate task)
    {
        var __METHOD__ = nameof(ScheduleDailyTask);
        NewThrd(() =>
        {

            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(hour, minute, task.Method.Name));

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static string GetTodayCodeMnsHrs(int hoursToSubtract)
    {
        // 获取当前时间
        DateTime currentTime = DateTime.Now;

        // 从当前时间减去指定的小时数
        DateTime newTime = currentTime.AddHours(-hoursToSubtract);

        // 格式化为 "yyyymmdd" 并返回
        return newTime.ToString("MMdd");
    }
    public static async Task SumupDailyRpt()
    {
        try
        {
            // 创建 Telegram Bot 客户端
            var botClient = new TelegramBotClient(tokenbot);

            // 准备消息内容
            string messageContent = "日报小助手统计";
            string folderPath = BaseFolderName4dlyrptPart + GetTodayCodeMnsHrs(8);
            string alreadySendUsers = GetFileNamesAsJSONFrmFldr(folderPath);
            messageContent = $"{messageContent}\n目前已经发送的如下：\n{alreadySendUsers}";

            // 发送消息到指定聊天
            await botClient.SendTextMessageAsync(
                chatId: chatID,
                text: messageContent
            );


            string mkd2console = GetRptToday(folderPath);
            messageContent = $"目前还没有发送的人员如下:\n" + mkd2console;
            // 发送消息到指定聊天
            await botClient.SendTextMessageAsync(
                chatId: chatID,
                text: messageContent
            );

            Console.WriteLine("Message sent successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send message: {ex.Message}");
        }
    }


    public static async Task SendMessage4DailyRpt()
    {
        try
        {
            // 创建 Telegram Bot 客户端
            var botClient = new TelegramBotClient(tokenbot);

            // 准备消息内容
            string messageContent = "日报小助手提醒啦：没有发日报的请及时发日报，已发的忽略";
            string folderPath = CreateFolderBasedOnDate(BaseFolderName4dlyrptPart);
            string alreadySendUsers = GetFileNamesAsJSONFrmFldr(folderPath);
            messageContent = $"{messageContent}\n目前已经发送的如下：\n{alreadySendUsers}";

            // 发送消息到指定聊天
            await botClient.SendTextMessageAsync(
                chatId: chatID,
                text: messageContent
            );

            Console.WriteLine("Message sent successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send message: {ex.Message}");
        }
    }

    // 创建基于日期的文件夹
    public static string CreateFolderBasedOnDate(string baseFolderName)
    {
        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseFolderName + DateTime.Now.ToString("MMdd"));
        Directory.CreateDirectory(folderPath);
        return folderPath;
    }

    // 从文件夹中获取文件名并以 JSON 格式返回

    private static void RcvMsgStart()
    {

        // 获取真实路径并打印
        string realPath = GetRealPath($"{prjdir}/cfg/cfg.ini");
        Console.WriteLine("GetRealPath=>" + realPath);

        // 从配置文件读取字典
        Dictionary<string, string> config = GetDicFromIni($"{prjdir}/cfg/cfg.ini");

        // 如果文件不存在，使用指定路径读取配置文件
        //if (!System.IO.File.Exists("cfg117.ini"))
        //{
        //    Console.WriteLine(@"GetDicFromIni D:\0prj\mdsj\codelib2023\cfg117.ini");
        //    config = GetDicFromIni(@"D:\0prj\mdsj\codelib2023\cfg117.ini");
        //}

        // 从配置中获取 Telegram 机器人令牌
        tokenbot = config["token"];
        chatID = config["chatID"];
        botClient = new TelegramBotClient(tokenbot);
        // 发送消息
        SendMessage("start...");

        //todo here should wrt to rot ini therre..not here
        获取机器人的信息();
        botClient.StartReceiving(updateHandler: EvtUpdateHdlrAsyncSafe,
                  pollingErrorHandler: tglib.bot_pollingErrorHandler,
                  receiverOptions: new ReceiverOptions()
                  {
                      AllowedUpdates = System.Array.Empty<UpdateType>(),
                      // 接收所有类型的更新
                      //AllowedUpdates = [UpdateType.Message,
                      //    UpdateType.CallbackQuery,
                      //    UpdateType.ChannelPost,
                      //    UpdateType.MyChatMember,
                      //    UpdateType.ChatMember,
                      //    UpdateType.ChatJoinRequest],
                      ThrowPendingUpdates = true,
                  });


    }
    static async System.Threading.Tasks.Task EvtUpdateHdlrAsyncSafe(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {


        //  throw new Exception("myex");
        //   call_user_func(evt_aHandleUpdateAsync, botClient, update, cancellationToken, reqThreadId)

        //try todo map evt
        callAsyncNewThrdx(() =>
        {
            try
            {
                string reqThreadId = geneReqid();
                EvtUpdateHdlr(botClient, update, cancellationToken, reqThreadId);
                //     throw new InvalidOperationException("An error occurred in the task.");

            }
            catch (jmp2endEx e22)
            {
                Print("jmp2exitEx"); Print(e22.Message); ;
            }
            catch (Exception e)
            {
                logErr2024(e, "evt_aHandleUpdateAsyncSafe", "errlogDir", null);
            }
            return 0;


        });
        //     Task.Run(async );
        //  int reqThreadId = Thread.CurrentThread.ManagedThreadId;


    }

    //收到消息时执行的方法
    static void EvtUpdateHdlr(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string reqThreadId)
    {
        //  throw new Exception("myex");

        var __METHOD__ = nameof(EvtUpdateHdlr);
        PrintCallFunArgs(__METHOD__, func_get_args(update));
        logCls.log("fun " + __METHOD__, func_get_args(update), null, "logDir", reqThreadId);
        Print(update?.Message?.Text);
        //    tts(update?.Message?.Text);
        // print(json_encode(update));
        Print("tag4520");



        CallAsyncNewThrd(() =>
        {
            Thread.Sleep(1500);
            bot_logRcvMsg(update);
            Thread.Sleep(6000);
            dbgpad = 0;
            //todo save chat sess
        });

        UpdtHdlr(update);

        if (update.Message != null && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
        {
            MsgHdlr(update);
        }


    }

    static async Task RcvMsgStartPullover()
    {
        botClient = new TelegramBotClient(tokenbot);

        // 设置更新偏移量
        int offset = 0;

        while (true)
        {
            try
            {
                // 获取更新
                var updates = await botClient.GetUpdatesAsync(offset);

                foreach (var update in updates)
                {
                    // 更新偏移量
                    offset = update.Id + 1;

                    UpdtHdlr(update);

                    if (update.Message != null && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                    {
                        await MsgHdlr(update);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // 等待 1 秒钟再进行下一次轮询
            await Task.Delay(1000);
        }
    }

    private static void UpdtHdlr(Update update)
    {
        //  throw new NotImplementedException();
    }

    private static async Task MsgHdlr(Update update)
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
        if (message.Text.Contains("今日工作内容"))
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
            string messageContent = $"接受到日报消息.\n目前已经发送的人员如下：\n{alreadySendUsers}";

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

    private static void SaveMessageToFile4DlyrptUidFldMd(Message message)
    {
        string uid = message.From?.Id.ToString();
        // 实际实现保存消息到文件
        string baseFolderName = ($"../../../db/dlyrptUid{uid}");
        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseFolderName);
        Directory.CreateDirectory(folderPath);

        // 创建 dlyrpt 文件夹（如果不存在）
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 序列化消息为 JSON
        string jsonData;
        try
        {
            jsonData = System.Text.Json.JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"JSON 序列化失败: {ex.Message}", ex);
        }

        // 获取发送人的用户名
        string username = message.From?.Username ?? "unknown";

        string timecode = DateTime.Now.ToString("MMdd");
        string fname = $"{timecode} {uid} uname({username}) frstLastname({message.From?.FirstName} {message.From?.LastName})";
        //  +DateTime.Now.ToString("yyyyMMdd")
        // 确定文件路径
        string fileName = ConvertToValidFileName(fname);
        string filePath = Path.Combine(folderPath, fileName + ".json");

        // 保存 JSON 文件
        try
        {
            System.IO.File.WriteAllTextAsync(filePath, jsonData);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"无法写入文件: {ex.Message}", ex);
        }
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
    private static void SaveMessageToFile4Dlyrpt(Message message)
    {
        // 实际实现保存消息到文件
        string folderPath = (BaseFolderName4dlyrptPart + GetTodayCode());

        // 创建 dlyrpt 文件夹（如果不存在）
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 序列化消息为 JSON
        string jsonData;
        try
        {
            jsonData = System.Text.Json.JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"JSON 序列化失败: {ex.Message}", ex);
        }

        // 获取发送人的用户名
        string username = message.From?.Username ?? "unknown";
        string uid = message.From?.Id.ToString();
        string timecode = DateTime.Now.ToString("MMdd");
        string uname = $"{timecode} {uid} uname({username}) frstLastname({message.From?.FirstName} {message.From?.LastName})";

        // 确定文件路径
        string fileName = ConvertToValidFileName(uname);
        string filePath = Path.Combine(folderPath, fileName + ".json");

        // 保存 JSON 文件
        try
        {
            System.IO.File.WriteAllTextAsync(filePath, jsonData);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"无法写入文件: {ex.Message}", ex);
        }
    }

    private static string GetTodayCode()
    {
        return GetTodayCodeMnsHrs(6);
    }
}
