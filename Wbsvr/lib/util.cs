
global using static prjx.libx.util;
//using NAudio.Wave;
using Newtonsoft.Json;
using prjx.libx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Management;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static prjx.libx.corex;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
//using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
namespace prjx.libx
{
    internal class util
    {
       // public static string prjdir = @"../../../";
        public static MemoryCache cache2024 = new MemoryCache(new MemoryCacheOptions());
        public const string pageprm251 = "token page pages pagesize limit page limit pagesize from ";
        //  public static bool jmp2exitFlag;
        public static ThreadLocal<bool> jmp2exitFlagInThrd = new ThreadLocal<bool>(() =>
        {
            // 初始化每个线程的值为 false
            return false;
        });
        public static ThreadLocal<bool> Jmp2endCurFunFlag = new ThreadLocal<bool>(() =>
        {
            // 初始化每个线程的值为 false
            return false;
        });

        public static ThreadLocal<SortedList> ifStrutsThrdloc = new ThreadLocal<SortedList>(() =>
        {
            return NewIFAst();
        });

        public static ThreadLocal<object> lastSendMsg = new ThreadLocal<object>(() =>
        {
            
            return null;
        });

        public static SortedList NewIFAst()
        {
            SortedList ifx = new SortedList();

          
            ifx.Add("cdts", new ArrayList());
            ifx.Add("cdtsRzt", false);
            ifx.Add("choose", "Then");
            return ifx;
        }

        public static ThreadLocal<string> jmp2endCurFunInThrd = new ThreadLocal<string>(() =>
        {
            // 初始化每个线程的值为 false
            return "";
        });
        public static string botname = "LianXin_BianMinBot";

