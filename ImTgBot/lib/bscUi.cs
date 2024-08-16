global using static libx.bscUi;
using HandlebarsDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace libx
{
    internal class bscUi
    {
         public static object render_title_table = "table_title";
        public static object render_rowRender = "render_rowRender";


        public static int FindIdxCloseTag(string key, string[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string tk = tokens[i];
                if (tk == "{{/" + key + "}}")
                    return i;
            }
            return -1;
        }

        public static string RenderTemplate4ur(string template, object data)
        {
            string rt = "";
            string[] tokens = SplitByToken(template);
            int curIdx = 0;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (curIdx >= tokens.Length)
                    break;
                string tk = tokens[curIdx];
                if (tk.StartsWith("{{#"))
                {
                    string key = tk.Substring(3, tk.Length - 5).Trim();
                    int closetagIdx = FindIdxCloseTag(key, tokens);
                    string[] subtokes = SliceStrArr(tokens, curIdx+1, closetagIdx);
                    string rowTmplt = string.Join("", subtokes);
                    rowTmplt = rowTmplt.Trim()+"\n";
                    object list = GetFieldV2(data, key, new object());
                    string rdredTxt = renderRowTmplt(rowTmplt, list);//new ExpandoObject()
                    rt = rt + rdredTxt;
                    curIdx = closetagIdx + 1;
                    continue;
                }
                if (tk.StartsWith("{{/"))
                {
                    curIdx++;
                    continue;
                }
                if (tk.StartsWith("{{"))
                {
                    if (tk.StartsWith("{{") && tk.EndsWith("}}"))
                    {
                        string key = tk.Substring(2, tk.Length - 4).Trim();
                        rt = rt + GetFieldAsStrFrmObj(data, key);
                        curIdx++;
                        continue;
                    }


                }

                //nml
                rt = rt + tk;
                curIdx++;

            }
            return rt;

        }

        public static string[] SplitByToken(string template)
        {
            return ParseTemplate(template);
        }
        public static string[] ParseTemplate(string template)
        {
            var tokens = new List<string>();
            var currentToken = new StringBuilder();
            bool inTag = false;

            for (int i = 0; i < template.Length; i++)
            {
                char c = template[i];

                if (c == '{' && template[i + 1] == '{')
                {
                    if (currentToken.Length > 0)
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                    }
                    inTag = true;
                    i++;
                }
                else if (c == '}' && template[i + 1] == '}')
                {
                    tokens.Add("{{" + currentToken.ToString() + "}}");
                    currentToken.Clear();
                    inTag = false;
                    i++;
                }
                else
                {
                    currentToken.Append(c);
                }
            }

            // 处理最后剩余的文本
            if (currentToken.Length > 0)
            {
                tokens.Add(currentToken.ToString());
            }

            return tokens.ToArray();
        }


        public static string renderRowTmplt(string rowTmplt, object list)
        {
            string fnl = "";
            List<Hashtable> li = (List<Hashtable>)list;
            foreach (Hashtable item in li)
            {
                string rzt = RendMustacheFrmTmpltTxt(item, rowTmplt);
                fnl = fnl + rzt;
            }
            return fnl;
        }
        public static string RendMustacheFrmTmpltTxt(Hashtable ht, string tmpltTxt)
        {
         //   var tmpltTxt = ReadAllText(tmpltTxt);
            // 使用正则表达式匹配模板标签 {{key}}
            var regex = new Regex(@"{{(.*?)}}");

            // 替换模板标签
            var result = regex.Replace(tmpltTxt, match =>
            {
                var key = match.Groups[1].Value;
                // 如果 Hashtable 中包含这个键，则用对应的值替换
                if (ht.ContainsKey(key))
                {
                    return ht[key]?.ToString() ?? string.Empty;
                }
                // 如果 Hashtable 中不包含这个键，则保留原标签
                return match.Value;
            });

            return result;
        }

        public static string RendMustache(Hashtable ht, string tmpltf)
        {
            var tmpltTxt = ReadAllText(tmpltf);
            // 使用正则表达式匹配模板标签 {{key}}
            var regex = new Regex(@"{{(.*?)}}");

            // 替换模板标签
            var result = regex.Replace(tmpltTxt, match =>
            {
                var key = match.Groups[1].Value;
                // 如果 Hashtable 中包含这个键，则用对应的值替换
                if (ht.ContainsKey(key))
                {
                    return ht[key]?.ToString() ?? string.Empty;
                }
                // 如果 Hashtable 中不包含这个键，则保留原标签
                return match.Value;
            });

            return result;
        }


        /// <summary>
        /// 渲染模板文件数据，读取Hashtable每个key，替换模板标签。。
        /// mustache格式的  {{key}}
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="tmpltf"></param>
        /// <returns></returns>
        public static string RendTmpltMD(Hashtable ht, string tmpltf)
        {
            var tmpltTxt = ReadAllText(tmpltf);
            // 使用正则表达式匹配模板标签 {{key}}
            var regex = new Regex(@"{{(.*?)}}");

            // 替换模板标签
            var result = regex.Replace(tmpltTxt, match =>
            {
                var key = match.Groups[1].Value;
                // 如果 Hashtable 中包含这个键，则用对应的值替换
                if (ht.ContainsKey(key))
                {
                    return ht[key]?.ToString() ?? string.Empty;
                }
                // 如果 Hashtable 中不包含这个键，则保留原标签
                return match.Value;
            });

            return result;
        }
        public static void rendTest(List<SortedList> li)
        {
            // 读取模板文件内容
            string template = System.IO.File.ReadAllText($"{prjdir}/cfg/tmplt1.md");
            // 创建数据模型
            var data = new
            {
                list = li,
            };
            // 渲染模板
            string result = RenderTemplate(template, data);
            Print(result);
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

        public static string FormatSortedListToMarkdown4rptToday(SortedList sortedList)
        {
            var sb = new StringBuilder();

            // 添加表头
            sb.AppendLine("  | name     | ");
            sb.AppendLine("|-----------|");

            // 遍历 SortedList 并添加到 Markdown 表格
            foreach (DictionaryEntry entry in sortedList)
            {
                string key = (string)entry.Key;
                string value = (string)entry.Value;
                sb.AppendLine($" | {value,-9} |");
            }

            return sb.ToString();
        }

        public static string RenderTemplate(string template, object data)
        {
            // 编译模板
            var templateCompiled = Handlebars.Compile(template);

            // 渲染模板
            string result = templateCompiled(data);

            return DecodeHtmlEntities(result);
        }



        /*
         
         
            //string title = "|uid|连续缺失天数|name|\n|-----|-----|---|\n";
            //Hashtable tmpltMkdwn = new Hashtable();
            //tmpltMkdwn.Add(render_title_table, title);
            //tmpltMkdwn.Add(render_rowRender, (SortedList row) =>
            //{
            //    return "|☀️" + row["uid"].ToString() + "|" + row["连续缺失天数"].ToString() + "|" + row["name"].ToString() + "|";
            //});

            //string mkdwn2 = RenderTable(li, tmpltMkdwn);

            //string mkdwn = FormatListToMarkdown(li, title, (row) =>
            //        {
            //            return "|" + row["uid"].ToString() + "|" + row["连续缺失天数"].ToString() + "|" + row["name"].ToString() + "|";
            //        });
         */

        public static string RenderTable(List<SortedList> li, Hashtable tmplt)
        {
            var sb = new StringBuilder();

            // 添加表头
            //  sb.AppendLine("| uid      | name     | demo|");
           sb.AppendLine(tmplt[render_title_table].ToString());

            sb.AppendLine(ConvertToMarkdownTableSpltLine(tmplt[render_title_table].ToString()));
            //here can calc |----|
            //    sb.AppendLine("|----------|-----------|--------|");
            Func<SortedList, string> rendRowFun = (Func<SortedList, string>)tmplt[render_rowRender];
            foreach (SortedList row in li)
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


        public static string ConvertToMarkdownTableSpltLine(string headerRow, string separator = "---")
        {
            // 将表头按照 '|' 分割
            var headers = headerRow.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(h => h.Trim())
                                   .ToArray();

            // 生成 Markdown 表格的表头行
           // var markdownHeader = $"| {string.Join(" | ", headers)} |";

            // 生成表头与数据行之间的分隔符行
            var separatorRow = $"| {string.Join(" | ", headers.Select(_ => separator))} |";

            return $"{separatorRow}";
        }
    }
}
