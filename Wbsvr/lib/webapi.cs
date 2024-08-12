global using static mdsj.lib.webapi;
 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
 
 
using prjx.libx;
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
 

namespace mdsj.lib
{
    // best api todo is v2 more smpl
    // best api todo is v2 more smpl
    internal class webapi
    {

        /*
             webapiStart((context) =>
            {
                var request = context.Request;
                string methd = request.Path;
              ////  methd = methd.Substring(1);
                if (methd == "/swag")
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                     var rzt= Wbapi_swagApi("mdsj.xml");
                    context.Response.WriteAsync(rzt.ToString(), Encoding.UTF8).GetAwaiter().GetResult();
                    jmp2end();
                }
                  
            });
         
         */

        /// <summary>
        /// startWebapi
        /// </summary>
        /// <param name="httpHdlrSpel"></param>
        /// <param name="api_prefix"></param>
        public static void StartWebapi(Action<HttpRequest, HttpResponse> httpHdlrSpel, string api_prefix)
        {
            var builder = Microsoft.AspNetCore.Builder. WebApplication.CreateBuilder();
            // Configure Kestrel to listen on a specific port
            ConfigureWebHostBuilder webHost = builder.WebHost;
            webHost.ConfigureLogging(logging => { });
            // webHost.UseStaticWebAssets
            webHost.ConfigureKestrel(serverOptions =>
            {
                Dictionary<string, string> map = GetDicFromIni($"{prjdir}/cfg/cfg.ini");
                int port = GetFieldAsInt526(map, "wbsvs_port", 5000);
                serverOptions.ListenAnyIP(port); // 自定义端口号，例如5001

                //--------cfg https block
                int https = GetFieldAsInt526(map, "https", 0);
                serverOptions.Limits.MaxRequestBodySize = 20 * 1024 * 1024; // 20 MB
                if (https == 1)
                    CfgHttps(serverOptions, map);
                //-----end cfg https
            });
            var app = builder.Build();
            //http://localhost:5000/dafen?callGetlistFromDb=yourValue11
            //拦截请求：
            RequestDelegate RequestDelegate1 = async (HttpContext context) =>
                        {
                            // todo  TryNotLgJmpEndAsync
                            Action<HttpContext> a = async (HttpContext context) =>
                            {

                            };
                            try
                            {
                                //    a(context);
                                jmp2exitFlagInThrd.Value = false;
                                Print(" start req..."); PrintTimestamp();
                                //here cant new thrd..beir req close early
                                //here use call but not calltryAll bcs not want jmp Ex prt
                                // 确保 HttpHdlr 是异步的
                                await HttpHdlr(context.Request, context.Response, api_prefix, httpHdlrSpel);
                                Print(" end req...");
                                PrintTimestamp();
                            }
                            catch (jmp2endEx e)
                            {

                            }
                            catch (Exception e)
                            {
                                PrintCatchEx("RequestDelegate", e);
                                //impt cant find path ,need tips 
                                context.Response.WriteAsync(e.Message);
                            }


                            Print(" end req...");
                            PrintTimestamp();
                        };
            app.Run(RequestDelegate1);
            app.Run();
        }


