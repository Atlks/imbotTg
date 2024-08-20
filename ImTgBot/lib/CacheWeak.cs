global using static libx.WeakCache;
using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libx
{


    class WeakCache
    {
        public static Dictionary<string, WeakReference<object>> cache =
            new Dictionary<string, WeakReference<object>>();

        //public void Add(string key, Func<object> setKeyV)
        //{
        //    // 添加或更新缓存中的值
        //    cache[key] = new WeakReference<object>(setKeyV());
        //}
        public static void Add(string key, object v)
        {
            // 添加或更新缓存中的值
            cache[key] = new WeakReference<object>(v);
        }

        public static object Get(string key)
        {
            // 尝试从缓存中获取值
            if (cache.TryGetValue(key, out WeakReference<object> weakReference))
            {
                if (weakReference.TryGetTarget(out object target))
                {
                    return target;
                }
                else
                {
                    // 对象已被垃圾回收，从缓存中移除
                    cache.Remove(key);
                }
            }
            // 缓存中没有该键或对象已被回收
            return null;
        }

        public static void Remove(string key)
        {
            // 从缓存中移除键
            cache.Remove(key);
        }

        static void Main122()
        {
           
            // 添加对象到缓存
            WeakCache.Add("item1", () => new object());

            // 尝试获取缓存的对象
            var cachedItem = WeakCache.Get("item1");
            if (cachedItem != null)
            {
                Console.WriteLine("Item1 is still in cache.");
            }
            else
            {
                Console.WriteLine("Item1 has been collected.");
            }

            // 手动触发垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // 再次尝试获取缓存的对象
            cachedItem = WeakCache.Get("item1");
            if (cachedItem != null)
            {
                Console.WriteLine("Item1 is still in cache.");
            }
            else
            {
                Console.WriteLine("Item1 has been collected.");
            }
        }
    }

}
