using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class timeCls
    {


        //转换时间格式
        public static string FormatTimeSpan(TimeSpan? timeSpan)
        {
            return string.Format("{0:D2}:{1:D2}", timeSpan?.Hours, timeSpan?.Minutes);
        }
    }
}
