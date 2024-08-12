global using static prjx.libx.aop;
using prjx.libx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    internal class aop
    {
        //public static async Task<object> callxAsync2024(Delegate callback, params object[] args)
        //{

        //    var __METHOD__ = callback.Method.Name;
        //    print_call_FunArgs(__METHOD__, dbgCls.func_get_args(args));
        //    object o = null;
        //    //      try
        //    // Get the MethodInfo of the delegate
        //    MethodInfo methodInfo = callback.Method;

        //    try
        //    {
        //        // Check if the method is asynchronous (returns a Task or Task<T>)
        //        if (typeof(Task).IsAssignableFrom(methodInfo.ReturnType))
        //        {
        //            // Invoke the delegate and get the Task
        //            var task = (Task)methodInfo.Invoke(callback.Target, args);
        //            await task.ConfigureAwait(false);

        //            // If the task has a result (i.e., it's a Task<T>), get the result
        //            if (methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        //            {
        //                var resultProperty = methodInfo.ReturnType.GetProperty("Result");
        //                o = resultProperty.GetValue(task);
        //            }
        //        }
        //        else
        //        {
        //            // Invoke the delegate synchronously
        //            o = methodInfo.Invoke(callback.Target, args);
        //        }

        //    }
        //    catch (jmp2endEx e1)
        //    {
        //        throw e1;
        //    }
        //    catch (Exception ex)
        //    {
        //        print_catchEx(__METHOD__, ex);
        //        SortedList dbgobj = new SortedList();
        //        dbgobj.Add("mtth", __METHOD__ + "(((" + json_encode_noFmt(func_get_args(args)) + ")))");
        //        logErr2024(ex, __METHOD__, "errdir", dbgobj);
        //    }
        //    //    call
        //    if (o != null)
        //        print_ret(__METHOD__, o);
        //    else
        //        print_ret(__METHOD__, 0);
        //    return o;

        //}


    }

}
