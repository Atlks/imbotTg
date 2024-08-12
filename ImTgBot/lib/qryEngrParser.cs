global using static libx.qryEngrParser;
global using static libx.Filtr;
using prjx.libx;
using System;
//using System.Runtime.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
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
using static prjx.libx.corex;

//using static libx.storeEngr4Nodesqlt;
using Microsoft.Extensions.Primitives;
using prjx;
 
using prjx;
using Microsoft.Extensions.Caching.Memory;
namespace libx
{
    internal class qryEngrParser
    {
        public static bool hasCondt(Dictionary<string, string> whereExprsObj, string fld)
        {
            return whereExprsObj.ContainsKey(fld);

            //if (park4srch == null)
            //{
            //    return false;
            //}
            //return true;
        }
        public static bool hasCondt(Dictionary<string, StringValues> whereExprsObj, string fld)
        {
            return whereExprsObj.ContainsKey(fld);

            //if (park4srch == null)
            //{
            //    return false;
            //}
            //return true;
        }
        public static int Qe_del(string id, string fromDdataDir, Func<string, List<SortedList>> rndFun, Func<(SortedList, string), int> del_row_Fun)
        {

            var patns_dbfs = _calcPatnsV3(fromDdataDir, "");
            string[] arr = patns_dbfs.Split(',');

            int n = 0;
            foreach (string dbf in arr)
            {
                Func<SortedList, bool> whereFun = (SortedList row) =>
                {
                    if (string.IsNullOrEmpty(id))
                        return false;
                    if (row["id"].ToString().ToLower() == (id.ToLower()))
                        return true;
                    return false;
                };

                //= _qryBySnglePart(dbf, whereFun, cfgStrEngr);
                //rztLi = arrCls.array_merge(rztLi, li);
                List<SortedList> delneedRowSX = _qryBySnglePart(dbf, whereFun, rndFun);
                //   var tuple = (id: id, dbf: dbf);
                //   var tuple = (id: id, dbf: dbf);
                foreach (SortedList delneedRow in delneedRowSX)
                {
                    n = n + del_row_Fun((rw: delneedRow, dbf: dbf));
                }

            }

            return n;
        }
        public static List<bool> getLstFltrsFrmQrystr(string qrystr, SortedList row)
        {
            List<bool> li = new List<bool>();


            Dictionary<string, string> qrystrDic = LoadDic4qryCdtn(qrystr);
            foreach_DictionaryKeys(qrystrDic, (string key) =>
            {
                string v = GetFieldAsStr(qrystrDic, key);
                if (v == "")
                    return;   //

                if (v.StartsWith("%") && v.EndsWith("%"))
                {
                    li.Add((isFldValContain(row, key, qrystrDic)));
                }
                else if (v.Contains(","))
                {
                    object rowVal = GetField(row, key);
                    string cdtVals = GetField(qrystrDic, key);
                    bool bool1155 = IsIn4qrycdt(rowVal, cdtVals);
                    li.Add(bool1155);
                }
                else
                    li.Add((isFldValEq111(row, key, qrystrDic)));



            });
            return li;
        }
        public static List<Filtr> getLstFltrCdtnsFrmQrystr(string qrystr, SortedList row)
        {
            List<Filtr> li = new List<Filtr>();


            Dictionary<string, string> filters = LoadDic4qryCdtn(qrystr);
            foreach_DictionaryKeys(filters, (string key) =>
            {
                li.Add(new Filtr(isFldValEq111(row, key, filters)));
            });
            return li;
        }

        public static List<SortedList> GetListFltrV2(
            string fromDdataDir, string shanrES,
            Func<SortedList, bool> whereFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES, "whereFun()"));
            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。

