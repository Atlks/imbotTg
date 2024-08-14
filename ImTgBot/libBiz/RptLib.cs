global using static libBiz.RptLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace libBiz
{
    internal class RptLib
    {
        public const string BaseFolderName4dlyrptPart = "../../../db/dlyrpt";

        public static void RptConsecutiveMissingDays()
        {
            SortedList inimap = NewSortedListFrmIni($"{prjdir}/cfg/mem.ini");
            HashSet<string> ids = GetKeysAsHashSet(inimap);
            List<SortedList> li = new List<SortedList>();
            foreach (string id in ids)
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
                o.Add("连续缺失天数", missdays);

                li.Add(o);
            }
            Print(EncodeJsonFmt(li));

            string title = "|uid|连续缺失天数|name|\n|-----|-----|---|\n";
            Hashtable tmpltMkdwn = new Hashtable();
            tmpltMkdwn.Add(render_title_table, title);
            tmpltMkdwn.Add(render_rowRender, (SortedList row) =>
            {
                return "|☀️" + row["uid"].ToString() + "|" + row["连续缺失天数"].ToString() + "|" + row["name"].ToString() + "|";
            });

            string mkdwn2 = RenderTable(li, tmpltMkdwn);

            //string mkdwn = FormatListToMarkdown(li, title, (row) =>
            //        {
            //            return "|" + row["uid"].ToString() + "|" + row["连续缺失天数"].ToString() + "|" + row["name"].ToString() + "|";
            //        });
            Print(mkdwn2);

            string mkd2console = FormatAndPrintMarkdownTable(mkdwn2);
            Print(mkd2console);

            string messageContent = $"连续缺失日报的统计如下:\n" + mkd2console;
            // 发送消息到指定聊天
            Sendmsg(chatID, messageContent);

        }

        public static void Sendmsg(string chatID, string messageContent)
        {
            try
            {
                botClient.SendTextMessageAsync(
                               chatId: chatID,
                               text: messageContent
                           ).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                PrintCatchEx("sendmgs", e);
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
                // 创建 Telegram Bot 客户端
                var botClient = new TelegramBotClient(tokenbot);

                // 准备消息内容

                string dt = GetTodayCodeMnsHrs(8);
                string messageContent = "日报小助手统计" + dt;
                string folderPath = BaseFolderName4dlyrptPart + dt;
                string alreadySendUsers = GetFileNamesAsJSONFrmFldr(folderPath);
                //     messageContent = $"{messageContent}\n目前已经发送的如下：\n{alreadySendUsers}";

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

        /// <summary>
        ///   Render(tmple(title,rowRender)   ,dataList)
        /// </summary>
        /// <param name="sortedList"></param>
        /// <param name="title"></param>
        /// <param name="rendRowFun"></param>
        /// <returns></returns>
        public static string FormatListToMarkdown(List<SortedList> sortedList, string title, Func<SortedList, string> rendRowFun)
        {
            var sb = new StringBuilder();

            // 添加表头
            //  sb.AppendLine("| uid      | name     | demo|");
            sb.Append(title);

            //    sb.AppendLine("|----------|-----------|--------|");

            foreach (SortedList row in sortedList)
            {
                sb.AppendLine(rendRowFun(row));
            }
            // 遍历 SortedList 并添加到 Markdown 表格
            //foreach (DictionaryEntry entry in sortedList)
            //{
            //    string key = (string)entry.Key;
            //    string value = (string)entry.Value;

            //}

            return sb.ToString();
        }

        public static string FormatSortedListToMarkdown(SortedList sortedList)
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

        // 创建基于日期的文件夹
        public static string CreateFolderBasedOnDate(string baseFolderName)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseFolderName + DateTime.Now.ToString("MMdd"));
            Directory.CreateDirectory(folderPath);
            return folderPath;
        }
    }
}