        public static void StartWebapiV2()
        {
            //  Action<HttpRequest, HttpResponse> httpHdlrSpel, 
            string api_fix = "Wapi";
            var builder = WebApplication.CreateBuilder();
            // Configure Kestrel to listen on a specific port
            ConfigureWebHostBuilder webHost = builder.WebHost;
            webHost.ConfigureLogging(logging => { });
            // webHost.UseStaticWebAssets
            webHost.ConfigureKestrel(serverOptions =>
            {
                Dictionary<string, string> map = GetDicFromIni($"{prjdir}/cfg/cfg.ini");
                int port = GetFieldAsInt526(map, "wbsvs_port", 5000);
                serverOptions.ListenAnyIP(port); // 自定义端口号，例如5001

                //--------cfg https block
                int https = GetFieldAsInt526(map, "https", 0);
                serverOptions.Limits.MaxRequestBodySize = 20 * 1024 * 1024; // 20 MB
                if (https == 1)
                    CfgHttps(serverOptions, map);
                //-----end cfg https
            });
            var app = builder.Build();
            //http://localhost:5000/dafen?callGetlistFromDb=yourValue11
            //拦截请求：
            RequestDelegate RequestDelegate1 = async (HttpContext context) =>
            {
                // todo  TryNotLgJmpEndAsync
                Action<HttpContext> a = async (HttpContext context) =>
                {

                };
                try
                {
                    //    a(context);
                    jmp2exitFlagInThrd.Value = false;
                    Print(" start req..."); PrintTimestamp();
                    //here cant new thrd..beir req close early
                    //here use call but not calltryAll bcs not want jmp Ex prt
                    // 确保 HttpHdlr 是异步的
                    await HttpHdlrV2(context.Request, context.Response);
                    Print(" end req...");
                    PrintTimestamp();
                }
                catch (jmp2endEx e)
                {

                }
                catch (Exception e)
                {
                    PrintCatchEx("RequestDelegate", e);
                    //impt cant find path ,need tips 
                    context.Response.WriteAsync(e.Message);
                }


                Print(" end req...");
                PrintTimestamp();
            };
            app.Run(RequestDelegate1);
            app.Run();
        }

        private static void CfgHttps(KestrelServerOptions serverOptions, Dictionary<string, string> map)
        {
            int httpsPort = GetFieldAsInt526(map, "https_port", 443); // Define your HTTPS port
            string https_cert_path = GetField(map, "https_cert_path");                                                  // Configure HTTPS
            var certPath = $"{prjdir}cfg\\" + https_cert_path;
            Print(certPath);
            var keypath = GetField(map, "https_cert_password");
            keypath = $"{prjdir}cfg\\private.key";
            //   certPassword = ReadAllText(certPassword);
            //    certPassword = "";
            Print("certPassword=>" + keypath);
            if (File.Exists(certPath))
            {
                serverOptions.ListenAnyIP(httpsPort, listenOptions =>
                {
                    Print("httpsPort=>" + httpsPort);
                    Print("certPath=>" + certPath); Print("keypath=>" + keypath);
                    listenOptions.UseHttps(certPath, keypath);
                });
                //serverOptions.ListenAnyIP(443, listenOptions =>
                //{
                //    listenOptions.UseHttps("path/to/your/certificate.crt", "path/to/your/private.key");
                //});
            }
            else
            {
                Console.WriteLine($"!!!Certificate file not found at: {certPath}");
            }
        }



        public static string webrootDir = $"{prjdir}/webroot";


        /*
         
           //txt   text / plain; charset = UTF - 8
            //pdf word zip file
            //css  text/css; charset=UTF-8
            //js   text/javascript
            //png   image/png
            //ico  image/x-icon
            // Invk(img_httpHdlrFilImg);
            // 静态资源处理函数调用
            object? audioRespAsync = null;
            extNhdlrChoosrMaplist.Add("mp3", audioRespAsync);
            object? videoRespAsync = null;
            extNhdlrChoosrMaplist.Add("mp4", videoRespAsync);
        
         
         */

