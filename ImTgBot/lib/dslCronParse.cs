using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
namespace prjx.libx
{

    /// <summary>
    /// 可以编写一个自定义的解析器。下面是一个基本的示例，实现了对标准的 5 部分 Cron 表达式的解析和检查
    /// </summary>
    public class CronParser
    {
        /// <summary>
        /// 可以编写一个自定义的解析器。下面是一个基本的示例，实现了对标准的 5 部分 Cron 表达式的解析和检查
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <param name="nowDt"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool ShouldRun(string cronExpression, DateTime nowDt)
        {
            var parts = cronExpression.Split(' ');
            if (parts.Length != 5)
            {
                throw new ArgumentException("Invalid cron expression");
            }

            var minute = parts[0];
            var hour = parts[1];
            var dayOfMonth = parts[2];
            var month = parts[3];
            var dayOfWeek = parts[4];

            return Match(nowDt.Minute, minute) &&
                   Match(nowDt.Hour, hour) &&
                   Match(nowDt.Day, dayOfMonth) &&
                   Match(nowDt.Month, month) &&
                   Match((int)nowDt.DayOfWeek, dayOfWeek);
        }

        private static bool Match(int value, string expression)
        {
            if (expression == "*")
            {
                return true;
            }

            var values = ParseExpression(expression);
            return values.Contains(value);
        }

        private static List<int> ParseExpression(string expression)
        {
            var values = new List<int>();
            var parts = expression.Split(',');

            foreach (var part in parts)
            {
                if (part.Contains("/"))
                {
                    var rangeParts = part.Split('/');
                    var range = ParseRange(rangeParts[0]);
                    var step = int.Parse(rangeParts[1]);

                    values.AddRange(range.Where((_, index) => index % step == 0));
                }
                else if (part.Contains("-"))
                {
                    values.AddRange(ParseRange(part));
                }
                else
                {
                    values.Add(int.Parse(part));
                }
            }

            return values;
        }

        private static IEnumerable<int> ParseRange(string range)
        {
            var bounds = range.Split('-');
            var start = int.Parse(bounds[0]);
            var end = int.Parse(bounds[1]);

            return Enumerable.Range(start, end - start + 1);
        }

        public static void Main33()
        {
            // 测试
            string cronExpression = "*/5 * * * *"; // 每5分钟执行一次
            DateTime now = DateTime.Now;

            bool shouldRun = ShouldRun(cronExpression, now);
            ConsoleWriteLine($"Should run: {shouldRun}");
        }
    }

}
