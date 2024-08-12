global using static prjx.libx.strCls;
//using JiebaNet.Segmenter;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static prjx.libx.corex;
//   prj202405.lib.strCls
namespace prjx.libx
{
    internal class strCls
    {

        // 分割 HTML 文件中的字符串，依据 <%= 和 %> 标签
        public static List<string> SplitByExpressions(string filePath)
        {
            var segments = new List<string>();

            // 确保文件路径存在
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的文件未找到", filePath);
            }

            // 读取文件内容
            string fileContent = System.IO.File.ReadAllText(filePath);

            // 解析内容
            string[] parts = SplitContent(fileContent);

            // 添加分割后的部分到列表
            foreach (var part in parts)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    segments.Add(part.Trim());
                }
            }

            return segments;
        }

        // 根据 <%= 和 %> 分割字符串
        public static string[] SplitContent(string content)
        {
            // 使用分隔符进行分割
            var startDelimiter = "<%=";
            var endDelimiter = "%>";

            var segments = new List<string>();
            int currentIndex = 0;

            while (currentIndex < content.Length)
            {
                // 查找开始分隔符
                int startIndex = content.IndexOf(startDelimiter, currentIndex);
                if (startIndex == -1)
                {
                    // 没有找到开始分隔符，剩余部分作为一个段
                    segments.Add(content.Substring(currentIndex));
                    break;
                }

                // 添加开始分隔符前的部分
                if (startIndex > currentIndex)
                {
                    segments.Add(content.Substring(currentIndex, startIndex - currentIndex));
                }

                // 查找结束分隔符
                int endIndex = content.IndexOf(endDelimiter, startIndex + startDelimiter.Length);
                if (endIndex == -1)
                {
                    // 没有找到结束分隔符，添加剩余部分作为一个段
                    segments.Add(content.Substring(startIndex));
                    break;
                }

                // 添加分隔符之间的部分  - startDelimiter.Length
                segments.Add(content.Substring(startIndex, endIndex - startIndex + endDelimiter.Length));

                // 更新当前索引
                currentIndex = endIndex + endDelimiter.Length;
            }

            return segments.ToArray();
        }



        /// <summary>
        /// 截取字符串的前100个字符。
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string Left(object input2, int len)
        {
            string input = ToStr(input2);
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty; // 如果输入为空或 null，则返回空字符串
            }

            // 截取前100个字符，如果输入长度不足100个字符，则返回整个字符串
            return input.Length <= len ? input : input.Substring(0, len);
        }
        public static string repeat(int count)
        {
            if (count < 0)
                count = 0;
            return new string('$', count);
        }
        public static string str_repeat(int count)
        {
            if (count < 0)
                count = 0;
            return new string('$', count);
        }
        public static string Left(string str, int len)
        {
            if (str == null)
            {
                return "";
              //  throw new ArgumentNullException(nameof(str), "Input string cannot be null.");
            }

          //  const int len = 3;
            if (str.Length <= len)
            {
                return str;
            }

            return str.Substring(0, len);
        }
    
        public static HashSet<string> SplitTxtByChrs(string content, string spltChrs)
        {
            char[] separators = spltChrs.ToCharArray();// new char[] { ' ', '\r', '\n', ',' };

            HashSet<string> words = new HashSet<string>();

            try
            {
                // 读取文件内容
              //  string content = System.IO.File.ReadAllText(filePath);

                // 根据空格、回车符和逗号拆分单词

                string[] wordArray = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                // 将单词添加到HashSet中
                foreach (string word in wordArray)
                {
                    string word1 = word.Trim();
                    if (word1.Length > 0)
                        words.Add(word1);
                }
            }
            catch (Exception ex)
            {
               Print($"Error reading file: {ex.Message}");
            }

            return words;
        }


        public static HashSet<string> SplitFileByChrs(string filePath, string spltChrs)
        {
            char[] separators = spltChrs.ToCharArray();// new char[] { ' ', '\r', '\n', ',' };

            HashSet<string> words = new HashSet<string>();

            try
            {
                // 读取文件内容
                string content = System.IO.File.ReadAllText(filePath);

                // 根据空格、回车符和逗号拆分单词

                string[] wordArray = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                // 将单词添加到HashSet中
                foreach (string word in wordArray)
                {
                    string word1 = word.Trim();
                    if (word1.Length > 0)
                        words.Add(word1);
                }
            }
            catch (Exception ex)
            {
               Print($"Error reading file: {ex.Message}");
            }

            return words;
        }


        public static string SubstrAfterMarker(string input, string marker)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(marker))
            {
                return "";
              //  throw new ArgumentException("Input and marker cannot be null or empty.");
            }

            int markerIndex = input.IndexOf(marker);
            if (markerIndex == -1)
            {
                return string.Empty; // Marker not found, return empty string
            }

            int startIndex = markerIndex + marker.Length;
            if (startIndex >= input.Length)
            {
                return string.Empty; // No text after marker
            }

            return input.Substring(startIndex);
        }

        /// <summary>
        ///  // 示例用法
      //  string result = sprintf("Hello, {0}! You have {1} new messages.", "John", 5);

        /// 类似于 PHP 中的 sprintf 函数，使用格式字符串和参数数组构建格式化字符串。
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="args">参数数组</param>
        /// <returns>格式化后的字符串</returns>
        public static string Sprintf(string format, params object[] args)
        {
            return string.Format(format, args);
        }



        //public static string[] SpltByFenci(ref string msgx)
        //{
        //    msgx = ChineseCharacterConvert.Convert.ToSimple(msgx);
        //    var segmenter = new JiebaSegmenter();
        
        //    segmenter.LoadUserDict(userDictFile);
        //    segmenter.AddWord("会所"); // 可添加一个新词
        //    segmenter.AddWord("妙瓦底"); // 可添加一个新词
        //    segmenter.AddWord("御龙湾"); // 可添加一个新词
        //    HashSet<string> user_dict = GetUser_dict();
        //    foreach (string line in user_dict)
        //    {
        //        segmenter.AddWord(line);
        //    }
        //    HashSet<string> postnKywd位置词set = ReadLinesToHashSet("位置词.txt");
        //    foreach (string line in postnKywd位置词set)
        //    {
        //        segmenter.AddWord(line);
        //    }




        //    IEnumerable<string> enumerable = segmenter.CutForSearch(msgx);
        //    // 使用 LINQ 的 ToArray 方法进行转换
        //    string[] kwds = enumerable.ToArray();
        //    kwds = RemoveEmptyItem(kwds);
        //    //  string[] kwds = enumerable; // 搜索引擎模式
        //    string kdwsJoin = string.Join("/", kwds);
        //   print("【搜索引擎模式】：{0}", kdwsJoin);
        //    return kwds;
        //}

        public static string[] RemoveEmptyItem(string[] input)
        {
            if (input == null)
            {
                return new string[0];
            }

            return input
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();
        }
        //public static string calcKwds(ref string msgx)
        //{
        //    msgx = ChineseCharacterConvert.Convert.ToSimple(msgx);
        //    var segmenter = new JiebaSegmenter();
        //    segmenter.LoadUserDict("user_dict.txt");
        //    segmenter.AddWord("会所"); // 可添加一个新词
        //    segmenter.AddWord("妙瓦底"); // 可添加一个新词
        //    var kwds = segmenter.CutForSearch(msgx); // 搜索引擎模式
        //    string kdwsJoin = string.Join("/", kwds);
        //   print("【搜索引擎模式】：{0}", kdwsJoin);
        //    return kdwsJoin;
        //}

        /// <summary> 
        /// 繁体转换为简体
        /// </summary> 
        /// <param name="str">繁体字</param> 
        /// <returns>简体字</returns> 
        //public static string GetSimplified(string str)
        //{
        //    return ChineseConverter.Convert(str, ChineseConversionDirection.TraditionalToSimplified);
        //}

     

        //public static string ConvertToSimplifiedChinese(string traditionalChinese)
        //{
        //    // 使用 Microsoft.VisualBasic 中的 StrConv 方法进行转换
        //    string simplifiedChinese = Strings.StrConv(traditionalChinese, VbStrConv.SimplifiedChinese);
        //    return simplifiedChinese;
        //}
        internal static bool Contain(string? caption, string v)
        {
            if (caption == null)
                return false;
            return caption.Contains(v);
        }


        //默认分割
        public static string[] SplitAndTrim(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Array.Empty<string>();
            }

            // 分割字符串，使用 StringSplitOptions.RemoveEmptyEntries 去除多余的空项
            string[] splitResult = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // 对每个成员进行 Trim 操作
            for (int i = 0; i < splitResult.Length; i++)
            {
                splitResult[i] = splitResult[i].Trim();
            }

            return splitResult;
        }

        public  static string Sub1109(string input, int startIndex, int length)
        {
            // 如果起始位置小于字符串长度，则截取指定长度
            if (startIndex < input.Length)
            {
                // 如果起始位置 + 长度 大于字符串长度，则截取剩余的部分
                if (startIndex + length > input.Length)
                {
                    return input.Substring(startIndex);
                }
                else
                {
                    return input.Substring(startIndex, length);
                }
            }
            else
            {
                return string.Empty; // 或者可以抛出异常或者返回 null，视情况而定
            }
        }
        public static string SubstrGetTextAfterKeyword(string text, string keyword)
        {
            int index = text.IndexOf(keyword);
            if (index != -1)
            {
                // 提取关键字后面的文本
                return text.Substring(index + keyword.Length).Trim();
            }
            return null;
        }
        public static string TrimRemoveUnnecessaryCharacters4tgWhtapExt(string input)
        {
            // Define the characters to be removed
            char[] charsToRemove = new char[] { '\"', '[', ']' };

            // Create a new array to hold the filtered characters
            char[] result = new char[input.Length];
            int resultIndex = 0;

            // Iterate over the input string and copy only the characters that are not in charsToRemove
            foreach (char c in input)
            {
                bool shouldRemove = false;
                foreach (char remove in charsToRemove)
                {
                    if (c == remove)
                    {
                        shouldRemove = true;
                        break;
                    }
                }

                if (!shouldRemove)
                {
                    result[resultIndex++] = c;
                }
            }

            // Return the new string without the unnecessary characters
            string v = new string(result, 0, resultIndex);
            return v.Trim();
        }
        public static string ContainRetMatchWd(string? text, HashSet<string> trgSearchKwds)
        {
            if (text == null)
                return "";
            text = text.Trim().ToLower();
            //string[] kwds = trgSearchKwds.Split(" ");
            foreach (string kwd in trgSearchKwds)
            {
                var kwd2 = kwd.Trim().ToLower();
                if (kwd2.Length == 0)
                    continue;

                if (text.Contains(kwd2))
                {
                   Print(" str.containKwds() kwd=>" + kwd2);
                    return kwd2;
                }

            }



            return "";
        }
        public static string ContainRetMatchWd(string? text,string wdsFromfilePath)
        {
            HashSet<string> st = LoadHashstWordsFromFile(wdsFromfilePath);
            return (ContainRetMatchWd(text, st));
        }

     
        
        public static string[] TrimUper(string[] inputArray)
        {
            if (inputArray == null) return [];

            return inputArray
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim().ToUpper())
            .ToArray();
            //  return resultArray;
        }

        public static bool ContainKwdsV2(string? text, HashSet<string> trgSearchKwds)
        {
            if (text == null)
                return false;
            text = text.Trim().ToLower();
            //string[] kwds = trgSearchKwds.Split(" ");
            foreach (string kwd in trgSearchKwds)
            {
                var kwd2 = kwd.Trim().ToLower();
                if (kwd2.Length == 0)
                    continue;

                if (text.Contains(kwd2))
                {
                   Print(" str.containKwds() kwd=>" + kwd2);
                    return true;
                }

            }



            return false;
        }
        public static string[] Splt(object text)
        {
            if (text == null)
                return [];
            else
                return text.ToString().Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
        public static bool ContainKwds(string? text, HashSet<string> trgSearchKwds)
        {
            if (text == null)
                return false;
            //string[] kwds = trgSearchKwds.Split(" ");
            foreach (string kwd in trgSearchKwds)
            {
                var kwd2 = kwd.Trim();
                if (kwd2.Length == 0)
                    continue;

                if (text.Contains(kwd2))
                {
                   Print(" str.containKwds() kwd=>" + kwd2);
                    return true;
                }

            }



            return false;
        }
        public static bool ContainKwds(string? text, string trgSearchKwds)
        {
            if (text == null)
                return false;
            string[] kwds = trgSearchKwds.Split(" ");
            foreach (string kwd in kwds)
            {
                var kwd2 = kwd.Trim();
                if (kwd2.Length > 0)
                {
                    if (text.Contains(kwd2))
                    {
                       Print(" str.containKwds() kwd=>" + kwd2);
                        return true;
                    }

                }
            }



            return false;
        }

        //pai除 城市词
        //触发词  城市词  商品词
        //internal static int containCalcCntScore(string seasrchKw2ds, IEnumerable<string> segments)
        //{
        //    int n = 0;
        //    foreach (string kwd in segments)
        //    {
        //        var kwd2 = kwd.Trim();
        //        if (kwd2.Length > 0)
        //        {
        //            if (strBiz_isPostnWord(kwd2))
        //                continue;
        //            if (seasrchKw2ds.Contains(kwd2))
        //            {
        //                n++;
        //               print(" contain kwd=>" + kwd2);
        //            }

        //        }
        //    }
        //    return n;
        //}


        public static string ReplaceRemoveWords(string inputText, HashSet<string> wordsToRemove)
        {
            // 使用正则表达式拆分文本为单词
           // string[] words = Regex.Split(inputText, @"\W+");

            // 使用 StringBuilder 构建最终的字符串
          //  StringBuilder result = new StringBuilder();
            
            
                foreach (string word in wordsToRemove)
                {
                    // 如果当前单词不在要去除的单词集合中，则将其添加到结果中
                    if (inputText.Contains(word))
                    {
                        inputText = inputText.Replace(word, "");
                    }
                }
            
            

            // 返回结果并去除末尾多余的空格
            return inputText.ToString().Trim();
        }
        public static string replace_RemoveWordsDep(string inputText, HashSet<string> wordsToRemove)
        {
            // 使用正则表达式拆分文本为单词
            string[] words = Regex.Split(inputText, @"\W+");

            // 使用 StringBuilder 构建最终的字符串
            StringBuilder result = new StringBuilder();

            foreach (string word in words)
            {
                // 如果当前单词不在要去除的单词集合中，则将其添加到结果中
                if (!wordsToRemove.Contains(word))
                {
                    result.Append(word).Append(" ");
                }
            }

            // 返回结果并去除末尾多余的空格
            return result.ToString().Trim();
        }

        //pai除 城市词
        //触发词  城市词  商品词
        //internal static int containCalcCntScore(string seasrchKw2ds, IEnumerable<string> segments)
        //{
        //    int n = 0;
        //    foreach (string kwd in segments)
        //    {
        //        var kwd2 = kwd.Trim();
        //        if (kwd2.Length > 0)
        //        {
        //            if (strBiz_isPostnWord(kwd2))
        //                continue;
        //            if (seasrchKw2ds.Contains(kwd2))
        //            {
        //                n++;
        //               print(" contain kwd=>" + kwd2);
        //            }

        //        }
        //    }
        //    return n;
        //}




        /// <summary>
        /// Reads each line from the specified file and adds them to a HashSet.
        /// </summary>
        /// <param name="filePath">The path to the file to read.</param>
        /// <returns>A HashSet containing the unique lines from the file.</returns>
        public static HashSet<string> ReadLinesToHashSet(string filePath)
        {
            HashSet<string> linesHashSet = new HashSet<string>();

            try
            {
                // Read each line from the file and add it to the HashSet
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if(line.Trim().Length>0)
                        linesHashSet.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
               Print($"An error occurred while reading the file: {ex.Message}");
            }

            return linesHashSet;
        }

        internal static bool StrEq(object? v, string cityName4srch)
        {
            if (v == null) return false; if (cityName4srch == null) return false;
            return v.ToString().Trim().ToUpper().Equals(cityName4srch.Trim().ToUpper());
        }


        internal static bool StrEqV2(object? rowVal, Dictionary<string, Microsoft.Extensions.Primitives.StringValues> whereExprsObj, string cityName4srchxx)
        {

            string cityName4srch = LoadFieldTryGetValue(whereExprsObj, cityName4srchxx); ;
            if (cityName4srch == null)  //if not have this clm in where exprs
                return false;
            else if (cityName4srch != null)
            {
                if (rowVal == null)
                    return false;
                return rowVal.Equals(cityName4srch);
            }
            return false;

        }

        internal static string Join20241109(string v, ICollection keys)
        {
            // 获取Hashtable的所有键
            //     ICollection keys = hashtable.Keys;

            // 将键转换为字符串数组
            string[] keyArray = new string[keys.Count];
            keys.CopyTo(keyArray, 0);

            // 将字符串数组中的所有键连接成一个单一的字符串
            string allKeys = string.Join(", ", keyArray);
            return allKeys;
        }


        public static string str_trim_tolower(string msgx2024)
        {
            try
            {
                return msgx2024.Trim().ToString().ToLower();
            }
            catch (Exception ex)
            {
                return "";
            }


        }
        public static bool StrEq(string? username1, string? username2)
        {
            if (username1 == null || username2 == null)
                return false;
            return username1.Equals(username2);
        }


        public static Dictionary<string, string> parse_str(string queryString)
        {
            var queryDictionary = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(queryString))
            {
                return queryDictionary;
            }

            // Split the query string by '&'
            var pairs = queryString.Split('&');

            foreach (var pair in pairs)
            {
                // Split each pair by '='
                var keyValue = pair.Split(new[] { '=' }, 2);

                if (keyValue.Length == 2)
                {
                    var key = HttpUtility.UrlDecode(keyValue[0]);
                    var value = HttpUtility.UrlDecode(keyValue[1]);

                    // Add the key-value pair to the dictionary
                    queryDictionary[key] = value;
                }
                else if (keyValue.Length == 1)
                {
                    var key = HttpUtility.UrlDecode(keyValue[0]);
                    queryDictionary[key] = string.Empty;
                }
            }

            return queryDictionary;
        }

        //   parse_url 函数会返回一个数组，其中包含以下键（如果存在）：scheme、host、port、user、pass、path、query 和 fragment。
        // ToLower
        internal static bool isStartsWith(string? text, string v)
        {
            if (text == null) return false;

            if (text.StartsWith(v))
            {
                return true;
            }
            return false;
        }

    }
}
