global using static prjx.libx.cast;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace prjx.libx
{
    internal class cast
    {
        public static string cast_toString(object type)
        {
            if (type == null)
                return "";
            return type.ToString();
        }
        public static SortedList castUrlQueryString2hashtable(string queryString)
        {
            SortedList list = new SortedList();

            // Extract query string from URL
            //int queryStartIndex = url.IndexOf('?');
            //if (queryStartIndex >= 0)
            //{
            //    string queryString = url.Substring(queryStartIndex + 1);

            // Split query string into key-value pairs
            string[] pairs = queryString.Split('&');

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split('=');

                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    string value = keyValue[1];

                    // Add key-value pair to SortedList
                    if (!list.ContainsKey(key))
                    {
                        list.Add(key, value.Trim());
                    }
                }
                //  }
            }

            return list;
        }
        public static double ToNumber(object str)
        {
            return toNumber(str.ToString());
        }
            public static double toNumber(string str)
        {

            if (string.IsNullOrWhiteSpace(str))
            {
               Print("Input string cannot be null or whitespace.");
                return 0;
                //    throw new ArgumentNullException(nameof(str), "Input string cannot be null or whitespace.");
            }

            if (double.TryParse(str, out double result))
            {
                return result;
            }
            else
            {
               Print("Input string is not in the correct format for a double.");
                //  throw new FormatException("Input string is not in the correct format for a double.");
                return 0;
            }

        }


        public static SortedList ConvertToSortedList(object obj)
        {
            if (obj == null)
                return new SortedList();

            SortedList sortedList = new SortedList();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead)
                {
                    object value = property.GetValue(obj);
                    sortedList.Add(property.Name, value);
                }
            }

            return sortedList;
        }

        public static HashSet<string> ConvertToUpperCase(HashSet<string> originalSet)
        {
            return new HashSet<string>(originalSet.Select(s => s.ToUpper()));
        }
        public static HashSet<string> castArr2set(string[] stringArray)
        {
            HashSet<string> hashSet = new HashSet<string>(
                     stringArray.Where(s => !string.IsNullOrWhiteSpace(s))
                 );
            return hashSet;
        }
    }
}