        /*
                   //  response.AddHeader("Access-Control-Allow-Origin", "*");
            //response.ContentType = 内容类型和编码;
            //res.header('Access-Control-Allow-Origin', '*'); 
         */
        //  HttpResponse设置'Access-Control-Allow-Origin', '*
        /// <summary>
        /// httpHdlr
        /// </summary>
        /// <param name="httpHdlrApiSpecl"></param>
        /// <param name="context"></param>
        /// <param name="api_prefixDep"></param>
        public static async Task HttpHdlr(HttpRequest request, HttpResponse response, string api_prefixDep, Action<HttpRequest, HttpResponse> httpHdlrApiSpecl)
        {
            dbgpad = 0;
            var __METHOD__ = "HttpHdlr";
            PrintTimestamp($"enter {__METHOD__}() ");
            jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(__METHOD__, func_get_args("req,resp", api_prefixDep));
            var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Path;
            PrintLog(" request.Path =>" + request.Path);
            path = CastPathReal4biz(path);
            PrintLog(" CastPathReal4biz =>" + path);
            // 允许所有域名
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            //-------------whilte list url for pfm
            string skipFileRdUrlpath = "/getlist";
            HashSet<string> hs = new HashSet<string>();
            hs.Add(skipFileRdUrlpath);
            if (!hs.Contains(path))
            {
                //这里为了pfm ,so need jude skip fl hdr
                //----------------static res
                // 静态资源处理器映射表
                Hashtable extNhdlrChoosrMaplist = new Hashtable();
                extNhdlrChoosrMaplist.Add(" js", nameof(JsHttpHdlr));
                extNhdlrChoosrMaplist.Add(" css", nameof(CssHttpHdlr));
                extNhdlrChoosrMaplist.Add("txt   ", nameof(TxtHttpHdlr));
                extNhdlrChoosrMaplist.Add(" html htm", nameof(HtmlHttpHdlrfilTxtHtml));
                extNhdlrChoosrMaplist.Add("json", nameof(JsonFLhttpHdlrFilJson));
                extNhdlrChoosrMaplist.Add("jpg png", nameof(ImgHhttpHdlrFilImg));
                string path2 = request.Path;
                PrintTimestamp("bef call HttpHdlrFil");
                HttpHdlrFilss(request, response, extNhdlrChoosrMaplist);
                PrintTimestamp("end call HttpHdlrFil");
                if (jmp2exitFlagInThrd.Value == true)
                {
                    PrintLog("🛑🛑🛑!!* Jmp2end Frm =>" + jmp2endCurFunInThrd.Value);
                    return;
                }

            }

            //-------------------if spec  api
            // 处理特定API
            httpHdlrApiSpecl(request, response);
            if (jmp2exitFlagInThrd.Value == true)
                return;

            //----------doc
            string methd = request.Path;
            if (methd == "/swag")
            {
                var rzt146 = DocapiHttpHdlrApiSpelDocapi("mdsj.xml", response);
                SendResp(rzt146, "text/html; charset=utf-8", response);
                Jmp2end(nameof(HttpHdlr));
            }


            // upldPOSTWbapi
            //todo ref is too slow ,chg to delegate
            //------------httpHdlrApi--def json api mode
            //----------/XXX GET/POST WBAPI----
            //if (request.Method == HttpMethods.Post)

            var fn = GetFunFromPathUrl(path);

            var funname = "" + fn + request.Method + "Wbapi";
            //tod here maybe 50ms to dync invok ,can chg to delegate mode fast
            //but if no match method ,just not invk ,also fast..
            Callx(funname, request, response);

            //     arr_fltr330()
            //----dep
            //TODO DEP SHOULD AUTO get post call
            string fnm = api_prefixDep + fn;
            string args931 = Substring(queryString, 1);


            //   object rzt = CallxTryx(fnm, args931);
            // 使用表达式树创建委托
            var f = NewDelegate<Func<string, string>>(fnm);
            // 使用委托调用方法
            string result = f(args931);

            // 发送响应
            SendResp(result, "application/json; charset=utf-8", response);
            PrintRetx(__METHOD__, "");
            // 确保在处理完成后可以进行异步操作，如果有必要
            await Task.CompletedTask;
            PrintTimestamp(" end fun HttpHdlr()");
        }

