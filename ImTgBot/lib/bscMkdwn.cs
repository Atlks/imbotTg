global using static libx.bscMkdwn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  libx
{
    internal class bscMkdwn
    {
        public static string FormatAndPrintMarkdownTable(string markdownTable)
        {
            // 拆分 Markdown 表格的行
            var lines = markdownTable.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2)
            {
                Console.WriteLine("Invalid table format.");
                return "Invalid table format.";
            }

            // 计算列宽
            var columnWidths = CalculateColumnWidths(lines);

            // 打印格式化表格
        return    PrintFormattedTable(lines, columnWidths);
        }

        private static int[] CalculateColumnWidths(string[] lines)
        {
            // 获取表头和分隔行
            var headerLine = lines[0];
            var separatorLine = lines[1];

            // 分割列
            var headers = headerLine.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            var separators = separatorLine.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            // 计算每列的最大宽度
            var columnWidths = headers.Select(header => header.Trim().Length).ToArray();

            foreach (var line in lines.Skip(2))
            {
                var columns = line.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                for (int i = 0; i < columns.Length; i++)
                {
                    var columnWidth = columns[i].Trim().Length;
                    if (columnWidth > columnWidths[i])
                    {
                        columnWidths[i] = columnWidth;
                    }
                }
            }

            return columnWidths;
        }

        private static string PrintFormattedTable(string[] lines, int[] columnWidths)
        {
            var sb = new StringBuilder();

            // 打印表头
            var headerLine = lines[0].Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            sb.AppendLine(string.Join(" | ", headerLine.Select((header, index) => header.Trim().PadRight(columnWidths[index]))));

            // 打印分隔线
            var separatorLine = lines[1].Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            sb.AppendLine(string.Join(" | ", separatorLine.Select((sep, index) => new string('-', columnWidths[index]))));

            // 打印表格内容
            foreach (var line in lines.Skip(2))
            {
                var columns = line.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                sb.AppendLine(string.Join(" | ", columns.Select((column, index) => column.Trim().PadRight(columnWidths[index]))));
            }

            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
    }
}
