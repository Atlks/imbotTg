global using static libx.datetime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libx
{
    internal class datetime
    {
        public static string FmtDateMMDD(DateTime curdate)
        {
            return curdate.ToString("MMdd");
        }


        // 将 yyyyMMdd 格式的字符串转换为 DateTime 类型
        public static DateTime ConvertToDateTime(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                throw new ArgumentException("日期字符串不能为空或只包含空白字符。", nameof(dateString));
            }

            // 定义日期格式
            const string format = "yyyyMMdd";

            // 尝试将字符串转换为 DateTime 类型
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"无法将字符串 '{dateString}' 转换为 {format} 格式的日期。");
            }
        }
        public static int GetMaxDaysOfMonth(string yyyymm)
        {
            // 确保输入格式正确
            if (yyyymm.Length != 6 || !int.TryParse(yyyymm, out _))
            {
                throw new ArgumentException("输入格式无效，请使用 yyyymm 格式。");
            }

            // 提取年和月
            int year = int.Parse(yyyymm.Substring(0, 4));
            int month = int.Parse(yyyymm.Substring(4, 2));

            // 验证月份的有效性
            if (month < 1 || month > 12)
            {
                throw new ArgumentException("月份无效，请输入 01 到 12 之间的值。");
            }

            // 获取指定月份的最后一天
            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            // 返回最大天数
            return lastDayOfMonth.Day;
        }
    }
}
