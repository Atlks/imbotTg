global using static prjx.libx.htmlCls;
using HtmlAgilityPack;
using prjx.libx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using TheArtOfDev.HtmlRenderer.WinForms;
//using HtmlRenderer.WinForms;


namespace prjx.libx
{
    internal class htmlCls
    {

      public  static void GenerateImageFromHtml(string htmlFilePath, string outputImagePath)
        {
            string wkhtmltoimagePath = @"D:\Program Files\wkhtmltopdf\bin\wkhtmltoimage.exe";

            // 确保 wkhtmltoimage.exe 存在
            if (!System.IO.File.Exists(wkhtmltoimagePath))
            {
                throw new FileNotFoundException("wkhtmltoimage.exe not found.", wkhtmltoimagePath);
            }

            // 创建进程启动信息
            var processStartInfo = new ProcessStartInfo
            {
                FileName = wkhtmltoimagePath,
                Arguments = $"\"{htmlFilePath}\" \"{outputImagePath}\"",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // 启动进程并等待完成
            using (var process = Process.Start(processStartInfo))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Failed to start process.");
                }

                // 读取标准错误输出（如果有）
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"wkhtmltoimage failed with exit code {process.ExitCode}: {error}");
                }
            }
        }
        //static void RenderHtmlToImage(string htmlFilePath, string outputImagePath)
        //{
        //    // 读取 HTML 文件内容
        //    string htmlContent = File.ReadAllText(htmlFilePath);

        //    // 使用 HtmlRender 渲染 HTML 内容到图像
        //    // 创建一个空的 Bitmap 图像
        //    using (var bitmap = new Bitmap(1000, 1000)) // 使用适当的尺寸
        //    using (var graphics = Graphics.FromImage(bitmap))
        //    {
        //        // 设置图像背景颜色
        //        graphics.Clear(Color.White);

        //        // 创建一个 Graphics 对象用于渲染 HTML
        //        var htmlRenderer = new HtmlRenderer.HtmlPanel
        //        {
        //            Html = htmlContent,
        //            AutoSize = true
        //        };

        //        // 计算 HTML 面板的大小
        //        var preferredSize = htmlRenderer.GetPreferredSize(new Size(1000, 1000));

        //        // 更新 Bitmap 尺寸
        //        bitmap = new Bitmap((int)preferredSize.Width, (int)preferredSize.Height);
        //        graphics = Graphics.FromImage(bitmap);

        //        // 绘制 HTML 内容到图像
        //        htmlRenderer.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

        //        // 保存图像为 JPG 文件
        //        bitmap.Save(outputImagePath, ImageFormat.Jpeg);
        //    }
        //}


        public static HashSet<string> ExtractWordsFromFilesHtml(string folderPath)
        {
            HashSet<string> words = new HashSet<string>();

            if (!Directory.Exists(folderPath))
            {
                Print(" ..warning.... fld not exist: fun ExtractWordsFromFilesHtml " + folderPath);

                return words;
            }

            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                try
                {
                    // 读取文件内容
                    string content = System.IO.File.ReadAllText(file);
                    content = htm_strip_tags(content);
                    // 提取单词
                    var extractedWords = ExtractWords(content);

                    // 将提取的单词添加到 HashSet 中
                    foreach (var word in extractedWords)
                    {
                        var a = word.Split("-");
                        foreach (var wd in a)
                        {
                            var a2 = wd.Split("_");
                            foreach (var w3 in a2)
                            {
                                // if (w3.Length > 3)
                                words.Add(w3);
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(ExtractWordsFromFilesHtml), e);
                }

            }

            return words;
        }

        public static HashSet<string> ExtractWordsFromFiles(string folderPath)
        {
            HashSet<string> words = new HashSet<string>();

            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                // 读取文件内容
                string content = System.IO.File.ReadAllText(file);

                // 提取单词
                var extractedWords = ExtractWords(content);

                // 将提取的单词添加到 HashSet 中
                foreach (var word in extractedWords)
                {
                    var a = word.Split("-");
                    foreach (var wd in a)
                    {
                        var a2 = wd.Split("_");
                        foreach (var w3 in a2)
                        {
                            if (w3.Length > 3)
                                words.Add(w3);
                        }
                    }

                }
            }

            return words;
        }

        public static IEnumerable<string> ExtractWords(string text)
        {
            // 使用正则表达式提取单词
            MatchCollection matches = Regex.Matches(text, @"\b\w+\b");

            foreach (Match match in matches)
            {
                yield return match.Value.ToLower(); // 转换为小写以确保唯一性
            }
        }

        public static void Download(string url, string dataDir)
        {// 生成一个新的 UUID
            Guid newUuid = Guid.NewGuid();
            var html = GetHtmlContent(url);
            WriteAllText($"{dataDir}/{newUuid}.htm", html);
        }
        public static HashSet<string> ExtractHrefAttributes(string html)
        {
            HashSet<string> hrefs = new HashSet<string>();

            // 创建 HtmlDocument 对象并加载 HTML
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 查找所有 a 标签
            var nodes = doc.DocumentNode.SelectNodes("//a[@href]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    // 提取 href 属性值并添加到 HashSet 中
                    string href = node.GetAttributeValue("href", string.Empty);
                    if (!string.IsNullOrEmpty(href))
                    {
                        hrefs.Add(href);
                    }
                }
            }

            return hrefs;
        }
        public static HashSet<string> ExtractUrls(string input)
        {
            HashSet<string> urls = new HashSet<string>();

            // 定义匹配 URL 的正则表达式
            string pattern = @"https?://[^\s/$.?#].[^\s]*";
            Regex regex = new Regex(pattern);

            // 使用正则表达式匹配所有 URL
            MatchCollection matches = regex.Matches(input);

            // 将匹配的 URL 添加到 HashSet 中
            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }

            return urls;
        }

        public static string GetHtmlContent(string url)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), url));

            using (HttpClient client = new HttpClient())
            {
                try
                {       // 设置用户代理
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    string htmlContent = response.Content.ReadAsStringAsync().Result;
                    dbgCls.PrintRet(__METHOD__, htmlContent.Substring(0, 300));
                    return htmlContent;
                }
                catch (HttpRequestException e)
                {
                    Print($"Request error: {e.Message}");
                    dbgCls.PrintRet(__METHOD__, 0);
                    return null;
                }
            }
        }
         
        public static string htm_strip_tags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }


            // Remove <script> tags and their content
            string noScript = Regex.Replace(html, "<script.*?>.*?</script>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Regular expression to match HTML tags
            string pattern = "<.*?>";

            // Replace HTML tags with an empty string
            //    string result = Regex.Replace(noScript, pattern, string.Empty);
            // Replace HTML tags with an empty string
            string result = Regex.Replace(noScript, pattern, string.Empty, RegexOptions.Singleline);

            result = RemoveExtraNewlines(result);
            result = RemoveInvisibleCharacters(result);
            result = delEmpltyLines(result);

            result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            //result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            return result;
        }

        static void RemoveCustomEmojiRendererElement(string inputFilePath, string outputFilePath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(inputFilePath);

            // 在这里添加你的代码来去除 custom-emoji-renderer-element 标签
            // 这里提供一个示例来移除所有的 custom-emoji-renderer-element 标签
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//custom-emoji-renderer-element"))
            {
                node.ParentNode.RemoveChild(node);
            }

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//custom-emoji-element"))
            {
                node.ParentNode.RemoveChild(node);
            }

            doc.Save(outputFilePath);
        }
    }
}
