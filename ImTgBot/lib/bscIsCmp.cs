global using static prjx.libx.bscIsCmp;
using libx;
using Microsoft.Extensions.Primitives;
using prjx.libx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace prjx.libx
{
    internal class bscIsCmp
    {
        public static bool IsListSorted(object obj)
        {
            return obj is List<SortedList> sortedList;
        }

        public static bool IsListHashtb(object obj)
        {
            return obj is List<Hashtable> sortedList;
        }
        public static bool IsExistFilNameStartWz(string todaycode, string dbfld)
        {
            if (!Directory.Exists(dbfld))
                return false;
            var files = Directory.GetFiles(dbfld);
            foreach (string f in files)
            {
                string basename = Path.GetFileName(f);
                if (basename.StartsWith(todaycode))
                    return true;
            }
            return false;
        }

        public static bool IsString(object input)
        {
            return input is string;
        }

        /// <summary>
        /// token的设计  uname exprt...  MRG机器读取区域
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsValidToken(string token)
        {
            string[] tka = token.Split("_");
            string uid = GetElmt(tka, 0);
            string exprt = GetElmt(tka, 1);
            string mrgDecd = DecryptAes(exprt);
            string[] a = mrgDecd.Split(".");
            var uidInMrg = a[0];
            var exprtTime = a[1];
            var isstime = a[2];
            if (uid != uidInMrg)
                return false;
            if (IsExprt(exprtTime))
            {
                return false;
            }
            return true;
        }



        public static bool IsPark(string areaname)
        {
            if (string.IsNullOrEmpty(areaname))
                return false;
            HashSet<string> hs = GetHashsetFromFilTxt($"{prjdir}/cfg_cmd/园区列表.txt");
            return (hs.Contains(areaname));
        }

        public static bool ISCity(string areaname)
        {
            if (string.IsNullOrEmpty(areaname))
                return false;
            HashSet<string> hs = GetHashsetFromFilTxt($"{prjdir}/cfg_cmd/citys.txt");
            return (hs.Contains(areaname));
        }

        public static void weekendChk4lx()
        {
            var datecode = GetTodayCodeMnsHrs(6);
            if (IsSunday("2024" + datecode))
            {
                throw new Exception("sunday ex...");
            }
            if(IsBeforeMonday7AM())
                throw new Exception("sunday ex...");
            // if(ismonday)
            ///throw
        }

        static bool IsBeforeMonday7AM()
        {
            DateTime now = DateTime.Now;

            // 判断是否是星期一
            if (now.DayOfWeek == DayOfWeek.Monday)
            {
                // 判断时间是否在凌晨7点之前
                if (now.TimeOfDay < new TimeSpan(7, 0, 0))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool isMmsgHasMatchPostWd(HashSet<string> postnKywd位置词set, string[] kwds)
        {
            //if (text == null)
            //    return null;
            string[] spltWds = kwds;
            foreach (string wd in spltWds)
            {
                if (postnKywd位置词set.Contains(wd))
                {
                    Print("msgHasMatchPostWd():: postnKywd位置词set.Contains wd=>" + wd);
                    return true;
                }

            }
            return false;
        }

        public static bool isCcontainKwds42(HashSet<string> curRowKywdSset, string[] kwds)
        {
            kwds = Array.ConvertAll(kwds, s => s.ToUpper());
            curRowKywdSset = ConvertToUpperCase(curRowKywdSset);

            return isMmsgHasMatchPostWd(curRowKywdSset, kwds);
        }
        public static bool ISEndsWith(string ext, string extss)
        {
            string[] a = extss.Split(" ");
            foreach (string ex in a)
            {
                if (ext.EndsWith(ex))
                    return true;
            }
            return false;
        }


        public static bool FileHasExtname(string 路径)
        {
            string 文件扩展名 = Path.GetExtension(路径);
            //  string 文件路径 = $"{web根目录}{路径}";
            //   文件路径 = 格式化路径(文件路径);
            if (文件扩展名 == "")
                return false;
            else
                return true;
        }
        public static bool ExistFil(string path1)
        {
            return File.Exists(path1);
        }

        public static bool IsArray(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Type type = obj.GetType();

            // 检查类型是否为数组
            return type.IsArray;
        }
        public static bool IsWord(string input)
        {
            // 使用 Char 类的方法判断是否是字母或数字
            return input.All(c => char.IsLetter(c));
        }
        public static bool IsStr(object obj1)
        {

            return obj1 is string;
        }
        public static bool IsInt(string str)
        {
            return int.TryParse(str, out _);
        }
        public static bool IsNumeric(object str)
        {
            var s = ToStr(str);
            // 匹配整数或带小数点的数字
            return Regex.IsMatch(s, @"^[0-9]+(\.[0-9]+)?$");
        }

        //private static string ToString(object str)
        //{
        //    throw new NotImplementedException();
        //}

        public static bool IsNumeric(string str)
        {
            // 匹配整数或带小数点的数字
            return Regex.IsMatch(str, @"^[0-9]+(\.[0-9]+)?$");
        }
        public static bool IsCollection(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Type type = obj.GetType();

            // 检查类型是否实现了 IEnumerable 接口
            return typeof(IEnumerable).IsAssignableFrom(type);
        }


        public static bool IsStartsWithUppercase(string input)
        {
            // 判断字符串是否为空或为null
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // 检查第一个字符是否为大写字母
            return char.IsUpper(input[0]);
        }
        public static bool IsIn(object hour, string[]? times)
        {
            foreach (string o in times)
            {
                if (hour.ToString().ToUpper().Equals(o.ToUpper()))
                    return true;
            }
            return false;
        }

        public static bool IsEmpty(string v)
        {
            return string.IsNullOrEmpty(v);
        }
        public static bool IsStartsWithArrcomma(string? text, string arrcomma)
        {
            string[] a = arrcomma.Split(",");
            foreach (string msg in a)
            {
                string msg1 = msg.Trim();
                if (IsStartsWith(text, msg1))
                    return true;
            }
            return false;

        }
        public static bool IsStartsWith(string? text, string v)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            return text.Trim().StartsWith(v);
        }
        public static bool IsIn4qrycdt(object rowVal, object cdtVals)
        {
            if (cdtVals == null || cdtVals.ToString() == "")
                return true;
            string[] a = cdtVals.ToString().Split(",");

            foreach (string cdt in a)
            {
                if (rowVal.ToString().ToUpper().Equals(cdt.ToUpper()))
                    return true;
            }
            return false;
        }
        public static bool IsContains(HashSet<string> curRowKywdSset, string keywords)
        {
            string[] a = keywords.Split(",");
            foreach (string rowVal in curRowKywdSset)
            {
                foreach (string kwd in a)
                {
                    if (isEq(rowVal, kwd))
                        return true;
                }
            }
            return false;

        }

        public static bool isEq(string rowVal, string kwd)
        {
            return rowVal.Equals(kwd);
        }
        public static bool isEq4qrycdt(object rowVal, object cdtVal)
        {
            if (cdtVal == null || cdtVal.ToString() == "")
                return true;
            return rowVal.ToString().ToUpper().Equals(cdtVal.ToString().ToUpper());
        }
        public static bool IsChkOK(List<Filtr> li)
        {
            if (!ChkAllFltrTrueDep(li))
                return false;
            return true;
        }
        public static bool isFldValEq111(SortedList row, string Fld, Dictionary<string, StringValues> whereExprsObj)
        {
            //  string Fld = "城市";
            if (hasCondt(whereExprsObj, Fld))
                if (!StrEq(row[Fld], LoadFieldTryGetValue(whereExprsObj, Fld)))   //  cityname not in (citysss) 
                    return false;

            return true;
        }
        public static bool IsExistFil(string arg)
        {
            return System.IO.File.Exists(arg);
        }

        public static bool ISCtry(string areaname)
        {
            if (string.IsNullOrEmpty(areaname))
                return false;
            HashSet<string> hs = GetHashsetFromFilTxt($"{prjdir}/cfg_cmd/ctrys.txt");
            return (hs.Contains(areaname));
        }
        public static bool IsNotExistFil(string v)
        {
            return !System.IO.File.Exists(v);
        }

        public static bool isFldValContain(SortedList row, string Fld, Dictionary<string, string> whereExprsObj)
        {
            //  string Fld = "城市";
            if (hasCondt(whereExprsObj, Fld))
            {
                string valueInQrystr = LoadFieldAsStr(whereExprsObj, Fld);
                if (GetFieldAsStrDep(row, Fld).Contains(valueInQrystr))   //  cityname not in (citysss) 
                    return true;
                else
                    return false;
            }

            return true;
        }

        //public static bool isIn4Qry(SortedList row, string Fld, Dictionary<string, string> whereExprsObj)
        //{
        //    //  string Fld = "城市";
        //    if (hasCondt(whereExprsObj, Fld))
        //    {
        //        string commaStr = LoadField232(whereExprsObj, Fld);
        //     //   IsIn4qrycdt
        //        if (IsIn(row[Fld], commaStr))   //  cityname not in (citysss) 
        //            return false;
        //    }else            

        //    return true;
        //}
        public static bool isFldValEq111(SortedList row, string Fld, Dictionary<string, string> whereExprsObj)
        {
            //  string Fld = "城市";
            if (hasCondt(whereExprsObj, Fld))
                if (!strCls.StrEq(row[Fld], LoadField232(whereExprsObj, Fld)))   //  cityname not in (citysss) 
                    return false;

            return true;
        }
        public static bool IsFileExist(string 文件路径)
        {
            return ExistFil(文件路径);
        }
        public static bool IsFileNotExist(string 文件路径)
        {
            return !ExistFil(文件路径);
        }
        public static bool IsLetter(char character)
        {
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }
        public static bool IsNotExprt(string timeString)
        {
            return !IsExprt(timeString);
        }
        public static bool IsExprt(string timeString)
        {
            // 定义时间格式
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime parsedTime;

            // 尝试解析时间字符串
            bool isValid = DateTime.TryParseExact(timeString, format,
                              System.Globalization.CultureInfo.InvariantCulture,
                              System.Globalization.DateTimeStyles.None, out parsedTime);

            if (!isValid)
            {
                throw new ArgumentException("Invalid time format.");
            }

            // 获取当前时间
            DateTime currentTime = DateTime.Now;

            // 比较时间
            return parsedTime < currentTime;
        }


        public static bool IsTimeExprt(string timeString)
        {
            // 定义时间格式
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime parsedTime;

            // 尝试解析时间字符串
            bool isValid = DateTime.TryParseExact(timeString, format,
                              System.Globalization.CultureInfo.InvariantCulture,
                              System.Globalization.DateTimeStyles.None, out parsedTime);

            if (!isValid)
            {
                throw new ArgumentException("Invalid time format.");
            }

            // 获取当前时间
            DateTime currentTime = DateTime.Now;

            // 比较时间
            return parsedTime < currentTime;
        }


        public static bool IsEnglishLetter(char character)
        {
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }
        public static bool IsStartsWith(string msg2056, HashSet<string> hs11)
        {
            msg2056 = ToStr(msg2056);
            foreach (string ch in hs11)
            {
                if (msg2056.StartsWith(ch))
                    return true;
            }
            return false;
        }
        public static bool IsChkfltrOk(List<bool> li)
        {
            if (!ChkAllFltrTrue(li))
                return false;
            return true;
        }
        public static bool IsArrOrColl(object inputArray)
        {
            if (IsSortedList(inputArray))
            {
                return false;
            }
            if (IsHashtable(inputArray))
            {
                return false;
            }

            if (IsSortedListOfStringObject(inputArray))
            {
                return false;
            }
            if (IsArray(inputArray))
                return true;
            if (IsCollection(inputArray))
                return true;

            return false;
        }

        static bool IsSortedListOfStringObject(object obj)
        {
            return obj is SortedList<string, object>;
        }
        public static bool IsHashtable(object sortedList)
        {
            // 使用 is 关键字检查对象类型
            if (sortedList is Hashtable)
            {
                return true;
            }
            return false;
        }

        public static bool IsContainsKey(object obj1, string key)
        {
            if (obj1 == null || string.IsNullOrEmpty(key))
            {
                return false;
            }

            if (IsSortedList(obj1))
                return ((SortedList)obj1).ContainsKey(key);

            // 检查是否为字典类型
            if (obj1 is IDictionary<string, object> dictionary)
            {
                return dictionary.ContainsKey(key);
            }

            // 检查是否为动态对象
            if (obj1 is IDictionary<string, object> dynamicObject)
            {
                return dynamicObject.ContainsKey(key);
            }

            // 检查对象是否包含指定属性
            Type type = obj1.GetType();
            PropertyInfo property = type.GetProperty(key);
            return property != null;
        }

        public static bool IsSortedList(object sortedList)
        {
            // 使用 is 关键字检查对象类型
            if (sortedList is SortedList)
            {
                return true;
            }
            return false;
        }

        public static bool IsStrEndWz(string 路径, string 扩展名)
        {
            return 路径.ToUpper().Trim().EndsWith("." + 扩展名.Trim().ToUpper());
        }
        public static bool IsPathEndwithExt(string 路径, string 扩展名)
        {
            return 路径.ToUpper().Trim().EndsWith("." + 扩展名.Trim().ToUpper());
        }
    }
}
