global using static libx.bscMkdwn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace libx
{
    internal class bscMkdwn
    {

        public static string FormatMarkdown2consl(string markdown)
        {
            // 分割每一行
            var lines = markdown.Split("\n");
                //markdown.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            bool isTable = false;
            List<string[]> tableLines = new List<string[]>();
            string rztLines = "";
            foreach (var linex in lines)
            {
              var  line = linex.Trim();
               
                // 检查是否是表格行
                if (Regex.IsMatch(line, @"^\|.+\|$"))
                {
                    isTable = true;
                    line = line.Trim('|');
                    string[] itemLin = line.Split('|').Select(c => c.Trim()).ToArray();
                    tableLines.Add(itemLin);
                }
                else
                {    //    // 如果当前行不是表格
                    if (isTable)
                    {
                        // 如果当前行不是表格且之前行是表格，则格式化表格
                        rztLines = rztLines +   FormatTableMkd2console(tableLines)+"\n";
                        tableLines.Clear();
                        isTable = false;
                    }
                    //  Console.WriteLine(line);  // 原样输出非表格行
                    rztLines = rztLines + line + "\n";
                }
            }

            // 如果文档以表格结尾，处理剩余的表格
            if (isTable)
            {
                rztLines = rztLines + ( FormatTableMkd2console(tableLines))+"\n";
            }
            return rztLines;
        }

        /// <summary>
        /// convert fmt
        /// </summary>
        /// <param name="tableLines"></param>
        /// <returns></returns>
        public static string FormatTableMkd2console(List<string[]> tableLines)
        {
            if (tableLines.Count == 0) return "";

            // 计算每列的最大宽度
            var columnWidths = new int[tableLines[0].Length];
            foreach (var row in tableLines)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    columnWidths[i] = Math.Max(columnWidths[i],Len( row[i]));
                }
            }

            string rzt = "";
            // 输出格式化后的表格
            foreach (var row in tableLines)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    string colVal = row[i];
                    int widTotal = columnWidths[i];
                 
                    if(colVal=="---")
                        rzt = rzt + ($" { Repeat('-',widTotal)} |");
                    else
                        rzt = rzt + ($" {PadRight(colVal, widTotal)} |");
                    //    rzt= rzt+($"| {row[i].PadRight(columnWidths[i])} ");
                }
                rzt = rzt.Trim('|');
                rzt = rzt + ("\n");
            }
            return rzt;
        }
        public static string FormatAndPrintMarkdownTable(string markdownTable)
        {
            // 拆分 Markdown 表格的行
            var lines = markdownTable.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2)
            {
                Console.WriteLine("Invalid table format.");
                return "Invalid table format.";
            }

            // 计算列宽  int[]  columnWidths
            var columnWidths = CalculateColumnWidths(lines);

            // 打印格式化表格
            return PrintFormattedTable(lines, columnWidths);
        }

        public static int[] CalculateColumnWidths(string[] lines)
        {
            // 获取表头和分隔行
            var headerLine = lines[0];
            var separatorLine = lines[1];

            // 分割列
            var headers = headerLine.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            var separators = separatorLine.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            // 计算每列的最大宽度
            var columnWidths = headers.Select(header => header.Trim().Length).ToArray();

            //for data row ,so skip2
            foreach (var line in lines.Skip(2))
            {
                var columns = line.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                for (int i = 0; i < columns.Length; i++)
                {
                    try
                    {
                        string input = columns[i].Trim();
                        var cur_columnWidth = Len(input);
                        Print("len(s)==> " + cur_columnWidth + "(" + input + ")");
                        if (cur_columnWidth > columnWidths[i])
                        {
                            columnWidths[i] = cur_columnWidth;
                        }
                    }
                    catch (Exception e)
                    {
                        Print(e);
                    }

                }
            }

            return columnWidths;
        }


        public static string ConvertToMarkdown(List<SortedList> list)
        {
            if (list == null || list.Count == 0)
            {
                return string.Empty;
            }

            // 获取表头
            var headers = new List<string>();
            SortedList entriesMapRow = list[0];
            foreach (DictionaryEntry entry in entriesMapRow)
            {
                headers.Add(entry.Key.ToString());
            }

            // 创建表头行
            var sb = new StringBuilder();
            sb.AppendLine("| " + string.Join(" | ", headers) + " |");
            sb.AppendLine("|" + string.Join("|", new string[headers.Count].Select(_ => " --- ")) + "|");

            // 添加数据行
            foreach (var sortedList in list)
            {
                var row = new List<string>();
                foreach (var header in headers)
                {
                    if (sortedList.ContainsKey(header))
                    {
                        row.Add(sortedList[header]?.ToString() ?? string.Empty);
                    }
                    else
                    {
                        row.Add(string.Empty);
                    }
                }
                sb.AppendLine("| " + string.Join(" | ", row) + " |");
            }

            return sb.ToString();
        }

        private static string PrintFormattedTable(string[] lines, int[] columnWidths)
        {
            var sb = new StringBuilder();

            // 打印表头
            var headerLine = lines[0].Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            sb.AppendLine(string.Join(" | ", headerLine.Select((header, index) => header.Trim().PadRight(columnWidths[index]))));

            // 打印分隔线
            var separatorLine = lines[1].Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            IEnumerable<string> values = separatorLine.Select(
                (sep, index) =>
                {
                    try
                    {
                        return new string('-', columnWidths[index]);
                    }
                    catch (Exception e)
                    {
                        return new string('-', 3);
                    }

                }
            );
            sb.AppendLine(string.Join(" | ", values));

            // 打印表格内容
            foreach (var line in lines.Skip(2))
            {
                var columns = line.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                IEnumerable<string> values1 = columns.Select(
                    (column, index) => {
                        try
                        {
                            //PadRight totleWidth
                            // PadRight 这里有bug 需要计算字符排除汉字
                            return PadRight(column.Trim(),columnWidths[index]);
                        }catch(Exception e)
                        {
                            return column.Trim().PadRight(3);
                        }
                        
                    }
                );
                sb.AppendLine(string.Join(" | ", values1));
            }

            //    Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

      
    }
}
