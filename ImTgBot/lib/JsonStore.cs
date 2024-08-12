global using static prjx.libx.JsonStore;
using Newtonsoft.Json;
using prjx.libx;
 
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace prjx.libx
{
    internal class JsonStore
    {
        private static readonly int MaxFileCount = 30000; // 文件数量阈值
                                                          //     private static readonly string DirectoryPath = @"C:\YourDirectory"; // 监控目录路径
                                                          //      private static readonly string ZipOutputPath = @"C:\YourZipOutputDirectory"; // ZIP 文件输出目录

        public static ConcurrentDictionary<string, List<SortedList>> dbCache = new ConcurrentDictionary<string, List<SortedList>>();

        public static ConcurrentDictionary<string, System.Timers.Timer> tmrList = new ConcurrentDictionary<string, System.Timers.Timer>();
        //   private static

        public static System.Timers.Timer TimerStart4zipFls(string dataDir)
        {
            System.Timers.Timer timer;
            timer = new System.Timers.Timer(60000); // 每60秒检查一次
            timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                string[] files = Directory.GetFiles(dataDir);
                if (files.Length < MaxFileCount)
                    return;

                // 按添加时间排序
                var sortedFiles = files
                    .Select(file => new FileInfo(file))
                    .OrderBy(fileInfo => fileInfo.CreationTime)
                    .ToList();

                int zipFileIndex = 1;
                int fileCountPerZip = 30000; // 每个ZIP文件的文件数量

                while (sortedFiles.Count > 0)
                {
                    string zipFileName = $"{zipFileIndex:D3}.zip";
                    string zipFilePath = Path.Combine(dataDir, zipFileName);

                    using (FileStream zipToCreate = new FileStream(zipFilePath, FileMode.Create))
                    using (ZipArchive archive = new ZipArchive(zipToCreate, ZipArchiveMode.Create))
                    {
                        for (int i = 0; i < fileCountPerZip && sortedFiles.Count > 0; i++)
                        {
                            var file = sortedFiles.First();
                            sortedFiles.RemoveAt(0);

                            ZipArchiveEntry entry = archive.CreateEntry(Path.GetFileName(file.FullName));
                            using (Stream entryStream = entry.Open())
                            using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open))
                            {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }

                    zipFileIndex++;
                }

                Console.WriteLine("Files have been archived.");
                // end  timer.Elapsed
            };            ;
            timer.AutoReset = true;
            timer.Enabled = true;

            Console.WriteLine("Monitoring started...");
            Console.ReadLine(); // Keep the application running
            return timer;

        }

        //    private static void CheckDirectory


        /// <summary>
        /// 2. 并行处理
        //   使用并行处理来同时读取多个文件。这可以通过 Parallel.ForEach 或 Task.WhenAll 来实现。
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static async Task<List<SortedList>> ListFromDirJsonsAsync(string dir)
        {
            //asyn to imprv pfm
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            __METHOD__ = GetOriginalMethodName();

            dbgCls.PrintCallFunArgs(__METHOD__, func_get_args(dir));
            //     需要读取的时候，优先读取zip文件内容。然后在加载目录下其他文件。
            //if many file >3w or 5w files..
          //  List<SortedList> arr = new List<SortedList>();
            if (!dbCache.TryGetValue(dir, out List<SortedList> arr))
            {
                arr = new List<SortedList>();
                // 获取目录中的所有文件
                string[] filePaths = Directory.GetFiles(dir);

                //并行处理  imprv pfm
                await Task.WhenAll(filePaths.Select(async filePath =>
                {
                    // 将JSON字符串转换为List<Dictionary<string, object>>
                    //  string txt = File.ReadAllText(dbf);
                    string txt = await File.ReadAllTextAsync(filePath);
                    if (txt.Trim().Length == 0)
                        txt = "{}";
                    SortedList obj = JsonConvert.DeserializeObject<SortedList>(txt);
                    arr.Add(obj);
                }));
             //   dbCache.AddIfNotContainsKey(dir, arr);

                //   dbCache[dir] = arr;
            }
            if (!tmrList.TryGetValue(dir, out System.Timers.Timer tmr))
            {
             //   TimerStart534(dir);
            }


                PrintRet(__METHOD__, ArrSlice(arr, 0, 1));


            return dbCache[dir];
        }


        public static void SaveToJsonSngleFile(SortedList SortedList1, string dir)
        {
            if (LoadFieldFrmStlst(SortedList1, "id", "") == "")
                SetField938(SortedList1, "id", dtime.uuidYYMMDDhhmmssfff());
            var dbfile = dir + "/id_" + SortedList1["id"] + ".json";
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(SortedList1, dbfile));

            //------------wrt to cache
            List<SortedList> li127 =   ListFromDirJsonsAsync(dir).Result;
            SetElmt(li127, SortedList1);



            //-------------wrt to file
            try
            {

                Mkdir4File(dbfile);
                // 创建目录
                // 使用 Path.GetDirectoryName 方法获取目录路径
                string directoryPath = System.IO.Path.GetDirectoryName(dbfile);

                // 检查目录是否存在
                if (directoryPath.Length > 0)
                    if (!Directory.Exists(directoryPath))
                    {
                        if (directoryPath.Length > 5)  //maybe relt path is empty
                        {
                            // 创建目录及所有上级目录
                            Directory.CreateDirectory(directoryPath);
                            Print($"Created directory: {directoryPath}");
                        }

                    }
                //  Directory.CreateDirectory(logdir);
                // 将JSON字符串转换为List<Dictionary<string, object>>


                string key = SortedList1["id"].ToString();
                //  SetField938(iotTable, key, SortedList1);

                wriToDbf((SortedList1), dbfile);

            }
            catch (Exception e)
            {

                PrintExcept(__METHOD__, e);
            }



            PrintRet(__METHOD__, 0);
        }

        /// <summary>
        ///  merge mode
        /// </summary>
        /// <param name="SortedList1"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static void UpdtSqlmode(SortedList SortedList1, string dir)
        {
            if (LoadFieldFrmStlst(SortedList1, "id", "") == "")
                SetField938(SortedList1, "id", dtime.uuidYYMMDDhhmmssfff());
            var dbfile = dir + "/id_" + SortedList1["id"] + ".json";
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(SortedList1, dbfile));

            //------------wrt to cache
            List<SortedList> li127 =   ListFromDirJsonsAsync(dir).Result;
            UpdtElmtSqlmode(li127, SortedList1);



            //-------------wrt to file
            try
            {

                Mkdir4File(dbfile);
                // 创建目录
                // 使用 Path.GetDirectoryName 方法获取目录路径
                string directoryPath = System.IO.Path.GetDirectoryName(dbfile);

                // 检查目录是否存在
                if (directoryPath.Length > 0)
                    if (!Directory.Exists(directoryPath))
                    {
                        if (directoryPath.Length > 5)  //maybe relt path is empty
                        {
                            // 创建目录及所有上级目录
                            Directory.CreateDirectory(directoryPath);
                            Print($"Created directory: {directoryPath}");
                        }

                    }
                //  Directory.CreateDirectory(logdir);
                // 将JSON字符串转换为List<Dictionary<string, object>>


                string id = SortedList1["id"].ToString();
                //  SetField938(iotTable, key, SortedList1);
                SortedList obj =  FindOne(dir, id);
                if (obj != null)
                {
                    CopySortedList(SortedList1, obj);
                    wriToDbf(obj, dbfile);
                }
                else
                {
                    wriToDbf(SortedList1, dbfile);
                }


            }
            catch (Exception e)
            {

                PrintExcept(__METHOD__, e);
            }



            PrintRet(__METHOD__, 0);
        }
        public static int Del(string dir, string id)
        {
            string filePath = dir + "/id_" + id + ".json";
            if (IsExistFil(filePath))
            {
                File.Delete(filePath);
                return 1;
            }
            return 0;
        }
        public static List<SortedList> rnd4jsonFl(string dbf)
        {
            if (dbf.EndsWith(".db"))
                return [];

            if (!dbf.EndsWith(".json"))
            {
                string ext = ".json";
                dbf = dbf + ext;
            }

            SortedList prm = new SortedList();

            //   prm.Add("partns", ($"{mrchtDir}\\{partns}"));


            prm.Add("dbf", ($"{dbf}"));
            return read2list(dbf);
        }

        public static SortedList FindOne(string dir, string id)
        {
            string filePath = dir + "/id_" + id + ".json";
            string txt =  File.ReadAllText(filePath);
            SortedList obj = null;
            if (txt.Trim().Length > 0)
                obj = JsonConvert.DeserializeObject<SortedList>(txt);
            return obj;
        }


    }
}
