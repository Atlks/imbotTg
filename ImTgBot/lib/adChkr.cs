using prjx.libx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using City = prjx.City;
using static prjx.libx.arrCls;//  prj202405.lib
using static prjx.libx.dbgCls;
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
//using static prjx.libBiz.strBiz;
//using static prjx.libBiz.tgBiz;
using static prjx.libx.strCls;
using static prjx.libx.corex;
using static prjx.libx.db;
using static prjx.libx.filex;
using static prjx.libx.ormJSonFL;
using static prjx.libx.strCls;
using static prjx.libx.bscEncdCls;
using static prjx.libx.net_http;

//using static prjx.libBiz.tgBiz;
using static prjx.libx.tglib;
using static prjx.libx.adChkr;
using System.Reflection;
namespace prjx.libx
{
    internal class adChkr
    {

        public static void logic_chkad(string text, string uid, long grpid, Action act)
        {
            var __METHOD__ = "logic_chkad";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(  text, uid, grpid));

            try
            {
                string prjdir = @"../../../";
                prjdir = filex.GetAbsolutePath(prjdir);
                string adwdlib = $"{prjdir}/gbwd垃圾关键词词库/ads_word.txt";
                HashSet<string> adwds = SplitFileByChrs(adwdlib, ",\r \n");
                int ctnScr = containCalcCntScoreSetfmt(text, adwds);

               Print("广告词包含分数=》" + ctnScr);


                if (text.Length < 10)
                    return;
                string timestampMM = DateTime.Now.ToString("MM");
                string fnameFrmTxt = ConvertToValidFileName(text);
               Print("fnameFrmTxt=>" + fnameFrmTxt);
                string fname = $"adchkDir/uid{uid}_grp{grpid}_Dt{timestampMM}___" + Sub1109 (fnameFrmTxt,0, 50) + ".txt";
                if (System.IO.File.Exists(fname))
                {
                   Print("是重复消息了" + fname);
                    file_put_contents(fname, "\n\n" + text + "", true);


                    act();


                }
                else
                    file_put_contents(fname, "\n\n" + text, true);
            }
            catch(Exception e)
            {
               Print("catch in ()=>" + __METHOD__ + "()");
               Print(e);
            }
          

            dbgCls.PrintRet(__METHOD__, 0);
        }

        private static int containCalcCntScoreSetfmt(string text, HashSet<string> adwds)
        {
            throw new NotImplementedException();
        }
    }
}
