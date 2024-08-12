using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class jsonCls
    {
        //public static void Main(string[] args)
        //{
        //    // 示例用法
        //    string filePath = "data.json";
        //    string query = "name == 'John'"; // 例如：查询名为 'John' 的对象

        //    List<JToken> result = QueryJsonArray(filePath, query);
        //    foreach (JToken item in result)
        //    {
        //       print(item);
        //    }
        //}

        /// <summary>
        /// 对 JSON 数组文件进行查询，并返回符合条件的 JSON 对象列表
        /// </summary>
        /// <param name="filePath">JSON 文件路径</param>
        /// <param name="query">查询条件</param>
        /// <returns>符合条件的 JSON 对象列表</returns>
        public static List<JToken> QueryJsonArray(string filePath, string query)
        {
            List<JToken> resultList = new List<JToken>();

            // 读取 JSON 文件内容
            string jsonText = File.ReadAllText(filePath);

            // 解析 JSON 数组
            JArray jsonArray = JArray.Parse(jsonText);

            // 应用查询条件
            foreach (JToken token in jsonArray)
            {
                if (EvaluateQuery(token, query))
                {
                    resultList.Add(token);
                }
            }

            return resultList;
        }

        /// <summary>
        /// 对 JSON 对象应用查询条件
        /// </summary>
        /// <param name="jsonObject">JSON 对象</param>
        /// <param name="query">查询条件</param>
        /// <returns>是否符合查询条件</returns>
        private static bool EvaluateQuery(JToken jsonObject, string query)
        {
            try
            {
                //JToken.SelectToken
                // 使用 LINQ to JSON 查询语法进行查询
                // return jsonObject.($"[?({query})]") != null;
                return true; ;
            }
            catch (Exception)
            {
                // 查询条件格式不正确
               Print("Invalid query format.");
                return false;
            }
        }
    }
}