        public static async Task HttpHdlrV2(HttpRequest request, HttpResponse response)
        {
            string api_prefixDep = "Wbapi";
            dbgpad = 0;
            var __METHOD__ = "HttpHdlr";
            PrintTimestamp($"enter {__METHOD__}() ");
            jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(__METHOD__, func_get_args("req,resp", api_prefixDep));
            var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Path;
            PrintLog(" request.Path =>" + request.Path);
            path = CastPathReal4biz(path);
            PrintLog(" CastPathReal4biz =>" + path);
            // 允许所有域名
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            //-------------whilte list url for pfm
            string skipFileRdUrlpath = "/getlist";
            HashSet<string> hs = new HashSet<string>();
            hs.Add(skipFileRdUrlpath);
            if (!hs.Contains(path))
            {
                //这里为了pfm ,so need jude skip fl hdr
                //----------------static res
                //    使用字典代替 Hashtable: Hashtable 是过时的，使用 Dictionary<string, string> 可以提高性能。
                // 静态资源处理器映射表
                var extNhdlrChoosrMaplist = new Dictionary<string, string>
                    {
                        { "js", nameof(JsHttpHdlr) },
                        { "css", nameof(CssHttpHdlr) },
                        { "txt", nameof(TxtHttpHdlr) },
                        { "html", nameof(HtmlHttpHdlrfilTxtHtml) },
                        { "json", nameof(JsonFLhttpHdlrFilJson) },
                        { "jpg", nameof(ImgHhttpHdlrFilImg) },
                        { "png", nameof(ImgHhttpHdlrFilImg) }
                    };
                var extNhdlrChoosrMaplistHstb = ToHashtableFrmDic(extNhdlrChoosrMaplist);
                string path2 = request.Path;
                PrintTimestamp("bef call HttpHdlrFil");
                HttpHdlrFilss(request, response, extNhdlrChoosrMaplistHstb);
                PrintTimestamp("end call HttpHdlrFil");
                if (jmp2exitFlagInThrd.Value == true)
                {
                    PrintLog("🛑🛑🛑!!* Jmp2end Frm =>" + jmp2endCurFunInThrd.Value);
                    return;
                }

            }

            //-------------------if spec  api
            // 处理特定API
            //httpHdlrApiSpecl(request, response);
            //if (jmp2exitFlagInThrd.Value == true)
            //    return;





            // upldPOSTWbapi
            //todo ref is too slow ,chg to delegate
            //------------httpHdlrApi--def json api mode
            //----------/XXX GET/POST WBAPI---           
            //----------doc  swagGetWbapi
            var fn = GetFunFromPathUrl(path);

            var funname = "" + fn + request.Method + "Wbapi";

            // 使用表达式树创建委托
            var f933 = NewDelegateV2<HttpHdlrDlgt>(funname);
            // 使用委托调用方法
            f933(request, response);
         //   f?.Invoke(request, response)) 更安全，因为它确保了只有在 f 不为 null 时才会执行 Invoke，从而避免了空引用异常。

            //     arr_fltr330()
            //----dep
            //TODO DEP SHOULD AUTO get post call
            string fnm = api_prefixDep + fn;
            string args931 = Substring(queryString, 1);


            //   object rzt = CallxTryx(fnm, args931);
            // 使用表达式树创建委托
            var f = NewDelegate<Func<string, string>>(fnm);
            // 使用委托调用方法
            string result = f(args931);

            // 发送响应
            SendResp(result, "application/json; charset=utf-8", response);
            PrintRetx(__METHOD__, "");
            // 确保在处理完成后可以进行异步操作，如果有必要
            await Task.CompletedTask;
            PrintTimestamp(" end fun HttpHdlr()");
        }

        public static Hashtable ToHashtableFrmDic(Dictionary<string, string> extNhdlrChoosrMaplist)
        {
            
            Hashtable hashtable = new Hashtable();

            // Iterate through the dictionary and add entries to the hashtable
            foreach (var kvp in extNhdlrChoosrMaplist)
            {
                hashtable.Add(kvp.Key, kvp.Value);
            }

            return hashtable;
        }

        public static async Task HttpHdlrApi(HttpRequest request, HttpResponse response)
        {
            dbgpad = 0;
            var __METHOD__ = "HttpHdlr";
            PrintTimestamp($"enter {__METHOD__}() ");
            jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(__METHOD__, func_get_args("req,resp"));
            var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Path;
            PrintLog(" request.Path =>" + request.Path);
            path = CastPathReal4biz(path);
            PrintLog(" CastPathReal4biz =>" + path);
            // 允许所有域名
            response.Headers.Add("Access-Control-Allow-Origin", "*");


            var fn = GetFunFromPathUrl(path);

            var funname = "" + fn + request.Method + "Wbapi";

            string args931 = Substring(queryString, 1);
            //   object rzt = CallxTryx(fnm, args931);
            // 使用表达式树创建委托
            var f = NewDelegate<Func<string, string>>(funname);
            // 使用委托调用方法
            string result = f(args931);

            // 发送响应
            SendResp(result, "application/json; charset=utf-8", response);
            PrintRetx(__METHOD__, "");
            // 确保在处理完成后可以进行异步操作，如果有必要
            await Task.CompletedTask;
            PrintTimestamp(" end fun HttpHdlr()");
        }

