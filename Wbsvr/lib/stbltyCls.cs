global using static prjx.libx.stbltyCls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class stbltyCls
    {

        public static void Boot4StbltSetting()
        {
            //------------------  
            CloseConsoleQuickEditMode(false);
            //------------------ 设置全局异常处理
            prjx.libx.exCls.set_error_handler();

            //log out
            //   RunSetRollLogFileV2();

          //  数组 字符串api安全api
        //    7.边界条件数组越界：访问数组或集合时，索引超出范围。
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        public static void CloseConsoleQuickEditMode(bool enable = false)
        {
            const string consoleKeyPath = @"HKEY_CURRENT_USER\Console";
            const string quickEditModeValueName = "QuickEdit";

            try
            {
                // Check if the registry key exists
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(consoleKeyPath, true))
                {
                    if (key != null)
                    {
                        // Set the QuickEdit value
                        key.SetValue(quickEditModeValueName, enable ? 1 : 0, RegistryValueKind.DWord);
                    }
                    else
                    {
                        Console.WriteLine("Registry key not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while modifying the registry: {ex.Message}");
            }
        }

        public static string TryToStr(object val,string def="")
        {

            if (val == null)
                return "";
            if (val is bool boolVal)
            {
                return boolVal ? "TRUE" : "FALSE";
            }
            //  // 对象是 long 类型，转换为字符串
            return val.ToString();
        }
        public static string ToStr1127(object val)
        {

            if (val == null)
                return "";
            if (val is bool boolVal)
            {
                return boolVal ? "TRUE" : "FALSE";
            }
            //  // 对象是 long 类型，转换为字符串
            return val.ToString();
        }
    }
}
