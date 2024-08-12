global using static InlineKeyboardHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;


public static class InlineKeyboardHelper
{
    //  InlineKeyboardMarkup inlineKeyboardMarkup = InlineKeyboardHelper.ConvertJsonToInlineKeyboardMarkup(json);

    public static InlineKeyboardMarkup ConvertJsonToInlineKeyboardMarkup(string json)
    {

        var inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();


        var inlineKeyboardData = JsonConvert.DeserializeObject<InlineKeyboardData>(json);
        

        foreach (var buttonRowInJson in inlineKeyboardData.InlineKeyboard)
        {
            var buttonList_RowInTg = new List<InlineKeyboardButton>();
            foreach (var button in buttonRowInJson)
            {
                if (!string.IsNullOrEmpty(button.CallbackData))
                {
                    buttonList_RowInTg.Add(InlineKeyboardButton.WithCallbackData(button.Text, button.CallbackData));
                }
                else if (!string.IsNullOrEmpty(button.Url))
                {
                    buttonList_RowInTg.Add(InlineKeyboardButton.WithUrl(button.Text, button.Url));
                }
            }
            inlineKeyboardButtons.Add(buttonList_RowInTg);
        }

        return new InlineKeyboardMarkup(inlineKeyboardButtons);
    }

    private class InlineKeyboardData
    {
        [JsonProperty("inline_keyboard")]
        public List<List<ButtonData>> InlineKeyboard { get; set; }
    }

    private class ButtonData
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("callback_data")]
        public string CallbackData { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}