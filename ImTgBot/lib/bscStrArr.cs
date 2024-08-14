global using static prjx.libx.bscStrArr;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class bscStrArr
    {
        private static string SubtractFrmFilist(string ids, string jsonArray)
        {
            // 将 JSON 数组字符串转换为 List<string>
            var userList = JsonConvert.DeserializeObject<List<string>>(jsonArray);
            // 创建一个新的列表来存储过滤后的结果
            var filteredList_ids = new List<string>();
            string[] a = ids.Split(" ");

            foreach (var flstr in userList)
            {
                foreach (string id in a)
                {
                    // 检查用户是否包含指定字符串
                    if (flstr.Contains(ids))
                    {
                        // 不包含指定字符串的用户添加到过滤列表
                        filteredList_ids.Add(id);
                    }
                }

            }
            return Subtract(ids, filteredList_ids);

        }
        public static HashSet<string> Subtract(HashSet<string> ids, HashSet<string> filteredList_ids)
        {
            return Difference<string>(ids, filteredList_ids);
        }
        public static HashSet<T> Difference<T>(HashSet<T> set1, HashSet<T> set2)
        {
            // 创建 set1 的副本以保持原始集合不变
            var resultSet = new HashSet<T>(set1);

            // 从 resultSet 中移除 set2 中的元素
            resultSet.ExceptWith(set2);

            return resultSet;
        }
        public static string Subtract(string ids, List<string> filteredList_ids)
        {
            // 将空格分割的 ids 字符串转换为列表
            var idList = ids.Split(' ').ToList();

            // 创建一个 HashSet 来加快过滤操作
            var filteredSet = new HashSet<string>(filteredList_ids);

            // 过滤掉在 filteredListIds 中的 ID
            var resultList = idList.Where(id => !filteredSet.Contains(id)).ToList();

            // 将结果列表转换为以空格分隔的字符串
            return string.Join(" ", resultList);
        }
        public static HashSet<string> SubtractFrmFilist(HashSet<string> ids, string jsonArray)
        {
            // 将 JSON 数组字符串转换为 List<string>
            var userList = JsonConvert.DeserializeObject<List<string>>(jsonArray);
            // 创建一个新的列表来存储过滤后的结果
            var filteredList_ids = new HashSet<string>();
            // string[] a = ids.Split(" ");

            foreach (var flstr in userList)
            {
                foreach (string id in ids)
                {
                    // 检查用户是否包含指定字符串
                    if (flstr.Contains(id))
                    {
                        // 不包含指定字符串的用户添加到过滤列表
                        filteredList_ids.Add(id);
                    }
                }

            }
            return Subtract(ids, filteredList_ids);

        }
        public static SortedList FltWhrKeyIn(SortedList inimap, HashSet<string> noExistUid)
        {
            // 创建新的 SortedList 来存储过滤后的键值对
            var filteredList = new SortedList();

            // 遍历 inimap 的所有键
            foreach (DictionaryEntry entry in inimap)
            {
                string key = (string)entry.Key;

                // 如果键在 noExistUid 中，添加到新的 SortedList
                if (noExistUid.Contains(key))
                {
                    filteredList.Add(key, entry.Value);
                }
            }

            return filteredList;
        }


        //public static bool IsContainsAny(string flstr, string ids)
        //{
        //  //  throw new NotImplementedException();
        //}
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
