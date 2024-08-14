﻿global using static libx.bscMkdwn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace libx
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
            return PrintFormattedTable(lines, columnWidths);
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

            //for data row ,so skip2
            foreach (var line in lines.Skip(2))
            {
                var columns = line.Split('|').Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                for (int i = 0; i < columns.Length; i++)
                {
                    try
                    {
                        var columnWidth = columns[i].Trim().Length;
                        if (columnWidth > columnWidths[i])
                        {
                            columnWidths[i] = columnWidth;
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
                            return column.Trim().PadRight(columnWidths[index]);
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
