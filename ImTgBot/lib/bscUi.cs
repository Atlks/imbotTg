global using static libx.bscUi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libx
{
    internal class bscUi
    {
        public static object render_title_table = "table_title";
        public static object render_rowRender = "render_rowRender";
        public static string RenderTable(List<SortedList> li, Hashtable tmplt)
        {
            var sb = new StringBuilder();

            // 添加表头
            //  sb.AppendLine("| uid      | name     | demo|");
            sb.Append(tmplt[render_title_table]);

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

    }
}