        public static string CastPathReal4biz(string path)
        {
            path = CastToPathReal(path);

            var nginccfg = "D:\\nginx-1.27.0\\conf\\nginx.conf";
            //    List<Hashtable> li = ParseNginxConfigV2(ReadAllText(nginccfg));
            if (path.StartsWith("/api/"))
                path = SubStr(path, 4);// rmv api
            return path;
        }






        /// <summary>
        /// 统一查询接口
        /// http://localhost:5000/qry?fromData=闲置
        /// http://localhost:5000/qry?fromData=招聘
        /// http://localhost:5000/qry?fromData=买号
        /// http://localhost:5000/qry?fromData=猎艳
        /// http://localhost:5000/qry?fromData=买号-购买记录
        /// 买号-购买记录
        ///    使用  /qry?path=pinlunDir评论数据/avymrhifuyzkfetlnifryraazk
        /// </summary>
        /// <param name="path">数据文件路径</param>
        /// <returns></returns>
        public static string WbapiXqry(string qrystr)
        {
            SortedList qrystrHstb = GetHashtableFromQrystr(qrystr);
            var li = ormJSonFL.QrySglFL($"{prjdir}/db/" + qrystrHstb["fromData"] + ".json");
            var list_rzt2 = SliceByPagemodeByQrystr(li, qrystr);

            return EncodeJson(list_rzt2);
        }

        /// <summary>
        /// 通用查询接口
        /// </summary>
        /// <param name="qrystr"></param>
        /// <returns></returns>
        public static string WbapiXgetlistData(string qrystr)
        {
            SortedList qrystrHstb = GetHashtableFromQrystr(qrystr);
            var li = ormJSonFL.QrySglFL($"{prjdir}/db/" + qrystrHstb["fromData"] + ".json");
            var list_rzt2 = SliceByPagemodeByQrystr(li, qrystr);

            return EncodeJson(list_rzt2);
        }

        public static string WbapiXqryBinDb(string qrystr)
        {
            SortedList qrystrHstb = GetHashtableFromQrystr(qrystr);
            var li = ormJSonFL.QrySglFL($"db555/" + qrystrHstb["fromData"] + ".json");
            return EncodeJson(li);
        }

        public delegate void HttpHdlrDlgt(HttpRequest request, HttpResponse response);
        /// <summary>
        /// httpHdlrFil
        /// </summary>
        /// <param name="context"></param>
        /// <param name="extnameNhdlrChooser"></param>
        public static void HttpHdlrFilss(HttpRequest request, HttpResponse response, Hashtable extnameNhdlrChooser)
        {
            var __METHOD__ = "HttpHdlrFil";
            PrintTimestamp($"enter {__METHOD__}() ");

            //MethodBase.GetCurrentMethod().Name;
            jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(__METHOD__, func_get_args("req,resp", extnameNhdlrChooser));
            var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Path;

            path = CastPathReal4biz(path);

            if (path.Contains("analytics"))
                Print("Dbg2432");

            ForeachHashtableFlgVer(extnameNhdlrChooser, (DictionaryEntry de) =>
        {
            string[] exts = de.Key.ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string ext in exts)
            {
                if (path.ToUpper().Trim().EndsWith("." + ext.Trim().ToUpper()))
                {
                    //   MethodInfo a
                    var typex = de.Value.GetType().ToString();
                    //  Delegate? value = (Delegate)de.Value;
                    //   callx(value, context);




                    var func1 = de.Value.ToString();
                    //todo here can chagng to delegate
                    // 使用表达式树创建委托
                    var f = NewDelegateV2<HttpHdlrDlgt>(func1);
                    // 使用委托调用方法
                    f(request, response);
                    //  var task = Callx(func1, request, response);

                    //  task.Wait();  
                    jmp2exitFlagInThrd.Value = true;
                    //  jmp2exitFlag = true;
                    break;
                    //  Jmp2end();

                }
            }



        });
            if (jmp2exitFlagInThrd.Value == true)
            {
                PrintRetx(__METHOD__, "");
                return;
            }

