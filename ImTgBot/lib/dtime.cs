using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static prjx.libx.dtime;
namespace prjx.libx
{
    internal class dtime
    {

        public static string ConvertUnixTimeStampToDateTime(long unixTimeStamp)
        {
            // 创建 DateTimeOffset 对象，表示 UTC 时间
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp);

            // 将 UTC 时间转换为本地时间
            DateTime dateTime = dateTimeOffset.LocalDateTime;

            // 格式化日期时间字符串
            string formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            return formattedDate;
        }
        public static void showSpanTime(long timestamp, string showtitle)
        {
            long timestamp_end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long spantime = (timestamp_end - timestamp);

           Print(showtitle + spantime);
        }

        /// <summary>
        /// 将日期字符串解析为 Unix 时间戳。
        /// </summary>
        /// <param name="dateStr">日期字符串。</param>
        /// <returns>Unix 时间戳（自1970年1月1日以来的秒数）。如果解析失败，则返回 -1。</returns>
        public static long strtotime(string dateStr)
        {
            DateTime dateTime;
            string[] formats = {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd",
            "MM/dd/yyyy HH:mm:ss",
            "MM/dd/yyyy",
            "MMM d, yyyy h:mm:ss tt",
            "MMM d, yyyy",
            "dddd, MMMM d, yyyy h:mm:ss tt",
            "dddd, MMMM d, yyyy",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyyMMddTHHmmssZ",
            "o"  // ISO 8601 format
        };

            if (DateTime.TryParseExact(dateStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
                return dateTimeOffset.ToUnixTimeSeconds();
            }
            else
            {
                return -1; // 如果解析失败，返回 -1
            }
        }

        internal static string datetime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string uuidYYMMDDhhmmssfff()
        {
            return DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        }
    }
}
