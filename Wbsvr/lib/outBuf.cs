using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    using System;
    using System.IO;

    public class OutputBuffer
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;

        /// <summary>
        /// 启动输出缓冲
        /// </summary>
        public void ob_start()
        {
            // 保存原始的输出
            originalOutput =System.  Console.Out;

            // 创建一个新的 StringWriter 实例
            stringWriter = new StringWriter();

            // 将 Console.Out 设置为 StringWriter 实例
          //  Console.SetOut(stringWriter);
        }

        /// <summary>
        /// 获取并清空当前缓冲区的内容
        /// </summary>
        /// <returns>缓冲区内容</returns>
        public string ob_get_contents()
        {
            return stringWriter.ToString();
        }

        /// <summary>
        /// 清空缓冲区
        /// </summary>
        public void ob_clean()
        {
            stringWriter.GetStringBuilder().Clear();
        }

        /// <summary>
        /// 获取缓冲区内容并关闭缓冲
        /// </summary>
        /// <returns>缓冲区内容</returns>
        public string ob_end()
        {
            string output = stringWriter.ToString();

            // 恢复原始的输出
            System. Console.SetOut(originalOutput);

            // 释放 StringWriter
            stringWriter.Dispose();
            stringWriter = null;

            return output;
        }
    }

    //// 示例用法
    //class Program
    //{
    //    static void Main()
    //    {
    //        OutputBuffer ob = new OutputBuffer();

    //        // 启动输出缓冲
    //        ob.ob_start();

    //        // 输出一些内容
    //       print("Hello, World!");
    //       print("This is a test.");

    //        // 获取并打印缓冲区内容
    //        string output = ob.ob_end();
    //       print("Buffered Output:");
    //       print(output);
    //    }
    //}

}
