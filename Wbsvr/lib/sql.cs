using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace prjx.libx
{
    internal class sqlCls
    {



        public static string arr_toSqlPrms4insert(SortedList SortedList1)
        {



            //var columns = implode(", ", arr.Keys);

            var columns = strCls.Join20241109(",", SortedList1.Keys);          

            string values2024 = valueSqlFmt(SortedList1);
            return "(" + columns + ")values(" + values2024 + ")";
        }

        private static string valueSqlFmt(SortedList SortedList1)
        {
           
            // 获取Hashtable的所有值
            ICollection values = SortedList1.Values;

            // 将值转换为字符串数组
            object[] valueArray = values.Cast<object>().ToArray();

            // 使用 LINQ 的 Select 操作对字符串数组进行操作
            //   if(value==null)
            var escaped_values = valueArray.Select(value =>
            {
                if (value == null)
                    return "null";
                else
                    return "'" + value.ToString() + "'";
            }

            ).ToArray();
            string values2024 = string.Join(",", escaped_values);
            return values2024;
        }
    }
}