        /*
         确保自签名证书中的 Common Name (CN) 或 Subject Alternative Name (SAN) 字段包含正确的域名。例如，证书的 CN 字段应设置为 lianxin.co，或者在 SAN 字段中列出 lianxin.co。
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="certPath"></param>
        /// <param name="certPassword"></param>
        public static void GenerateAndSaveCertificate(string domain, string certPath, string certPassword)
        {
            using (var rsa =  RSA.Create(2048))
            {
                var request = new CertificateRequest($"CN={domain}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                // Set certificate validity period
                var notBefore = DateTimeOffset.UtcNow;
                var notAfter = notBefore.AddYears(1); // Valid for 1 year

                // Create the self-signed certificate
                var certificate = request.CreateSelfSigned(notBefore, notAfter);

                // Export the certificate to a .pfx file
                var pfxBytes = certificate.Export(X509ContentType.Pfx, certPassword);

                File.WriteAllBytes(certPath, pfxBytes);
            }
        }

        /// <summary>
        /// Trims each element in the input string array and returns a new array.
        /// </summary>
        /// <param name="lines">The input string array.</param>
        /// <returns>A new string array with each element trimmed.</returns>
        /// <summary>
        /// Trims non-empty elements in the input string array and returns a new array.
        /// </summary>
        /// <param name="lines">The input string array.</param>
        /// <returns>A new string array with trimmed non-empty elements.</returns>
        public static string[] delEmptyLines(string[] lines)
        {
            if (lines == null)
            {
                return new string[0];
            }

            // Use LINQ Select to trim non-empty elements in the input array
            string[] trimmedArray = lines
                .Where(line => !string.IsNullOrWhiteSpace(line)) // Filter out empty or whitespace lines
                .Select(line => line) // Trim each non-empty line
                .ToArray();

            return trimmedArray;
        }

        public static string delEmpltyLines(string result)
        {
            string[] lines = result.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            lines = delEmptyLines(lines);
            //for (int i = 0; i < lines.Length; i++)
            //{
            //    if (i < 2)
            //        continue;
            //    string line = lines[i];
            //    line = line.Trim();
            //    char[] charr= line.ToCharArray();

            //}
            result = string.Join("\n", lines);
            return result;
        }

        /// <summary>
        /// Removes all non-visible characters from the input string except for carriage return, newline, and space.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A string with only visible characters, spaces, carriage return, and newline.</returns>
        public static string RemoveInvisibleCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Define the regular expression pattern to match all non-printable characters
            // except for space (\x20), carriage return (\x0D), and newline (\x0A).
            string pattern = @"[^\x20-\x7E\x0A\x0D]";

            // Replace matched characters with an empty string
            string result = Regex.Replace(input, pattern, string.Empty);


            // Define the regular expression pattern to match tab characters (\t)
            string pattern2 = @"\t";

            // Replace matched tab characters with an empty string
            result = Regex.Replace(result, pattern2, string.Empty);

            return result;
        }

        public static string RemoveExtraNewlines(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Use regular expression to replace multiple consecutive newline characters with a single newline
            string result = Regex.Replace(input, @"(\r\n|\r|\n)+", Environment.NewLine);

            return result;
        }

        /*
       .crt 文件通常是一个证书文件，通常用于 SSL/TLS 证书。它包含以下内容：
1.公钥：用于加密数据或验证签名。
2.证书颁发机构 (CA) 的签名：CA 用私钥对证书进行签名，证明证书的真实性。
3.证书的有效期：包含证书的开始和结束日期。
4.证书持有者的信息：包括持有者的名称、组织和其他识别信息。
5.证书的使用目的：例如用于加密、签名或身份验证。
6.证书序列号：唯一标识证书。
      */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public static void ParseCertificate(string filePath)
        {
            try
            {
                // 加载证书
                X509Certificate2 certificate = new X509Certificate2(filePath);

                WriteAllText("crt.json", certificate);
                // 提取证书信息
                string subject = certificate.Subject;
                string issuer = certificate.Issuer;
                DateTime notBefore = certificate.NotBefore;
                DateTime notAfter = certificate.NotAfter;
                string thumbprint = certificate.Thumbprint;

                // 打印证书信息
                Console.WriteLine("Subject: " + subject);
                Console.WriteLine("Issuer: " + issuer);
                Console.WriteLine("Valid From: " + notBefore);
                Console.WriteLine("Valid To: " + notAfter);
                Console.WriteLine("Thumbprint: " + thumbprint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing certificate: " + ex.Message);
            }
        }
        public static string ExtractCName(string LINE)
        {
            if(!LINE.Contains("withdraw"))
            {
                return "";
            }

            string pattern22 = @"\b[A-Z]{2,}\b"; // 匹配连续的大写字母（长度为2或更多）

            Match match33 = Regex.Match(LINE, pattern22);

            if (match33.Success)
            {
                return match33.Value;
              //  Console.WriteLine("Extracted word: " + match.Value);
            }
            // 定义正则表达式来匹配币名
            // 匹配可能的币种名称，例如 ETH、BTC、SOL
            string pattern = @"\b(ETH|BTC|SOL|USDT|LTC|OP|ARB|LINK|BNB|XRP|DOGE|SHIB|ADA|AVAX|BCH|DOT|MATIC|ICP|UNI)\b";

            // 创建正则表达式对象
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // 查找匹配项
            var match = regex.Match(LINE);

            // 如果找到匹配项，返回币名
            if (match.Success)
            {
                return match.Value; // 返回匹配的币名
            }
            else
            {
                return ""; // 如果没有找到匹配项，返回 null
            }
        }
        public static string ExtractAddress(string line)
        {
            // 定义要提取的前缀
            string prefix = "Withdrawal Address :";

           
            line = line.ToLower();
            if (line.Contains("withdrawal") && line.Contains("address"))
            {
                // 查找前缀在输入字符串中的位置
                int startIndex = line.IndexOf(":");
                if (startIndex == -1)
                {
                    // 前缀未找到
                    return "";
                }

                // 计算实际提取内容的起始位置
                startIndex +=1;

                // 提取内容并去掉前导空白字符
                string extracted = line.Substring(startIndex).Trim();

                return extracted.Trim();
            }
            return "";

        }
        //public static void TransferFileByRdpWmi(string sourceFilePath, string destinationFilePath, string targetHost, string username, string password)
        //{
        //    Print(" start TransferFileByRdpWmi()" + sourceFilePath + $"  {destinationFilePath} {targetHost} {username} {password} ");
        //    ManagementScope scope = null;

        //    //    在本地计算机上尝试连接到本地 WMI 服务，以确保 WMI 本身工作正常：
        //    //   如果本地连接成功，但远程连接失败，问题可能与网络配置或远程设置有关。



        //    scope = new ManagementScope(@"\\.\root\cimv2");
        //    scope.Connect();

        //    try
        //    {
        //        ConnectionOptions options = new ConnectionOptions
        //        {
        //            Username = username,
        //            Password = password
        //        };

        //        scope = new ManagementScope($@"\\{targetHost}\root\cimv2", options);
        //        scope.Connect();
        //    }
        //    catch (COMException ex)
        //    {
        //        Console.WriteLine($"COMException: {ex.Message}");
        //        Console.WriteLine($"Error Code: {ex.ErrorCode}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception: {ex.Message}");
        //    }
        //    // Create the management object for file transfer
        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT * FROM Win32_Process"));
        //    foreach (ManagementObject obj in searcher.Get())
        //    {
        //        // Create process to copy the file
        //        ManagementClass processClass = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
        //        ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
        //        inParams["CommandLine"] = $"cmd.exe /c copy \"{sourceFilePath}\" \"{destinationFilePath}\"";
        //        ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);

        //        Console.WriteLine($"Process created with ID {outParams["ProcessId"]}");
        //    }
        //}


        public static List<Hashtable> ParseNginxConfigV2(string configContent)
        {
            var mappings = new List<Hashtable>();
            var lines = configContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            string currentLocation = null;
            string proxyPass = null;

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("location"))
                {
                    if (currentLocation != null)
                    {
                        // Add previous location and its proxy_pass to the list
                        if (proxyPass != null)
                        {
                            var mapping = new Hashtable
                        {
                            { currentLocation, proxyPass }
                        };
                            mappings.Add(mapping);
                        }
                    }

                    // Start new location
                    currentLocation = trimmedLine.Split(' ')[1].Trim();
                    proxyPass = null; // Reset proxy_pass for the new location
                }
                else if (trimmedLine.StartsWith("proxy_pass"))
                {
                    proxyPass = trimmedLine.Split(' ')[1].Trim(';').Trim();
                }
            }

