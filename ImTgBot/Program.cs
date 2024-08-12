using Newtonsoft.Json;
using prjx.libx;
using prjx.libx;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private const string BaseFolderName4dlyrptPart = "../../../db/dlyrpt";

    public static async Task Main(string[] args)
    {
        Evtboot(() =>
        {
            //   botClient = botClient;



        });
        RcvMsgStart();

        // 设置每天 5:30 执行任务
        ScheduleDailyTask(11, 36,  SendMessage4DailyRpt);//test


        ScheduleDailyTask(17, 59, SendMessage4DailyRpt);
        ScheduleDailyTask(18, 30, SendMessage4DailyRpt);
        ScheduleDailyTask(20, 30, SendMessage4DailyRpt);
        ScheduleDailyTask(11, 00, SendMessage4DailyRpt);

        ScheduleDailyTask(3, 00, SumupDailyRpt);

        LoopForever();
    }

    // 定时任务调度器
    static   void ScheduleDailyTask(int hour, int minute, Delegate task)
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
    public static string GetTodayCodeMnsHrs(int  hoursToSubtract)
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
            string folderPath = BaseFolderName4dlyrptPart+ GetTodayCodeMnsHrs(4);
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


    public static async Task SendMessage4DailyRpt()
    {
        try
        {
            // 创建 Telegram Bot 客户端
            var botClient = new TelegramBotClient(tokenbot);

            // 准备消息内容
            string messageContent = "日报小助手提醒啦：没有发日报的请及时发日报，已发的忽略";
            string folderPath = CreateFolderBasedOnDate(BaseFolderName4dlyrptPart);
            string alreadySendUsers =   GetFileNamesAsJSONFrmFldr(folderPath);
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
        LogRcvMsgAsync(update,"MsgDir");

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
            string folderPath =  (BaseFolderName4dlyrptPart+ GetTodayCode());
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
        string folderPath = (BaseFolderName4dlyrptPart+GetTodayCode());

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
        return GetTodayCodeMnsHrs(4);
    }
}
