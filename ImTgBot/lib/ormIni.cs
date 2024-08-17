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
using System.IO.Compression;

namespace prjx.libx
{
    internal class ormIni
    {
        public static void Save2Ini(SortedList sortedList, string dir)
        {
            //if (!File.Exists(Strfile))
            //    File.WriteAllText(Strfile, "[]");
            const bool Append = false;
            if (!sortedList.ContainsKey("id"))
                sortedList.Add("id", GetUuid());
            string Strfile = $"{dir}/id_" + GetFieldAsStr(sortedList, "id") + ".ini";
            Mkdir4File(Strfile);
            // 使用StreamWriter追加写入文件
            using (StreamWriter writer = new StreamWriter(Strfile, Append, Encoding.UTF8))
            {
                // smp mode
                //  if (sortedList.ContainsKey("id"))
                //     writer.WriteLine($"\n\n[{sortedList["id"]}]");
                foreach (DictionaryEntry entry in sortedList)
                {
                    writer.WriteLine($"{entry.Key}={entry.Value}");
                }
            }
        }

        public static void saveIni(SortedList<string, string> sortedList, string Strfile)
        {
            //if (!File.Exists(Strfile))
            //    File.WriteAllText(Strfile, "[]");
            const bool Append = true;
            // 使用StreamWriter追加写入文件
            using (StreamWriter writer = new StreamWriter(Strfile, Append, Encoding.UTF8))
            {
                if (sortedList.ContainsKey("id"))
                    writer.WriteLine($"\n\n[{sortedList["id"]}]");
                foreach (KeyValuePair<string, string> entry in sortedList)
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

        static List<SortedList<string, string>> ReadIniFilesFromZips(string directoryPath)
        {
            List<SortedList<string, string>> iniFilesData = new List<SortedList<string, string>>();

            // 获取指定目录下的所有zip文件
            var zipFiles = Directory.GetFiles(directoryPath, "*.zip");

            foreach (var zipFile in zipFiles)
            {
                // 读取zip文件
                using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(".ini", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var stream = entry.Open())
                            using (var reader = new StreamReader(stream))
                            {
                                SortedList<string, string> iniData = new SortedList<string, string>();

                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    // 假设ini文件格式是 key=value
                                    var parts = line.Split(new[] { '=' }, 2);
                                    if (parts.Length == 2)
                                    {
                                        iniData[parts[0].Trim()] = parts[1].Trim();
                                    }
                                }

                                iniFilesData.Add(iniData);
                            }
                        }
                    }
                }
            }

            return iniFilesData;
        }

        static List<SortedList<string, string>> MergeLists(List<SortedList<string, string>> list1, List<SortedList<string, string>> list2)
        {
            List<SortedList<string, string>> mergedList = new List<SortedList<string, string>>();

            var idhs = new HashSet<string>();
            // 合并第一个列表中的所有SortedList
            foreach (var sortedList in list1)
            {
                idhs.Add(sortedList["id"]);
                mergedList.Add(new SortedList<string, string>(sortedList));
            }

            // 合并第二个列表中的所有SortedList
            foreach (var sortedList in list2)
            {
                // 处理相同key的情况
                if (idhs.Contains(sortedList["id"]))
                    continue;

                // 如果没有相同的key，直接添加
                mergedList.Add(new SortedList<string, string>(sortedList));
            }

            return mergedList;
        }
        public static List<SortedList<string, string>> Qry(string dir)
        {
            var liFrmZip = ReadIniFilesFromZips(dir);
            List<SortedList<string, string>> liFrmINis = ReadFrmINiFils(dir);

            return MergeLists(liFrmINis, liFrmZip);
        }

        /// <summary>
        /// 3. I/O 操作模式：
      //  并行处理：如果直接从文件夹中读取文件，你可以通过并行读取来提高速度，
      //  充分利用多核 CPU。Zip 文件的读取虽然也可以并行处理，但受到解压缩和文件流操作的限制。
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static List<SortedList<string, string>> ReadFrmINiFils(string dir)
        {
            var liFrmINis = new List<SortedList<string, string>>();

            // 获取目录下的所有 .ini 文件
            string[] iniFiles = Directory.GetFiles(dir, "*.ini");

            foreach (string file in iniFiles)
            {
                var sortedList = new SortedList<string, string>();

                // 读取文件的每一行
                string[] lines = File.ReadAllLines(file);

                foreach (string line in lines)
                {
                    // 跳过空行和注释行
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                        continue;

                    // 解析键值对
                    string[] keyValue = line.Split(new char[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        sortedList[key] = value;
                    }
                }

                liFrmINis.Add(sortedList);
            }

            return liFrmINis;
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
            var __METHOD__ = MethodBase.GetCurrentMethod().Name + $"({strfile})";
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
                    }
                    catch (Exception e) { logErr2025(e, __METHOD__, logdir); }

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
