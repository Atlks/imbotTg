global using static libx.datetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libx
{
    internal class datetime
    {
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