            //------------------other ext use down mode
            // 获取文件的实际扩展名
            string fileExtension = Path.GetExtension(path);
            string filePath = $"{webrootDir}{path}";
            filePath = CastNormalizePath(filePath);
            if (文件有扩展名(filePath) && 文件存在(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long fileSize = fileInfo.Length;
                if (fileSize < 1000 * 1000)
                {
                    HtmlHttpHdlrfilTxtHtml(request, response); ; return;
                }

                else
                {
                    FildownHttpHdlrFilDown(request, response); return;

                }

            }

            if (文件有扩展名(filePath) && 文件不存在(filePath))
            {
                发送响应_资源不存在(response);
                Jmp2end(nameof(HttpHdlrFilss));
            }



            //---------------- rest nt have ext
            //  var filepath = path.Substring(1);
            if (fileExtension == "" && File.Exists($"{prjdir}{path}"))
            {
                HtmlHttpHdlrfilTxtHtml(request, response);
                Jmp2end(nameof(HttpHdlrFilss));
            }
            PrintTimestamp(" end HttpHdlrFil() ");

        }



        //----------------------------------swag
        // Add services to the container.
        //  builder.Services.AddSwagger();

        //   var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //    app.UseSwagger();
        //    app.UseSwaggerUI(c =>
        //    {
        //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini API V1");
        //    });
        //}
        //--------------------end swag----
        //   public static string staticResDir = $"{prjdir}/webroot";
        public static async System.Threading.Tasks.Task HtmlHttpHdlrfilTxtHtml(HttpRequest request, HttpResponse response)
        {
            // 获取当前请求的 URL

            string path = request.Path;
            // if (path.EndsWith(".htm"))
            {
                // 设置响应内容类型和编码

                string f = webrootDir + DecodeUrl(path);
                object rzt2 = ReadAllText(f);
                SendResp(rzt2, "text/html; charset=utf-8", response);
                Jmp2endDep(); return;
            }
        }
        public static void CssHttpHdlr
     (HttpRequest request, HttpResponse response)
        {
            // 获取当前请求的 URL

            string path = request.Path;
            // if (path.EndsWith(".htm"))
            {
                // 设置响应内容类型和编码            

                string f = webrootDir + DecodeUrl(path);
                object rzt2 = ReadAllText(f);
                SendResp(rzt2, "text/css; charset=utf-8", response);
                Print(" send finish....");
                Jmp2endDep(); return;
            }
        }

        public static void JsHttpHdlr
         (HttpRequest request, HttpResponse response)
        {
            // 获取当前请求的 URL

            string path = request.Path;
            // if (path.EndsWith(".htm"))
            {
                // 设置响应内容类型和编码            

                string f = webrootDir + DecodeUrl(path);
                object rzt2 = ReadAllText(f);
                SendResp(rzt2, "text/javascript; charset=utf-8", response);
                Print(" send finish....");
                Jmp2endDep(); return;
            }
        }

        public static void TxtHttpHdlr
            (HttpRequest request, HttpResponse response)
        {
            // 获取当前请求的 URL

            string path = request.Path;
            // if (path.EndsWith(".htm"))
            {
                // 设置响应内容类型和编码            

                string f = webrootDir + DecodeUrl(path);
                object rzt2 = ReadAllText(f);
                SendResp(rzt2, "text/plain; charset=utf-8", response);
                Print(" send finish....");
                Jmp2endDep(); return;
            }
        }
        public static void swagGETWbapi(HttpRequest request, HttpResponse response)
        {
            var rzt146 = DocapiHttpHdlrApiSpelDocapi("mdsj.xml", response);
            SendResp(rzt146, "text/html; charset=utf-8", response);
            // Jmp2end(nameof(HttpHdlr));
            Jmp2end(nameof(swagGETWbapi));
        }

