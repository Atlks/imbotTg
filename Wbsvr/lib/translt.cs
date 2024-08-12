global using static prjx.libx.translt;
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class translt
    {
        static Hashtable dicWord5k = GetHashtabFromIniFl("wdlib.enNcn5k.v2.ini");

        //public static void transltTest()
        //{
        //    List<string> liRzt = new List<string>();
        // //   FolderPath = "downHtmldir";
        //    List<string> li = loadLstWdsFrmDataDirHtml("spdr/downHtmtTaskQue");
        //    List<string> li2 = loadLstWdsFrmDataDirHtml("spdr/downHtmldirLog");
        //    li = array_merge(li, li2);
        //    //  List<string> li = getListFrmFil($"{prjdir}/cfg/testart/t3.txt");
        //    li = RemoveEmptyElements(li);
        //    foreach_listStr(li, (string line) =>
        //    {
        //        string transed = transedE2Cn(line);
        //        liRzt.Add(transed);
        //    });
        //    //get misswdFmt.txt
        //    string rzt = JoinStringsWithNewlines(liRzt);
        //    WriteAllText("transed.txt", rzt);
        //    Print(rzt);
        //}

        private static List<string> RemoveEmptyElements(List<string> li)
        {
            throw new NotImplementedException();
        }

        //  private const string



        //private static string transedE2Cn(string line)
        //{
        //    List<string> liRzt = new List<string>();
        //    string[] wds = Tokenize(line);
        //    foreach (string wd in wds)
        //    {
        //        if (wd.Contains("sibl"))
        //            Print("dbg");
        //        if (wd.Contains("ontivero"))
        //            Print("dbg");
        //        transE2cn4wd(liRzt, wd);

        //    }
        //    // 将 List<string> 转换为用空格隔开的字符串
        //    string result = String.Join("    ", liRzt);
        //    return result;

        //}

      public  static HashSet<string> hs_mswd = new HashSet<string>();

        //private static void transE2cn4wd(List<string> liRzt, string wd)
        //{
        //    if (IsNumeric(wd))
        //    { //数字
        //        liRzt.Add(wd);
        //        return;

        //    }
        //    if (!IsWord(wd))
        //    {
        //        // Or   标点
        //        //标点
        //        liRzt.Add(wd);
        //    }
        //    //is word
        //    if (IsStartsWithUppercase(wd))
        //    {
        //        string wdRoot = GetRoot(wd);
        //       // string cnWd = transE2cWd(wdRoot);
        //        liRzt.Add(wd + "");
        //        return;
        //    }

        //    //normal word
        //    if (wd.Length <= 3)
        //    {
        //        liRzt.Add(wd + "");
        //        return;
        //    }

        //    string cnWd = transE2cWd(wd);
        //    if (cnWd != "")
        //    {
        //        //普通单词找到对于翻译词了
        //        liRzt.Add(wd + "[" + cnWd + "." + wd + "]");
        //        return;
        //    }
        //    else
        //    {
        //        //非普通单词，找不到翻译词，尝试 使用词干法
        //        string wdRoot = GetRoot(wd);
        //        cnWd = transE2cWd(wdRoot);
        //        if(cnWd=="")
        //        {
        //            //log miss wd
        //            logF($"\n{wd}=\n{wdRoot}=","misswd.log");
        //            //for misswd.log then trans fill to wdlib.ini
        //            hs_mswd.Add(wd + ""); hs_mswd.Add(wdRoot + "");
        //        }
        //        liRzt.Add(wd + "[" + cnWd + "." + wdRoot + "]");
        //        return;
        //    }





        //}

  
        static string[] Tokenize(string input)
        {
            // 定义正则表达式，匹配单词和标点符号
            string pattern = @"([\w']+|[^\w\s])";
            Regex regex = new Regex(pattern);

            // 使用正则表达式拆分输入字符串
            MatchCollection matches = regex.Matches(input);

            // 构建结果数组
            string[] tokens = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                tokens[i] = matches[i].Value;
            }

            return tokens;
        }
        private static string transE2cWd(string wdOrwdRoot)
        {

            //     WriteAllText("wd5000.json", encodeJson(dic));
            // dic = ToLower(dic);
            return (string)LoadField(dicWord5k, wdOrwdRoot, "");
        }

      

      
    
    }
}