            string[] shareArr = shanrES.Split(',');
            foreach (var shar in shareArr)
            {

                string rndFun;
                SortedList curShareCfg = ShareDetail(fromDdataDir, shar);
                rndFun = (string)curShareCfg["rndFun"];
                var CurSharFullpath = fromDdataDir + "/" + shar
                    + "." + GetFieldAsStrDep(curShareCfg, "ext");

                //    Func<string, List<SortedList>> rndEng_Fun = (Func<string, List<SortedList>>)GetFunc(); ;
                var dbg = (shar: shar, storeEngr: rndFun, CurSharFullpath: CurSharFullpath);
                List<SortedList> li = arr_fltr4ReadShare(CurSharFullpath, whereFun, rndFun.ToString(), dbg);
                //append to rztLi
                Append(li, rztLi);
            }
            PrintRet(__METHOD__, "rztLi.size:" + rztLi.Count);
            return rztLi;
        }

        public static List<SortedList> GetListFltr(string fromDdataDir, string shanrES,
      string qrystr
    )
        {
            PrintTimestamp(" start fun GetListFltr");
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES, "whereFun()"));

 
            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。

            // string[] shareArr = shanrES.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            string shareStr = _calcPatnsV4(fromDdataDir, shanrES);
            string[] shareArr = shareStr.Split(',');
            foreach (var shar in shareArr)
            {
                var CurSharFullpath = fromDdataDir + "/" + shar;
                string rndFun;
                SortedList curShareCfg = ShareDetail(fromDdataDir, shar);
                if (curShareCfg == null)
                {
                    Print("!!!⚠️⚠️wanging... cangt find shareCfg:" + shar);
                    continue;
                }
                rndFun = (string)curShareCfg["rndFun"];

                //    Func<string, List<SortedList>> rndEng_Fun = (Func<string, List<SortedList>>)GetFunc(); ;
                var dbg = (shar: shar, storeEngr: rndFun, CurSharFullpath: CurSharFullpath);
                List<SortedList> li = GetListFltr4ReadShare(CurSharFullpath, qrystr, rndFun.ToString(), dbg);
                rztLi = array_merge(rztLi, li);
            }
            PrintRet(__METHOD__, "rztLi.size:" + rztLi.Count);
            PrintTimestamp(" start fun " + __METHOD__);
            return rztLi;
        }

        public static List<SortedList> GetListFltr4ReadShare(string curSharFullpath, string qrystr, string rndFun, (string shar, string? storeEngr, string CurSharFullpath) dbg)
        {
            Func<SortedList, bool> whereFun = CastQrystr2FltrCdtFun(qrystr);
            List<SortedList> list = arr_fltr4ReadShare(curSharFullpath,  whereFun, rndFun,dbg);
            return list;
        }


        /// <summary>
        /// pfrm prblm  
        /// </summary>
        /// <param name="fromDdataDir"></param>
        /// <param name="shanrES"></param>
        /// <param name="whereFun"></param>
        /// <returns></returns>
        public static List<SortedList> GetListFltr(string fromDdataDir, string shanrES,
         Func<SortedList, bool> whereFun
       )
        {
            PrintTimestamp(" start fun GetListFltr");
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES, "whereFun()"));


            //   SortedList shareCfgList = getShareCfgLst(fromDdataDir);

            //  List<t> rsRztInlnKbdBtn = new List<t>();

            //if (shareCfgList is null)
            //{
            //    throw new ArgumentNullException(nameof(shareCfgList));
            //}

            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。

            // string[] shareArr = shanrES.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            string shareStr = _calcPatnsV4(fromDdataDir, shanrES);
            string[] shareArr = shareStr.Split(',');
            //todo here can chg to corrent binxin prcs
            foreach (var shar in shareArr)
            {
                var CurSharFullpath = fromDdataDir + "/" + shar;
                string rndFun;
                SortedList curShareCfg = ShareDetail(fromDdataDir, shar);
                if (curShareCfg == null)
                {
                    Print("!!!⚠️⚠️wanging... cangt find shareCfg:" + shar);
                    continue;
                }
                rndFun = (string)curShareCfg["rndFun"];

                //    Func<string, List<SortedList>> rndEng_Fun = (Func<string, List<SortedList>>)GetFunc(); ;
                var dbg = (shar: shar, storeEngr: rndFun, CurSharFullpath: CurSharFullpath);
                List<SortedList> li = arr_fltr4ReadShare(CurSharFullpath, whereFun, rndFun.ToString(), dbg);
                rztLi = array_merge(rztLi, li);
            }
            PrintRet(__METHOD__, "rztLi.size:" + rztLi.Count);
            PrintTimestamp(" start fun " + __METHOD__);
            return rztLi;
        }

        //public static List<t> xxxx<t>()
        //{
        //    SortedList row = new SortedList();
        //    List<t> rsRztInlnKbdBtn = new List<t>();
        //    rsRztInlnKbdBtn.Add( row);
        //    return rsRztInlnKbdBtn;
        //}
        public static List<SortedList> arr_fltr4readDir(string fromDdataDir, string shanrES,
            Func<SortedList, bool> whereFun
          )
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES, "whereFun()"));


            //   SortedList shareCfgList = getShareCfgLst(fromDdataDir);

            //  List<t> rsRztInlnKbdBtn = new List<t>();

            //if (shareCfgList is null)
            //{
            //    throw new ArgumentNullException(nameof(shareCfgList));
            //}

            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。

            // string[] shareArr = shanrES.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            string shareStr = _calcPatnsV4(fromDdataDir, shanrES);
            string[] shareArr = shareStr.Split(',');
            foreach (var shar in shareArr)
            {
                var CurSharFullpath = fromDdataDir + "/" + shar;
                string rndFun;
                SortedList curShareCfg = ShareDetail(fromDdataDir, shar);
                rndFun = (string)curShareCfg["rndFun"];

                //    Func<string, List<SortedList>> rndEng_Fun = (Func<string, List<SortedList>>)GetFunc(); ;
                var dbg = (shar: shar, storeEngr: rndFun, CurSharFullpath: CurSharFullpath);
                List<SortedList> li = arr_fltr4ReadShare(CurSharFullpath, whereFun, rndFun.ToString(), dbg);
                rztLi = array_merge(rztLi, li);
            }
            PrintRet(__METHOD__, "rztLi.size:" + rztLi.Count);
            return rztLi;
        }
        public static Func<SortedList, bool> CastQrystr2FltrCdtFun(string qrystr)
        {
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                //if (row["园区"].ToString().Contains("东风"))
                //    Print("dbg243");

                List<bool> li = getLstFltrsFrmQrystr(qrystr, row);

                //    bool rzt=isxxx（）&isXXX();

                if (!ChkAllFltrTrue(li))
                    return false;
                return true;
            };
            return whereFun;
        }

        private static string getRndFun(string fromDdataDir, string shanrES, string shar)
        {
            string rndFun;
            if (string.IsNullOrEmpty(shanrES))
            {
                if (shar.EndsWith(".db"))
                    return "rnd_next4Sqlt";
                if (shar.EndsWith(".json"))
                    return nameof(rnd4jsonFl);
                if (shar.EndsWith(".ini"))
                    return nameof(rnd4jsonFl);
                if (shar.EndsWith(".xlsx"))
                    return nameof(rnd4jsonFl);
                return nameof(rnd4jsonFl);
            }
            else
            {
                SortedList curShareCfg = ShareDetail(fromDdataDir, shar);
                rndFun = (string)curShareCfg["rndFun"];
            }

            return rndFun;
        }

        //arr_fltr4readDir
        public static List<SortedList> arr_fltrV2(string fromDdataDir, string shanrES,
           Func<SortedList, bool> whereFun,
           Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES));


            //  List<t> rsRztInlnKbdBtn = new List<t>();

            if (rndFun is null)
            {
                throw new ArgumentNullException(nameof(rndFun));
            }

            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            var share_dbfs = _calcPatnsV3(fromDdataDir, shanrES);
            string[] arr = share_dbfs.Split(',');
            foreach (string dbfCurShar in arr)
            {
                List<SortedList> li = _qryBySnglePart(dbfCurShar, whereFun, rndFun);
                rztLi = array_merge(rztLi, li);
            }

            return rztLi;
        }

        public static List<SortedList> arr_fltrDep2024(string fromDdataDir, string shanrES,
                 Func<SortedList, bool> whereFun,
                 Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES));


            //  List<t> rsRztInlnKbdBtn = new List<t>();

            if (rndFun is null)
            {
                throw new ArgumentNullException(nameof(rndFun));
            }

            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            var share_dbfs = _calcPatnsV3(fromDdataDir, shanrES);
            string[] arr = share_dbfs.Split(',');
            foreach (string dbfCurShar in arr)
            {
                List<SortedList> li = _qryBySnglePart(dbfCurShar, whereFun, rndFun);
                rztLi = array_merge(rztLi, li);
            }

            return rztLi;
        }

        public static List<t> Qe_qryV3<t>(string fromDdataDir, string shanrES,
          Func<SortedList, bool> whereFun,
          Func<SortedList, int> ordFun,
          Func<SortedList, t> selktFun,
          Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES));
            List<t> listFnl = new List<t>();
            try
            {


                if (rndFun is null)
                {
                    throw new ArgumentNullException(nameof(rndFun));
                }

                List<SortedList> rztLi = arr_fltrDep2024(fromDdataDir, shanrES, whereFun, rndFun);



                if (ordFun != null)
                {
                    rztLi = rztLi.Cast<SortedList>()
                                   .OrderBy(ordFun)
                                   .ToList();
                }


                if (selktFun == null)
                {
                    throw new ArgumentNullException("  need slktfun ,u can use def slktfun");
                    //   dbgCls.setDbgValRtval(__METHOD__, 0);
                    //   return rztLi;

                }

                listFnl = arr_transfm_map(rztLi, selktFun);
            }

            catch (Exception e)
            {
                Print($"--ex catch---- mtth:{__METHOD__}((( {json_encode_noFmt(func_get_args(fromDdataDir, shanrES))}");
                Print(e);
                logErr2025(e, __METHOD__, "errdir");
                //  return rsRztInlnKbdBtn;
            }

            PrintRet(__METHOD__, 0);
            return listFnl;
        }

        private static List<t> arr_transfm_map<t>(List<SortedList> rztLi, Func<SortedList, t> selktFun)
        {
            List<t> listFnl = new List<t>();
            for (int i = 0; i < rztLi.Count; i++)
            {
                SortedList row = rztLi[i];

                listFnl.Add(selktFun(row));


            }
            return listFnl;
        }

        public static List<t> Qe_qryV2<t>(string fromDdataDir, string shanrES,
            Func<SortedList, bool> whereFun,
            Func<SortedList, int> ordFun,
            Func<SortedList, t> selktFun,
            Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES));


            List<t> rsRztInlnKbdBtn = new List<t>();
            try
            {
                if (rndFun is null)
                {
                    throw new ArgumentNullException(nameof(rndFun));
                }

                List<SortedList> rztLi = new List<SortedList>();
                //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
                var share_dbfs = _calcPatnsV3(fromDdataDir, shanrES);
                string[] arr = share_dbfs.Split(',');
                foreach (string dbfCurShar in arr)
                {
                    List<SortedList> li = _qryBySnglePart(dbfCurShar, whereFun, rndFun);
                    rztLi = array_merge(rztLi, li);
                }

                if (ordFun != null)
                {
                    rztLi = rztLi.Cast<SortedList>()
                                   .OrderBy(ordFun)
                                   .ToList();
                }


                if (selktFun == null)
                {
                    throw new ArgumentNullException("  need slktfun ,u can use def slktfun");
                    //   dbgCls.setDbgValRtval(__METHOD__, 0);
                    //   return rztLi;

                }


                for (int i = 0; i < rztLi.Count; i++)
                {
                    SortedList row = rztLi[i];

                    rsRztInlnKbdBtn.Add(selktFun(row));


                }


            }
            catch (Exception e)
            {
                Print($"--ex catch---- mtth:{__METHOD__}((( {json_encode_noFmt(func_get_args(fromDdataDir, shanrES))}");
                Print(e);
                logErr2025(e, __METHOD__, "errdir");
                //  return rsRztInlnKbdBtn;
            }
            dbgCls.PrintRet(__METHOD__, 0);
            return rsRztInlnKbdBtn;
        }

        public static List<SortedList> Qe_qry(string fromDdataDir, string partnsExprs, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> rndFun)
        {
            if (rndFun is null)
            {
                throw new ArgumentNullException(nameof(rndFun));
            }

            List<SortedList> rztLi = new List<SortedList>();
            var patns_dbfs = _calcPatnsV3(fromDdataDir, partnsExprs);
            string[] arr = patns_dbfs.Split(',');
            foreach (string dbf in arr)
            {
                try
                {
                    List<SortedList> li = _qryBySnglePart(dbf, whereFun, rndFun);
                    rztLi = arrCls.array_merge(rztLi, li);
                }
                catch (Exception e)
                {
                    PrintExcept("Qe_qry", e);
                }

            }

            return rztLi;
        }

        public static int Qe_save(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> rndFun, Func<SortedList, int> wrt_rowFun, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_merge(sortedListNew, dataDir, rndFun, wrt_rowFun);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, wrt_rowFun);

            }
        }

        public static int Qe_saveOrUpdtMerge(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> rndFun, Func<SortedList, int> wrt_row_fun, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_merge(sortedListNew, dataDir, rndFun, wrt_row_fun);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, wrt_row_fun);

            }
        }

        public static int Qe_saveOrUpdtReplace(SortedList sortedListNew, string dataDir, Func<SortedList, int> wrt_rowFun, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_replace(sortedListNew, dataDir, wrt_rowFun);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, wrt_rowFun);

            }
        }

        private static int qe_replace(SortedList sortedListNew, string dataDir, Func<SortedList, int> wrt_rowFun)
        {

            int str = wrt_rowFun(sortedListNew);
            return str;
        }

        private static int qe_merge(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> rndFun, Func<SortedList, int> wrt_rowFun)
        {

            SortedList old = (Qe_find(sortedListNew["id"].ToString(), dataDir, null, rndFun));
            SortedList mereed = CopyToOldSortedList(sortedListNew, old);
            int str = wrt_rowFun(mereed);
            return str;
        }

        private static int qe_add(SortedList sortedListNew, string dataDir, Func<SortedList, int> wrt_rowFun)
        {
            //  mereed = sortedListNew;
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            sortedListNew["id"] = timestamp;
            int str = wrt_rowFun(sortedListNew);
            return str;
        }

        // str eng is find_current_row
        public static SortedList Qe_find(string id, string dataDir, string partns, Func<string, List<SortedList>> rndFun)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), id, dataDir, partns));

            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(id))
                    return false;
                if (row["id"].ToString().ToLower() == (id.ToLower()))
                    return true;
                return false;
            };

            //from xxx partion(aa,bb) where xxx
            List<SortedList> rztLi = Qe_qry(dataDir, partns, whereFun, rndFun: rndFun);


            SortedList results = rztLi[0];
            dbgCls.PrintRet(__METHOD__, "results.Count=>" + results.Count);
            return results;
        }

        internal static string _calcPatnsV4(string FromdataDir, string shareFiles)
        {
            PrintTimestamp(" start _calcPatnsV4()");
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), FromdataDir, shareFiles));

            string result = shareFiles;

            if (string.IsNullOrEmpty(shareFiles))
            {
                SortedList sharecfg = shareList(FromdataDir);
                result = GetKeysCommaSeparated(sharecfg);
            }



            dbgCls.PrintRet(__METHOD__, result);
            PrintTimestamp(" end _calcPatnsV4()");
            return result;
        }


        internal static string _calcPatnsV3(string dir, string shareFiles)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dir, shareFiles));

            //if (string.IsNullOrEmpty(Extname))
            //    Extname = "txt";
            if (string.IsNullOrEmpty(shareFiles))
            {

                string rzt = GetFilePathsCommaSeparated(dir);
                dbgCls.PrintRet(__METHOD__, rzt);
                return rzt;
            }
            ArrayList arrayList = new ArrayList();
            string[] dbArr = shareFiles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dbf in dbArr)
            {
                string path = dir + "/" + dbf + "";
                //if (!File.Exists(path))
                //{
                //   print("not exist file dbf=>" + path);
                //    continue;
                //}
                arrayList.Add(path);
            }

            // 使用 ArrayList 的 ToArray 方法将其转换为对象数组
            object[] objectArray = arrayList.ToArray();

            // 使用 String.Join 方法将数组转换为逗号分割的字符串
            string result = string.Join(",", objectArray);

            dbgCls.PrintRet(__METHOD__, result);

            return result;
        }

        //public static Func<string, List<SortedList>> rnd_next4SqltRfV2()

        /// <summary>
        /// pfm slow todo 
        /// </summary>
        /// <param name="shareName"></param>
        /// <param name="whereFun"></param>
        /// <param name="rnd"></param>
        /// <param name="dbg"></param>
        /// <returns></returns>
        public static List<SortedList> arr_fltr4ReadShare(string shareName, Func<SortedList, bool> whereFun, string rnd, object dbg)
        {
            PrintTimestamp($" start arr_fltr4ReadShare() {shareName} *whreFun {rnd} ");
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgsFast(__METHOD__, func_get_args(shareName, "*whreFun", rnd, dbg));

            SortedList<string, Func<string, List<SortedList>>> map = new SortedList<string, Func<string, List<SortedList>>>();
         //   map.Add("rnd_next4Sqlt", rnd_next4Sqlt);
            map.Add("rnd4jsonFl", rnd4jsonFl);
            //rnd=rnd_next4Sqlt
            // 将静态方法的引用赋值给 Action 委托
            //  Func<string, List<SortedList>> rnd_next4SqltRef = rnd_next4Sqlt;
            // Func<string, List<SortedList>> fun_rnd1 = rnd_next4Sqlt;
            Func<string, List<SortedList>> fun_rnd = (Func<string, List<SortedList>>)map[rnd];
            //    List<SortedList> li;

            //-----------cache todo time out 

            string key = shareName;
            PrintTimestamp(" bef TryGetValue frm cache");
            // 获取缓存项
            if (cache2024.TryGetValue(key, out List<SortedList> li))
            {
                Console.WriteLine(" get key from cache ok: key=>" + key);
            }
            else
            {
                PrintTimestamp(" bef fun_rnd(shareName)");
                li = fun_rnd(shareName);
                // 设置一个缓存项，10分钟后过期
                //note is add data need remove cache key
                //   cache2024.Set(key, li, TimeSpan.FromMinutes(10));
            }

 
            if (li.Count > 0 && whereFun != null)
                li = ArrFltrV2(li, whereFun);

            PrintRet(__METHOD__, li.Count);
            PrintTimestamp($" endfun arr_fltr4ReadShare()");
            return li;
        }
        public static SortedList cache311 = new SortedList();
        //private static List<SortedList> rnd_next4SqltRfV2(string arg)
        //{
        //    throw new NotImplementedException();
        //}

        public static List<SortedList> _qryByShare(string shareName, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> rndFun, object dbg)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(shareName, dbg));

            List<SortedList> li = rndFun(shareName);

            li = db.arr_fltr330(li, whereFun);

            dbgCls.PrintRet(__METHOD__, li.Count);
            return li;
        }
        //单个分区ony need where ,,,bcs order only need in mergeed...and map_select maybe orderd,and top n ,,then last is need to selectMap op
        public static List<SortedList> _qryBySnglePart(string dbfName, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> rndFun)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(dbfName, "rndFun"));

            List<SortedList> li = rndFun(dbfName);

            li = db.arr_fltr330(li, whereFun);

            dbgCls.PrintRet(__METHOD__, li.Count);
            return li;
        }


        public static bool ChkAllFltrTrue(List<bool> li)
        {
            foreach (bool cdt in li)
            {
                if (cdt == false)
                    return false;
            }
            return true;
        }
        //dep
        public static bool ChkAllFltrTrueDep(List<Filtr> li)
        {
            foreach (Filtr cdt in li)
            {
                if (cdt.left == false)
                    return false;
            }
            return true;
        }
    }


    //dep
    public class Filtr
    {
        public bool left;
        public bool v2;

        public Filtr(bool left)
        {
            this.left = left;
            this.v2 = true;
        }
    }
}