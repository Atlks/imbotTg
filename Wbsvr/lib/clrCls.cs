using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx
{
    internal class clrCls
    {

        static void RemoveCustomEmojiRendererElement(string inputFilePath, string outputFilePath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(inputFilePath);

            // 在这里添加你的代码来去除 custom-emoji-renderer-element 标签
            // 这里提供一个示例来移除所有的 custom-emoji-renderer-element 标签
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//custom-emoji-renderer-element"))
            {
                node.ParentNode.RemoveChild(node);
            }

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//custom-emoji-element"))
            {
                node.ParentNode.RemoveChild(node);
            }

            doc.Save(outputFilePath);
        }
    }
}
