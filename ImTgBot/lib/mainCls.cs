global using static libx.mainCls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libx
{
    internal class mainCls
    {
        public static void Main2024(Action act)
        {
           
            //     string req = "st finished HTTP/1.0 GET http://lianxin.co/api/getlist?page=1&pagesize=20&%E5%9B%AD%E5%8C%BA=&%E5%9B%BD%E5%AE%B6=%E5%8D%B0%E5%BA%A6%E5%B0%BC%E8%A5%BF%E4%BA%9A,%E6%B3%B0%E5%9B%BD,%E7%BC%85%E7%94%B8&%E5%9F%8E%E5%B8%82=";
            Evtboot(() =>
            {
                //   botClient = botClient;



            });
            // RcvMsgStart();
            act();
            
            LoopForever();
        }

    }
}
