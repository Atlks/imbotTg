using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static SqlParser.Ast.Statement;

namespace prjx.libx
{
    internal class opcode
    {

        //ldfld

        //  load field of an object
        //stfld

        //  store into a field of an object

        public static void stfld(object obj, string fld, object val)
        {

        }

        //	
        //  load an element fo an array

        public static object ldelem_ldElmt(ArrayList array, int index)
        {
            return array[index];
        }
        public static ArrayList newarr()
        {
            return new ArrayList();
        }

        //	
        //  store an element of an array
        public static void stelem_storeElmt2arr(ArrayList array, int index, object value)
        {

        }
    }
}
