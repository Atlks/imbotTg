//using DocumentFormat.OpenXml.Vml.Office;
using Newtonsoft.Json;
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static prjx.libx.arrCls;//  prj202405.lib
using static prjx.libx.dbgCls;
using static prjx.libx.logCls;
using System.Reflection;

namespace prjx.libx
{
    internal class ormIni
    {
          

        public static void saveIni(SortedList<string,string> sortedList, string Strfile)
        {
            //if (!File.Exists(Strfile))
            //    File.WriteAllText(Strfile, "[]");
            const bool Append = true;
            // 使用StreamWriter追加写入文件
            using (StreamWriter writer = new StreamWriter(Strfile, Append, Encoding.UTF8))
            {
                if(sortedList.ContainsKey("id"))
                   writer.WriteLine($"\n\n[{sortedList["id"]}]");
                foreach (KeyValuePair<string,string> entry in sortedList)
                {
                    writer.WriteLine($"{entry.Key}={entry.Value}");
                }
            }
        }

        public static void save(SortedList sortedList, string Strfile)
        {
            //if (!File.Exists(Strfile))
            //    File.WriteAllText(Strfile, "[]");

            // 使用StreamWriter追加写入文件
            using (StreamWriter writer = new StreamWriter(Strfile, true, Encoding.UTF8))
            {
                writer.WriteLine($"\n\n[{sortedList["id"]}]");
                foreach (DictionaryEntry entry in sortedList)
                {
                    writer.WriteLine($"{entry.Key}={entry.Value}");
                }
            }
        }


        public static List<Dictionary<string, string>> qry(string iniFilePath)
        {
            var dataList = new List<Dictionary<string, string>>();
            var currentSection = new Dictionary<string, string>();
            var sectionName = string.Empty;

            foreach (var line in File.ReadLines(iniFilePath))
            {
                var trimmedLine = line.Trim();

                // 忽略空行和注释行
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // 处理节（section）
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    // 如果当前节非空，添加到列表中
                    if (currentSection.Count > 0)
                    {
                        dataList.Add(currentSection);
                        currentSection = new Dictionary<string, string>();
                    }

                    sectionName = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    currentSection["SectionName"] = sectionName;
                }
                else
                {
                    // 处理键值对
                    var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        currentSection[key] = value;
                    }
                }
            }

            // 添加最后一个节到列表中
            if (currentSection.Count > 0)
            {
                dataList.Add(currentSection);
            }

            return dataList;
        }

        public static List<SortedList> qryV2(string iniFilePath)
        {


            if (!File.Exists(iniFilePath))
                File.WriteAllText(iniFilePath, "");
            var dataList = new List<SortedList>();
            var currentSection = new SortedList();
            var sectionName = string.Empty;

            foreach (var line in File.ReadLines(iniFilePath))
            {
                var trimmedLine = line.Trim();

                // 忽略空行和注释行
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // 处理节（section）
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    // 如果当前节非空，添加到列表中
                    if (currentSection.Count > 0)
                    {
                        dataList.Add(currentSection);
                        currentSection = new SortedList();
                    }

                    sectionName = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    currentSection["SectionName"] = sectionName;
                }
                else
                {
                    // 处理键值对
                    var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        currentSection[key] = value;
                    }
                }
            }

            // 添加最后一个节到列表中
            if (currentSection.Count > 0)
            {
                dataList.Add(currentSection);
            }

            return dataList;
        }

        public static List<Dictionary<string, string>> qryToDic(string iniFilePath)
        {


            if (!File.Exists(iniFilePath))
                File.WriteAllText(iniFilePath, "");
            var dataList = new List<Dictionary<string, string>>();
            var currentSection = new Dictionary<string, string>();
            var sectionName = string.Empty;

            foreach (var line in File.ReadLines(iniFilePath))
            {
                var trimmedLine = line.Trim();

                // 忽略空行和注释行
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // 处理节（section）
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    // 如果当前节非空，添加到列表中
                    if (currentSection.Count > 0)
                    {
                        dataList.Add(currentSection);
                        currentSection = new Dictionary<string, string>();
                    }

                    sectionName = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    currentSection["SectionName"] = sectionName;
                }
                else
                {
                    // 处理键值对
                    var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        currentSection[key] = value;
                    }
                }
            }

            // 添加最后一个节到列表中
            if (currentSection.Count > 0)
            {
                dataList.Add(currentSection);
            }

            return dataList;
        }

        internal static void saveRplsMlt(List<SortedList> lst_hash, string Strfile)
        {
            List<SortedList> list = qryV2(Strfile);
            SortedList listIot = db.lst2IOT(list);
            foreach (SortedList objSave in lst_hash)
            {
                SetFieldReplaceKeyV(listIot, LoadFieldTryGetValueAsStrDefNull(objSave, "id"), objSave);
            }
            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            wriToDbf(saveList_hpmod, Strfile);
        }

        private static void wriToDbf(ArrayList saveList_hpmod, string strfile)
        {
            const string logdir = "errlogDir";
            var __METHOD__ = MethodBase.GetCurrentMethod().Name+ $"({strfile})";
         //   dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), msg, whereExprs, dbf));

            Directory.CreateDirectory(logdir);
            File.Delete(strfile);
            // 使用StreamWriter追加写入文件
            using (StreamWriter writer = new StreamWriter(strfile, true, Encoding.UTF8))
            {
                foreach (SortedList objSave in saveList_hpmod)
                {
                    try
                    {
                        writer.WriteLine($"\n\n[{objSave["id"]}]");
                        foreach (DictionaryEntry entry in objSave)
                        {
                            try
                            {
                                writer.WriteLine($"{entry.Key}={entry.Value}");
                            }
                            catch (Exception e)
                            {
                              
                                logErr2025(e, __METHOD__, logdir);
                         
                            }

                        }
                    }catch (Exception e) { logErr2025(e, __METHOD__, logdir); }
                    
                }
            }
        }

        //public static void logErr2025(Exception e, string logdir)
        //{
        //    // 获取当前时间并格式化为文件名
        //    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        //    string fileName = $"{logdir}/{timestamp}.txt";
        //    File.WriteAllText(fileName, e.ToString());
        //}
    }
}
