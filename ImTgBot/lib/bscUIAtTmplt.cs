global using static libx.bscUIAtTmplt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace libx
{
    internal class bscUIAtTmplt
    {
        public static void rendTest3()
        {
            // 读取模板文件内容
            string template = System.IO.File.ReadAllText($"{prjdir}/cfg/tmplt2.md");
            var li = new List<dynamic>
            {
                new { uid = "001333", uname = "Alice" },
                new { uid = "002", uname = "Bob" }
            };

            List<Hashtable> liht = ConvertToHashtableList(li);
            // 创建数据模型
            var data = new
            {
                title = "titlexxx",
                list = liht,
            };
            // 渲染模板
            string result = RenderTemplate4AtUi(template, data);
            Print(result);
        }

        
        
        public static string RenderTemplate4AtUi(string template, object data)
        {
            string txt = "";
            string[] lines = template.Split("\n");
            int curIdx = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (curIdx >= lines.Length)
                    break;
                string line = lines[curIdx];
                line = line.Trim();
                if (EndsWithListTag(line))
                {
                    string key = ExtractListName(line);

                    // 查找 {{/ 在字符串中的索引位置
                    int closetagIdx = line.IndexOf("{{/");

                    string rowTmplt = line.Substring(0, closetagIdx);
                    rowTmplt = rowTmplt.Trim() + "\n";
                    object list = GetFieldV2(data, key, new object());
                    string rdredTxt = renderRowTmplt(rowTmplt, list);//new ExpandoObject()
                    txt = txt + rdredTxt;
                    curIdx++;
                    continue;
                }
                else if (line.Contains("{{"))
                {
                    string lin = RendMustacheFrmTmpltTxt(data, line)+"\n";
                    txt = txt + lin;
                 
                    curIdx++;
                    continue;
                }
                else
                {
                    //nml
                    txt = txt + line+"\n";
                    curIdx++;
                }



            }
            return txt;

        }



        public static bool EndsWithListTag(string input)
        {
            // 定义正则表达式，匹配以 {{/xxx}} 结尾的字符串
            string pattern = @"\{\{\/.+\}\}$";

            // 使用正则表达式进行匹配
            return Regex.IsMatch(input, pattern);
        }
        public static string ExtractListName(string input)
        {
            // 定义正则表达式，匹配 {{/xxx}} 并捕获 xxx 部分
            string pattern = @"\{\{\/(\w+)\}\}";

            // 使用正则表达式进行匹配
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                // 返回捕获组中的内容
                return match.Groups[1].Value;
            }
            else
            {
                return null; // 如果不匹配，返回 null
            }
        }

    }
}
