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

        public static int CountSundaysUntilEndOfMonth(string dateInput)
        {
            // 解析输入的日期格式
            if (DateTime.TryParseExact(dateInput, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                // 获取该日期所在月份的最后一天
                DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

                int sundayCount = 0;

                // 计算从给定日期到月底的星期天数量
                for (DateTime day = date; day <= lastDayOfMonth; day = day.AddDays(1))
                {
                    if (day.DayOfWeek == DayOfWeek.Sunday)
                    {
                        sundayCount++;
                    }
                }

                return sundayCount;
            }
            else
            {
                throw new ArgumentException("Invalid date format. Please provide a date in yyyyMMdd format.");
            }
        }
        public static int DaysUntilEndOfMonth(string dateInput)
        {
            // 解析输入的日期格式
            if (DateTime.TryParseExact(dateInput, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                // 获取该日期所在月份的最后一天
                DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

                // 计算从给定日期到月底的天数
                int remainingDays = (lastDayOfMonth - date).Days;

                return remainingDays;
            }
            else
            {
                throw new ArgumentException("Invalid date format. Please provide a date in yyyyMMdd format.");
            }
        }
        public static int GetWorkDaysExcludingSundays(string dateInput)
        {
            // 解析输入的日期格式
            if (DateTime.TryParseExact(dateInput, "yyyyMM", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                // 获取该月份的第一天和最后一天
                DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                int totalDays = (lastDayOfMonth - firstDayOfMonth).Days + 1;
                int sundaysCount = 0;

                // 计算该月份中星期日的数量
                for (DateTime day = firstDayOfMonth; day <= lastDayOfMonth; day = day.AddDays(1))
                {
                    if (day.DayOfWeek == DayOfWeek.Sunday)
                    {
                        sundaysCount++;
                    }
                }

                // 返回总天数减去星期日数量
                return totalDays - sundaysCount;
            }
            else
            {
                throw new ArgumentException("Invalid date format. Please provide a date in yyyyMMdd format.");
            }
        }
        public static bool IsSunday(string dateStr)
        {
            // 定义日期格式
            string format = "yyyyMMdd";
            DateTime date;

            // 尝试解析日期字符串
            if (DateTime.TryParseExact(dateStr, format, null, System.Globalization.DateTimeStyles.None, out date))
            {
                // 判断是否是星期天
                return date.DayOfWeek == DayOfWeek.Sunday;
            }
            else
            {
                // 如果解析失败，可以选择抛出异常或返回 false
                throw new ArgumentException("Invalid date format. Expected format is yyyyMMdd.");
            }
        }
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
