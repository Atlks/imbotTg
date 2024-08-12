global using static prjx.libx.qry_share;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class qry_share
    {
        //share_add   add cfg in cfg file...
        //
        public static SortedList ShareDetail(string FromdataDir, string shareName)
        {
            SortedList cfg4curDatatype = shareList(FromdataDir);
           Print(json_encode(cfg4curDatatype));

          //  SortedList cfg4curDatatype= shareCfgList[]
            SortedList? sortedList = (SortedList)cfg4curDatatype[shareName];
         
            return sortedList;
        }

        public static SortedList shareList(string dataType)
        {
            PrintTimestamp(" start shareList()" + dataType);
            //SortedList shareCfg1 = new SortedList();
            //shareCfg1.Add("name", "缅甸");
            //shareCfg1.Add("fmt", "sqlt");
            //shareCfg1.Add("storeEngr", "rnd_next4SqltRf");
            ////  shareCfg1.Add("storeEngr", rnd_next4SqltRf());
            //SortedList shareCfg2 = new SortedList();
            //shareCfg2.Add("name", "老挝");
            //shareCfg2.Add("fmt", "json");
            //shareCfg2.Add("storeEngr", "rnd4jsonFlRf");
       // public static List<SortedList> rnd_next4Sqlt(string dbf)
            ////     shareCfg2.Add("storeEngr", rnd4jsonFlRf());
            /////-----cfg  mercht商家数据 share
            var mymm4shareCfg = "name=缅甸&fmt=sqlt&rndFun=" + "rnd_next4Sqlt";
            var laos4shareCfg = "name=老挝&fmt=json&rndFun=" + nameof(rnd4jsonFl);
            //   SortedList shareCfgList4dataDir = new SortedList();
            //SortedList valueMM = castUrlQueryString2hashtable(mymm4shareCfg);
            //shareCfgList4dataDir.Add("缅甸", valueMM);
            //shareCfgList4dataDir.Add("老挝", castUrlQueryString2hashtable(laos4shareCfg));
            //SortedList cfgFnal = new SortedList();
            //cfgFnal.Add(dataType, shareCfgList4dataDir);
            //return (SortedList)cfgFnal[dataType];
            SortedList shareCfgList4dataDir = ReadJsonToSortedList($"{prjdir}/cfgShare/{dataType}.json");
            CastVal2hashtable(shareCfgList4dataDir);
            PrintTimestamp(" endfun shareList()" + dataType);
            return shareCfgList4dataDir;


        }
    }
}
