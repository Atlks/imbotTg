global using static prjx.libx.menuMng;
using HtmlAgilityPack;
using Newtonsoft.Json;
using prjx;
using prjx.libx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace prjx.libx
{
    internal class menuMng
    {

        public static void evt_btm_menuitem_clickV2(long? chat_id, string imgPath, string msgtxt, IReplyMarkup rplyKbdMkp, Update? update)
        {
            // [CallerMemberName] string methodName = ""
            //  CallerMemberName 只能获取上一级的调用方法，不能本级别的，只好手工赋值了
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            //  __METHOD__ = methodName;
            __METHOD__ = "evt_menuitem_click";  //bcs in task so cant get currentmethod
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), chat_id, rplyKbdMkp));

            //  Program.botClient.send
            try
            {
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                //   Message message2 =   await Program.botClient.EditMessageReplyMarkupAsync(chat_id,(int)update?.Message?.MessageId, rplyKbdMkp);
                Message message2 = botClient.SendTextMessageAsync(
                chat_id, msgtxt,
                    parseMode: ParseMode.Html,
                   replyMarkup: rplyKbdMkp,
                   protectContent: false, disableWebPagePreview: true
                   , replyToMessageId: update.Message.MessageId
                   ).Result;
                //  Program.botClient.SendTextMessageAsync()
               Print(JsonConvert.SerializeObject(message2));



            }
            catch (Exception ex) {Print(ex.ToString()); }

            dbgCls.PrintRet(__METHOD__, 0);



        }
        public static void evt_btm_menuitem_click(long? chat_id, string imgPath, string msgtxt, InlineKeyboardMarkup rplyKbdMkp, Update? update)
        {
            // [CallerMemberName] string methodName = ""
            //  CallerMemberName 只能获取上一级的调用方法，不能本级别的，只好手工赋值了
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            //  __METHOD__ = methodName;
            __METHOD__ = "evt_menuitem_click";  //bcs in task so cant get currentmethod
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), chat_id, rplyKbdMkp));

     //       rplyKbdMkp.ResizeKeyboard = true;
            //  Program.botClient.send
            try
            {
                
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                //   Message message2 =   await Program.botClient.EditMessageReplyMarkupAsync(chat_id,(int)update?.Message?.MessageId, rplyKbdMkp);
                Message message2 =  botClient.SendTextMessageAsync(                    
                chat_id, msgtxt,
                    parseMode: ParseMode.Html,
                   replyMarkup: rplyKbdMkp,
                   protectContent: false, disableWebPagePreview: true).Result;
               Print(JsonConvert.SerializeObject(message2));



            }
            catch (Exception ex) {Print(ex.ToString()); }

            dbgCls.PrintRet(__METHOD__, 0);



        }

        public static InlineKeyboardButton[][] wdsFromFileRendrToBtnmenu(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<InlineKeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<InlineKeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        // { CallbackData = $"Merchant?id={guid}" }
                        InlineKeyboardButton btn = new InlineKeyboardButton(word) { CallbackData = $"cmd={word}" };
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }

        public static KeyboardButton[][] wdsFromFileRendrToTgBtmBtnmenuBycomma(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<KeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        KeyboardButton btn = new KeyboardButton(word);
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }

        public static KeyboardButton[][] wdsFromFileRendrToTgBtmBtnmenu(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<KeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        KeyboardButton btn = new KeyboardButton(word);
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }
        public static InlineKeyboardButton[][] ConvertHtmlLinksToTelegramButtonsV1001(string filePath)
        {
            // 读取HTML文件内容
            var htmlContent = System.IO.File.ReadAllText(filePath);

            // 使用HtmlAgilityPack解析HTML内容
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // 查找所有的<a>标签
            var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes == null || !linkNodes.Any())
            {
                return Array.Empty<InlineKeyboardButton[]>();
            }

            // 将每个<a>标签转换为InlineKeyboardButton
            var buttons = linkNodes
                .Select(linkNode =>
                {
                    var url = linkNode.GetAttributeValue("href", string.Empty);
                    var text = linkNode.InnerText;
                    return InlineKeyboardButton.WithUrl(text, url);
                })
                .ToList();

            // 将按钮组织成一个二维数组，这里假设每行只有一个按钮
            //var buttonRows = buttons
            //    .Select(button => new[] { button })
            //    .ToArray();

            // 将按钮组织成一个二维数组，每行3个按钮
            var buttonRows = buttons
                .Select((button, index) => new { button, index })
                .GroupBy(x => x.index / 3)
                .Select(g => g.Select(x => x.button).ToArray())
                .ToArray();
            return buttonRows;
        }

        public static InlineKeyboardButton[] ConvertHtmlLinksToTelegramButtonsSnglRow(string filePath)
        {
            // 读取HTML文件内容
            var htmlContent = System.IO.File.ReadAllText(filePath);

            // 使用HtmlAgilityPack解析HTML内容
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // 查找所有的<a>标签
            var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes == null || !linkNodes.Any())
            {
                return Array.Empty<InlineKeyboardButton>();
            }

            // 将每个<a>标签转换为InlineKeyboardButton
            var buttons = linkNodes
                .Select(linkNode =>
                {
                    var url = linkNode.GetAttributeValue("href", string.Empty);
                    var text = linkNode.InnerText;
                    return InlineKeyboardButton.WithUrl(text, url);
                })
                .ToArray();

            return buttons;
        }


    }
}
