using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class OpenAIResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public int Created { get; set; }
    public string Model { get; set; }
    public Choice[] Choices { get; set; }
    public Usage Usage { get; set; }
}

public class Choice
{
    public string Text { get; set; }
    public int Index { get; set; }
    public object Logprobs { get; set; }
    public string Finish_reason { get; set; }
}

public class Usage
{
    public int Prompt_tokens { get; set; }
    public int Completion_tokens { get; set; }
    public int Total_tokens { get; set; }
}

public class ChatGPTClient
{
    private readonly string _apiKey;

    public ChatGPTClient(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> GetChatGPTResponse(string question)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                 
              //  model = "text-davinci-003", // 使用适当的模型名称
                prompt = question,
                max_tokens = 100 // 根据需要调整
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var openAIResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseString);
                return openAIResponse.Choices[0].Text.Trim();
            }
            else
            {
                throw new Exception($"Error: {responseString}");
            }
        }
    }
}

class Program2024
{
    static async Task Main2(string[] args)
    {
        string apiKey = "sk-proj-N2Fq9Z6KNZ7xx98ssXshT3BlbkFJ2HyoaRNCbxEkQtYcGOu6";
        string question = "世界一共多少个城市";

        string answer = await srchByChtgpt(apiKey, question);
        try
        {

           Print($"ChatGPT的回答: {answer}");
        }
        catch (Exception ex)
        {
           Print($"请求出错: {ex.Message}");
        }
    }

    public static async Task<string> srchByChtgpt(string apiKey, string question)
    {
        try
        {
            var chatGptClient = new ChatGPTClient(apiKey);

           Print("请输入你的问题:" + question);

            string answer = await chatGptClient.GetChatGPTResponse(question);
            return answer;
        }catch (Exception ex)
        {
           Print(ex.Message);
            return "";
        }
       
    }
}
