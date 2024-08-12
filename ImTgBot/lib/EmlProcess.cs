using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class EmlProcess
    {
        public static void GetAddr(string dir)
        {
            SortedList li = new SortedList();
            // 获取目录中的所有文件
            string[] files = Directory.GetFiles(dir);

            // 遍历每一个文件
            foreach (var filePath in files)
            {
                Hashtable hs = new Hashtable();
                // 读取文件内容
                string content = System.IO.File.ReadAllText(filePath);
                if (content.Length == 0)
                    continue;
                content = htm_strip_tags(content);
                string[] lines = content.Split("\n");
                var add = ExtractAddress310(lines);
                var name = ExtrctName(lines);
                if (add.Length == 0)
                    continue;
                hs.Add("name", name); hs.Add("add", add);
                SetField(li, add, hs);
                //    li.Add(add,hs);
            }
            WriteAllText("adds428.json", li);
        }

        private static string ExtrctName(string[] lines)
        {
            foreach (var line in lines)
            {
                var add = ExtractCName(line);
                if (add.Length > 0)
                    return add;
            }
            return "";
        }

        private static string ExtractAddress310(string[] lines)
        {
            foreach (var line in lines)
            {
                var add = ExtractAddress(line);
                if (add.Length > 0)
                    return add;
            }
            return "";
        }

    }
}
