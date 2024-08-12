 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static prjx.libx.dsl;
namespace prjx.libx
{
    internal class dsl
    {
        static int dsl_linenum = 0;
        static string[] dsl_lines = [];
        public static void parse_str_dsl()
        {
            // 读取文件中的所有行
            dsl_lines = System.IO.File.ReadAllLines("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\dsl.txt");

            while (true)
            {

                if (dsl_linenum >= dsl_lines.Length)
                    break;
                string line = dsl_lines[dsl_linenum];
                echo(line);
                dsl_parseLine(line, dsl_linenum, dsl_lines);

            }
        }

        private static void dsl_parseLine(string line, int linenum, string[] lines)
        {
            //  print($"FUN dsl_parseLine() line:${line} linenum:${linenum}");
            line = line.Trim();
            if (line.Length == 0)
            {
                dsl_linenum++;
                return;
            }
            string[] wds = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string fun = wds[0].ToLower().Trim();
            if (fun == "let")
                dsl_funEvt_let(wds);
            if (fun == "echo")
            {
                dsl_funEvt_echo(line);
                return;
            }

            if (fun == "if")
            {
                dsl_funEvt_if(line, lines); return;
            }

            if (fun == "else")
            {
                dsl_funEvt_else(line); return;
            }

            if (fun == "endif")
            {
                dsl_funEvt_endif(line); return;
            }

            if (fun == "echo")
                dsl_funEvt_echo(line);
            if (fun == "fun")
                dsl_funEvt_fun(line);
            if (fun == "call")
                dsl_funEvt_call(line);

            //  print($"ENDFUN dsl_parseLine()  ");
        }

        private static void dsl_funEvt_call(string line)
        {
            dsl_linenum++;
        }

        private static void dsl_funEvt_fun(string line)
        {
            int curLineNum = dsl_linenum;
            // 使用 LINQ 的 Skip 方法截取数组
            string[] newArray = dsl_lines.Skip(dsl_linenum).ToArray();
            int idx = FindLineStartingWithElse(newArray, "endfun");
            dsl_linenum = curLineNum + idx + 1;
        }

        private static void dsl_funEvt_endif(string line)
        {
            dsl_linenum++;
        }

        private static void dsl_funEvt_else(string line)
        {
            int curLineNum = dsl_linenum;
            // 使用 LINQ 的 Skip 方法截取数组
            string[] newArray = dsl_lines.Skip(dsl_linenum).ToArray();
            int idx = FindLineStartingWithElse(newArray, "endif");
            dsl_linenum = curLineNum + idx + 1;
            dsl_linenum++;
        }

        private static void dsl_funEvt_if(string line, string[] lines)
        {
            string exprs = line.Substring(3);
            if (isBoolParse(exprs))
            {
                //JumpIfTrue
                dsl_linenum++;
            }
            else
            {
                JumpIfFalse(lines);
            }

        }

        //else evt
        private static void JumpIfFalse(string[] lines)
        {
            int curLineNum = dsl_linenum;
            // 使用 LINQ 的 Skip 方法截取数组
            string[] newArray = dsl_lines.Skip(curLineNum).ToArray();
            int idx = FindLineStartingWithElse(newArray, "else");
            dsl_linenum = curLineNum + idx + 1;

        }

        static int FindLineStartingWithElse(string[] lines, string startWithWd)
        {
            // 遍历字符串数组
            for (int i = 0; i < lines.Length; i++)
            {
                // 检查当前行是否以 "else" 开头
                //const string startWithWd = "else";
                if (lines[i].Trim().StartsWith(startWithWd, StringComparison.OrdinalIgnoreCase))
                {
                    return i; // 返回行号（索引）
                }
            }

            return -1; // 如果没有找到，返回 -1
        }

        private static bool isBoolParse(string exprs)
        {
            return true;
        }

        private static void dsl_funEvt_echo(string line)
        {
            line = line.Trim();
            echo(line.Substring(5)); dsl_linenum++;
        }

        public static SortedList varlst = new SortedList();

        private static void dsl_funEvt_let(string[] wds)
        {
            string varname = wds[1];
            string varval = wds[2];
            StaNamedProperty(varlst, varname, varval);
            dsl_linenum++;
        }

        //js op op 
        public static void StaNamedProperty(SortedList varlst, string varname, string varval)
        {
            if (varlst.ContainsKey(varname))
                varlst[varname] = varval;
            else
                varlst.Add(varname, varval);
        }

        public static void echo(string line)
        {
            Print(line);
        }
        public static void JumpLoop指令()
        {

        }
    }
}
