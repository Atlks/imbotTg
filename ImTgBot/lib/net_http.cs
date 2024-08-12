
using prjx.libx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


using static prjx.libx.exCls;
using static prjx.libx.arrCls;//  prj202405.lib
using static prjx.libx.dbgCls;
using static prjx.libx.logCls;
using static prjx.libx.corex;
using static prjx.libx.db;
using static prjx.libx.filex;
using static prjx.libx.ormJSonFL;
using static prjx.libx.strCls;
using static prjx.libx.bscEncdCls;
using static prjx.libx.net_http;

namespace prjx.libx
{
    internal class net_http
    {
        /// <summary>
        /// 发出 HTTP GET 请求并返回响应内容
        /// </summary>
        /// <param name="url">请求的 URL</param>
        /// <returns>HTTP 响应内容</returns>
        public static async Task<string> http_GetHttpResponseAsync(string url)
        {
            var __METHOD__ = "http_GetHttpResponseAsync";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args( url));

            // 创建 HttpClient 实例
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 发出 GET 请求
                    HttpResponseMessage response = await client.GetAsync(url);

                    // 确认响应状态成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string responseBody = await response.Content.ReadAsStringAsync();
                    responseBody= strip_tags(responseBody);
                    // 获取当前时间并格式化为文件名
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                    file_put_contents("httplogDir/"+ timestamp+".log", responseBody);
                    dbgCls.PrintRet(__METHOD__, Sub1109(responseBody,0,500));
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    // 捕获并处理请求异常
                   Print($"Request exception: {e.Message}");
                    dbgCls.PrintRet(__METHOD__, -0);
                    return null;
                }
            }

          

        }
    }
}
