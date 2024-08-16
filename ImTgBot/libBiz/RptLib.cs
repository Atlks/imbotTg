global using static libBiz.RptLib;
using HandlebarsDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace libBiz
{
    internal class RptLib
    {
        public const string BaseFolderName4dlyrptPart = "../../../db/dlyrpt";

        public static void RptConsecutiveMissingDays()
        {
            weekendChk();
            SortedList inimap = NewSortedListFrmIni($"{prjdir}/cfg/mem.ini");
            HashSet<string> ids = GetKeysAsHashSet(inimap);
            List<SortedList> li = new List<SortedList>();
            foreach (string id in ids)
            {
                try
                {
                    if (id == "6436991688")
                        Print("dbf202");
                    string dbfld = $"{prjdir}/db/dlyrptUid" + id;
                    string todaycode = GetTodayCode();
                    if (IsExistFilNameStartWz(todaycode, dbfld))
                        continue;

                    //有确实的了consct miss
                    int missdays = clalcMissdays(todaycode, dbfld);
                    SortedList o = new SortedList();
                    o.Add("uid", id);
                    o.Add("name", inimap[id]);
                    o.Add("连续缺失天数", "🔥" + missdays);

                    li.Add(o);
                }
                catch (Exception e)
                {
                    PrintExcept("RptConsecutiveMissingDays.forBlk", e);
                }

            }
            Print(EncodeJsonFmt(li));

            //    rendTest(li);

            //=---------------rendTable to mkd

            Hashtable tmpltMkdwn = new Hashtable();
            tmpltMkdwn.Add(render_title_table, "| 连续缺失天数 | uname | uid|");

            tmpltMkdwn.Add(render_rowRender, (SortedList row) =>
            {
                return "|" + row["连续缺失天数"].ToString() + "|" + row["name"].ToString() + "|" + row["uid"].ToString() + "|";
            });

            string mkdwn2tbl = RenderTable(li, tmpltMkdwn);
            //  string mkdwn2 = ConvertToMarkdown(li);
            Print(mkdwn2tbl);


            //-----------rend to consle
            string mkd2consoleTable = FormatAndPrintMarkdownTable(mkdwn2tbl);
            Print(mkd2consoleTable);


            //------------rend to tmplt
            Hashtable data = new Hashtable();
            data["tb1142"] = mkd2consoleTable;
            data["dt"] = GetTodayCode();


            var tmpltf = $"{prjdir}/cfg/csctv_lyesyvMiss_tmplt.md";
            string messageContent = RendTmpltMD(data, tmpltf);
            Print(messageContent);
            // 发送消息到指定聊天
            Sendmsg(chatID, messageContent);

        }





        public static void RptMonth()
        {
            SortedList inimap = NewSortedListFrmIni($"{prjdir}/cfg/mem.ini");
            HashSet<string> ids = GetKeysAsHashSet(inimap);
            List<SortedList> li = new List<SortedList>();
            string month = "";
            foreach (string id in ids)
            {
                if (id == "6436991688")
                    Print("dbf202");
                string dbfld = $"{prjdir}/db/dlyrptUid" + id;
                string todaycode = GetTodayCode();
                //if (IsExistFilNameStartWz(todaycode, dbfld))
                //    continue;
                month = "2024" + Left(todaycode, 2);
                string yyyymmdd = "2024" + todaycode;

                //有确实的了consct miss
                int missdays = 0;
                try
                {
                    int maxday = GetMaxDaysOfMonth(month);
                    missdays = calcCountByMonthByUid(month, dbfld);
                }
                catch (Exception e)
                {
                    Print(e);
                }

                if (missdays > 0)
                {
                    SortedList o = new SortedList();
                    //    o.Add("已发天数", missdays);
                    o.Add("缺失天数", missdays);
                    o.Add("id", id);
                    o.Add("name", inimap[id]);
                    li.Add(o);
                }

            }
            Print(EncodeJsonFmt(li));

            //string title = $"|uid|缺失天数|name|\n|-----|-----|---|\n";
            //Hashtable tmpltMkdwn = new Hashtable();
            //tmpltMkdwn.Add(render_title_table, title);
            //tmpltMkdwn.Add(render_rowRender, (SortedList row) =>
            //{
            //    return "|" + row["uid"].ToString() + "|" + row["缺失天数"].ToString() + "|" + row["name"].ToString() + "|";
            //});

            //string mkdwn2 = RenderTable(li, tmpltMkdwn);

            //string mkdwn = FormatListToMarkdown(li, title, (row) =>
            //        {
            //            return "|" + row["uid"].ToString() + "|" + row["连续缺失天数"].ToString() + "|" + row["name"].ToString() + "|";
            //        });

            string mkdwn2 = ConvertToMarkdown(li);
            Print(mkdwn2);

            Print("--------------------------------");
            string mkd2console = FormatAndPrintMarkdownTable(mkdwn2);
            Print($"|{month}|\n" + mkd2console);


            //------------rend to tmplt
            Hashtable data = new Hashtable();
            data["tb1142"] = mkd2console;
            data["dt"] = month;
            data["foot"] = "====================";

            var tmpltf = $"{prjdir}/cfg/rpt_month_tmplt.md";
            string messageContent = RendTmpltMD(data, tmpltf);
            Print(messageContent);
            // 发送消息到指定聊天
            Sendmsg(chatID, messageContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthYYYYMM">202408</param>
        /// <param name="dbfld"></param>
        /// <returns></returns>
        public static int calcCountByMonthByUid(string monthYYYYMM, string dbfld)
        {
            int maxday = GetMaxDaysOfMonth(monthYYYYMM);
            int paddWkdays = 13;
            if (monthYYYYMM != "202408")
                paddWkdays = 0;
            string yymm = monthYYYYMM;
            int wkdays = GetWorkDaysExcludingSundays(yymm);

            string todaycode = GetTodayCode();

            string yyyymmdd = "2024" + todaycode;
            int daysLeft = DaysUntilEndOfMonth(yyyymmdd);
            int sunLeft = CountSundaysUntilEndOfMonth(yyyymmdd);
            int leftWkdays = daysLeft - sunLeft;
            if (!Directory.Exists(dbfld))
                return wkdays - paddWkdays - leftWkdays;
            var files = Directory.GetFiles(dbfld);
            int countDays = 0;

            DateTime curdate = ConvertToDateTime(monthYYYYMM + "01");

            for (int i = 1; i < maxday; i++)
            {
                string curdateCode = FmtDateMMDD(curdate);
                if (curdateCode == "0813")
                    Print("dg323");
                if (IsExistFilNameStartWz(curdateCode, dbfld))
                {
                    countDays++;

                }
                curdate = curdate.AddDays(1);


            }
            //  return countDays;
            //miss days
            return wkdays  - paddWkdays - leftWkdays-countDays;


        }

        public static int clalcMissdays(string todaycode, string dbfld)
        {
            try
            {
                if (!Directory.Exists(dbfld))
                    return 30;
                var files = Directory.GetFiles(dbfld);
                int missdays = 0;
                string yymmdd = "2024" + todaycode;
                DateTime curdate = ConvertToDateTime(yymmdd);
                for (int i = 0; i < 30; i++)
                {
                    string curdateCode = FmtDateMMDD(curdate);
                    if (!IsExistFilNameStartWz(curdateCode, dbfld))
                    {
                        missdays++;
                        // 当前时间减去一天
                        curdate = curdate.AddDays(-1);
                        //   curdate =
                    }
                    else
                        return missdays;

                }
                return missdays;
            }
            catch (Exception e)
            {
                PrintExcept("calcCountByMonthByUid", e);
                return 0;
            }



        }




        public static void SaveMessageToFile4Dlyrpt(Message message)
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

        public static void SaveMessageToFile4DlyrptUidFldMd(Message message)
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
        /// <summary>
        /// add dly rpt Msg hdlr
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task AddDlyrpt(Message? message)
        {
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
        }

        public static string GetTodayCode()
        {
            return GetTodayCodeMnsHrs(6);
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
                weekendChk();
                // 创建 Telegram Bot 客户端
                var botClient = new TelegramBotClient(tokenbot);

                // 准备消息内容

                string dt = GetTodayCodeMnsHrs(8);

                string folderPath = BaseFolderName4dlyrptPart + dt;
                string alreadySendUsers = GetFileNamesAsJSONFrmFldr(folderPath);
                //     messageContent = $"{messageContent}\n目前已经发送的如下：\n{alreadySendUsers}";

                // 发送消息到指定聊天
                //await botClient.SendTextMessageAsync(
                //    chatId: chatID,
                //    text: messageContent
                //);


                string mkd2console = GetRptToday(folderPath);


                var tmpltf = $"{prjdir}/cfg/rpt_today_tmplt.md";
                Hashtable ht = new Hashtable();
                ht["dt"] = dt;
                ht["tb1142"] = mkd2console;

                string messageContent = RendTmpltMD(ht, tmpltf);

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

        public static async Task SumupDailyRptBydate(string date, string chatID22)
        {
            try
            {
                // 创建 Telegram Bot 客户端
                var botClient = new TelegramBotClient(tokenbot);

                // 准备消息内容
                string messageContent = "日报小助手统计周期" + date;
                string folderPath = BaseFolderName4dlyrptPart + date;
                string alreadySendUsers = GetFileNamesAsJSONFrmFldr(folderPath);
                //      messageContent = $"{messageContent}\n目前已经发送的如下：\n{alreadySendUsers}";

                // 发送消息到指定聊天
                //await botClient.SendTextMessageAsync(
                //    chatId: chatID,
                //    text: messageContent
                //);


                string mkd2console = GetRptToday(folderPath);
                messageContent = $"{messageContent}\n目前还没有发送的人员如下:\n" +
                    $"\n" + mkd2console;
                // 发送消息到指定聊天
                await botClient.SendTextMessageAsync(
                    chatId: chatID22,
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
                weekendChk();
                // 创建 Telegram Bot 客户端
                var botClient = new TelegramBotClient(tokenbot);

                // 准备消息内容
                string messageContent = "日报小助手提醒啦：没有发日报的请及时发日报，已发的忽略";
                string folderPath = CreateFolderBasedOnDate(BaseFolderName4dlyrptPart);
                string alreadySendUsers = GetFileNamesAsJSONFrmFldr(folderPath);
                //    messageContent = $"{messageContent}\n目前已经发送的如下：\n{alreadySendUsers}";

                // 发送消息到指定聊天
                await botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: messageContent
                );

                Console.WriteLine("Message sent successfully");
                SumupDailyRpt();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send message: {ex.Message}");
            }
        }



        public static string GetRptToday(string folderPath)
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


        // 创建基于日期的文件夹
        public static string CreateFolderBasedOnDate(string baseFolderName)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseFolderName + DateTime.Now.ToString("MMdd"));
            Directory.CreateDirectory(folderPath);
            return folderPath;
        }
    }
}
