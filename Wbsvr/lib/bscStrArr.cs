global using static prjx.libx.bscStrArr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class bscStrArr
    {
        public static string AddIdxToElmt(string[] items, string spltr)
        {
            int n = 1;
            // 使用索引和元素创建新的字符串数组
            string[] indexedItems = new string[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                indexedItems[i] = $"{i + 1}.{items[i]}";
            }

            // 用回车符连接所有元素
            return string.Join(spltr, indexedItems);
        }
    }
}