        public static void upldPOSTWbapi(HttpRequest request, HttpResponse response)
        {
            // Check if the request contains a file
            var fil = "";
            SortedList saveOBJ = ConvertFormToSortedList(request.Form);
            if (request.Form.Files.Count > 0)
            {
                foreach (var file in request.Form.Files)
                {
                    // Get the file content and save it to a desired location
                    var filePath = Path.Combine(saveOBJ["uppath"].ToString(), file.FileName);
                  
                    DelFil(filePath);
                    fil = filePath;
                    Mkdir4File(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyToAsync(stream).GetAwaiter().GetResult();
                    }
                }
            }

          


            Jmp2end("upldPOSTWbapi");
        }

        private static void DelFil(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }catch(Exception e)
            {

            }
           
        }

        public static async System.Threading.Tasks.Task FildownHttpHdlrFilDown(HttpRequest request, HttpResponse response)
        {


            string path = request.Path;
            // 设置响应内容类型和编码


            //    HttpListenerResponse response = (HttpListenerResponse)response2;
            string path1 = webrootDir + path;
            path1 = DecodeUrl(path1);
            if (!ExistFil(path1))
            {
                SendRespsJJJresNotExist404(response);
                return;
            }

            SendResps4file(path1, "application/octet-stream", response);
            Jmp2endDep();
            return;


        }



        public static async System.Threading.Tasks.Task ImgHhttpHdlrFilImg(HttpRequest request, HttpResponse response)
        {
            var args = func_get_args(request, response);
            jmp2endCurFunInThrd.Value = nameof(ImgHhttpHdlrFilImg);

            string path = request.Path;
            //   path1 = DecodeUrl(path1);
            path = CastPathReal4biz(path);
            string path1 = webrootDir + path;


            if (!ExistFil(path1))
            {
                SendRespsJJJresNotExist404(response);
                //    发送响应_资源不存在(HTTP响应对象);
                Jmp2endDep();
                return;
            }

            await SendResps4file(path1, "image/jpeg", response);
            jmp2exitFlagInThrd.Value = true;
            //here use retval ,not ex,bcs ex atlsest need  50ms;
            //     Jmp2end(nameof(ImgHhttpHdlrFilImg));
            return;
        }

        private static async System.Threading.Tasks.Task SendResps4file(string path1, string contType, HttpResponse response)
        {
            byte[] buffer = File.ReadAllBytes(path1);
            response.ContentType = contType;
            response.ContentLength = buffer.Length;
            //   await response.WriteAsync(buffer, 0, buffer.Length);
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
            response.Body.Close();
        }

        /// <summary>
        /// httpHdlrFilJson
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task JsonFLhttpHdlrFilJson(HttpRequest request, HttpResponse response)
        {
            Print(" JsonFLhttpHdlrFilJson()...start");
            PrintTimestamp();
            // 获取当前请求的 URL
            //  var request = context.Request;
            string path = request.Path;
            // 设置响应内容类型和编码
            response.ContentType = "application/json; charset=utf-8";
            path = CastPathReal4biz(path);

            string pathDsk = webrootDir + path;
            if (IsExistFil(pathDsk))
            {
                object rzt2 = ReadAllText(pathDsk);
                await response.WriteAsync(rzt2.ToString(), Encoding.UTF8);
            }
            else
            {
                PrintWarn("file not exist!" + pathDsk);
                await response.WriteAsync("file not exist!" + pathDsk, Encoding.UTF8);
            }

            //   Print();

            PrintTimestamp(" JsonFLhttpHdlrFilJson()...end");
            jmp2exitFlagInThrd.Value = true;
            //  Jmp2end();
            return;

        }




        /// <summary>
        /// httpHdlrApiSpelDocapi
        /// </summary>
        /// <param name="xmlpath"></param>
        /// <param name="context"></param>
        /// <returns>doc html</returns>
        public static string DocapiHttpHdlrApiSpelDocapi(string xmlpath, HttpResponse context)
        {
            context.ContentType = "text/html; charset=utf-8";
            //shangjiaID,uid,dafen
            //     SortedList dafenObj = getHstbFromQrystr(qrystr);
            //   ormJSonFL.save(dafenObj, "dafenDatadir/" + dafenObj["shangjiaID"] + ".json");
            ConvertXmlToHtml(xmlpath, xmlpath + ".htm");
            return ReadAllText(xmlpath + ".htm");
        }

    }
}
