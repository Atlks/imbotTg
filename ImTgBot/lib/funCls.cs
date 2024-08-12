global using static libx.funCls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
 
using prjx.libx;
using System.Reflection;
namespace libx
{
    internal class funCls
    {

        public bool MethodExists(string typeName, string methodName)
        {
            Type type = Type.GetType(typeName);
            if (type == null)
            {
               Print($"Type '{typeName}' not found.");
                return false;
            }

            MethodInfo methodInfo = type.GetMethod(methodName);
            if (methodInfo == null)
            {
               Print($"Method '{methodName}' not found in type '{typeName}'.");
                return false;
            }

            return true;
        }
    }
}