            // Add the last location if it has a proxy_pass
            if (currentLocation != null && proxyPass != null)
            {
                var mapping = new Hashtable
            {
                { currentLocation, proxyPass }
            };
                mappings.Add(mapping);
            }

            return mappings;
        }

        // Regex to match proxy_pass directives
        private static readonly Regex ProxyPassRegex = new Regex(@"proxy_pass\s+(\S+);", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Function to parse nginx.conf and extract proxy_pass URLs
        public static List<string> ExtractProxyPassUrls(string configFilePath)
        {
            var proxyPassUrls = new List<string>();

            if (!System.IO.File.Exists(configFilePath))
            {
                throw new FileNotFoundException("The specified nginx.conf file was not found.", configFilePath);
            }

            var fileContent = System.IO.File.ReadAllText(configFilePath);

            // Find all matches of proxy_pass in the file content
            var matches = ProxyPassRegex.Matches(fileContent);

            foreach (Match match in matches)
            {
                if (match.Success && match.Groups.Count > 1)
                {
                    // Extract the URL from the match
                    string url = match.Groups[1].Value.Trim();
                    proxyPassUrls.Add(url);
                }
            }

            return proxyPassUrls;
        }



        public static string ExtParks(string pkrPrm)
        {
            HashSet<string> pks = SplitToHashset(pkrPrm);
            HashSet<string> pksNew = new HashSet<string>();
            foreach (string pk in pks)
            {
                if (pk.Trim() == "")
                    continue;
                if (ISCtry(pk))
                {
                    string pks242 = CastToParksByCtry(pk);
                    AddElmts2hashset(pksNew, pks242);
                    continue;
                }
                if (ISCity(pk))
                {
                    string pks242 = CastToParksByCity(pk);
                    AddElmts2hashset(pksNew, pks242);
                    continue;
                }
                pksNew.Add(pk);
            }
            string rzt = ToStrFromHashset(pksNew);
            return rzt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        public static void SetConsoleQuickEditMode(bool enable=false)
        {
            const string consoleKeyPath = @"HKEY_CURRENT_USER\Console";
            const string quickEditModeValueName = "QuickEdit";

            try
            {
                // Check if the registry key exists
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(consoleKeyPath, true))
                {
                    if (key != null)
                    {
                        // Set the QuickEdit value
                        key.SetValue(quickEditModeValueName, enable ? 1 : 0, RegistryValueKind.DWord);
                    }
                    else
                    {
                        Console.WriteLine("￥SetConsoleQuickEditMode Registry key not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"￥SetConsoleQuickEditMode An error occurred while modifying the registry: {ex.Message}");
            }
        }

        //dep
        //public static List<Hashtable> ParseNginxConfig(string configContent)
        //{
        //    var mappings = new List<Hashtable>();
        //    // \S 在正则表达式中表示 非空白字符。
        //    var locationPattern = new Regex(@"location\s+(\S+)\s*\{(?:[\s\S]*?(proxy_pass\s+(\S+);)?)[\s\S]*?\}", RegexOptions.Multiline);
        //    var matches = locationPattern.Matches(configContent);

        //    foreach (Match match in matches)
        //    {
        //        if (match.Groups.Count >= 4)
        //        {
        //            string locationPath = match.Groups[1].Value.Trim();
        //            string proxyPass = match.Groups[3].Success ? match.Groups[3].Value.Trim() : null;

        //            if (proxyPass != null)
        //            {
        //                var mapping = new Hashtable
        //            {
        //                { locationPath, proxyPass }
        //            };
        //                mappings.Add(mapping);
        //            }
        //        }
        //    }

        //    return mappings;
        //}


        /// <summary>
        /// 通过 HTTP POST 请求将文件发送到指定的 URL
        /// </summary>
        /// <param name="filePath">要发送的文件路径</param>
        /// <param name="url">目标 URL</param>
        /// <returns>异步任务</returns>
        public static void UploadFileAsync(string filePath, string url)
        {
            Print("UploadFileAsync() " + filePath + " " + url);
            try
            {
                using (HttpClient client = new HttpClient())
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 创建文件内容
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                    // 添加文件内容到请求
                    content.Add(fileContent, "file", Path.GetFileName(filePath));

                    // 发送 POST 请求
                    HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult(); ;

                    // 检查响应状态
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("File uploaded successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to upload file. Status code: {response.StatusCode}");
                    }
                }
            }catch(Exception e)
            {
                PrintExcept("UploadFileAsync", e);
            }
            
          //  return 1;
        }



        public static void PrintTimestamp(string msg)
        {
            // 获取当前时间（本地时间）
            DateTime now = DateTime.Now;

            // 格式化为可读性较强的字符串，精确到毫秒
            string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // 打印结果
            Console.WriteLine($"⏱️⏱️ {msg} milliseconds: " + formattedDate);
        }
        public static void PrintTimestamp()
        {
            // 获取当前时间（本地时间）
            DateTime now = DateTime.Now;

            // 格式化为可读性较强的字符串，精确到毫秒
            string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // 打印结果
            Console.WriteLine("⏱️⏱️Current time with milliseconds: " + formattedDate);
        }
        public static int CalculateTotalPages(int pageSize, int totalRecords)
        {
            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            if (totalRecords < 0)
            {
                totalRecords = 0;
            }

            return (int)Math.Ceiling((double)totalRecords / pageSize);
        }
        //public static int GetBatteryPercentage()
        //{
        //    try
        //    {
        //        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
        //        foreach (ManagementObject obj in searcher.Get())
        //        {
        //            int estimatedChargeRemaining = Convert.ToInt32(obj["EstimatedChargeRemaining"]);
        //            return estimatedChargeRemaining;
        //        }

        //        throw new Exception("Battery not found");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("An error occurred while retrieving battery information: " + ex.Message);
        //    }
        //}

    
        public static string mp3FilePath_slowSkedu = "C:\\Users\\Administrator\\OneDrive\\mklv song lst\\Nana - Lonely HQ.mp3";

        public static string mp3FilePathEmgcy = "C:\\Users\\Administrator\\OneDrive\\90后非主流的歌曲 v2 w11\\Darin-Be What You Wanna Be HQ.mp3"; // 替换为你的 MP3 文件路径

        //public static void tipDayu(string msg2056, Telegram.Bot.Types.Update update)
        //{
        //    try
        //    {
        //        if (msg2056.Contains("xxx007") || msg2056.Contains("大鱼") || msg2056.Contains("鱼总"))
        //            playMp3(mp3FilePathEmgcy);

        //    }
        //    catch (Exception ex)
        //    {
        //       Print(ex);
        //        logCls.error_logV2(ex, "err.log");
        //    }
        //    try
        //    {
        //        if (update.Message.ReplyToMessage.From.FirstName.Contains("大鱼"))
        //            playMp3(mp3FilePathEmgcy);
        //    }
        //    catch (Exception ex) { }



        //}
     
        public static string userDictFile = $"{prjdir}/cfg/user_dict.txt";
     
     
        /*
         open com ..beri show tips com disable
         <Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>

  <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
  <EnableComHosting>true</EnableComHosting>
         */
    }
}
