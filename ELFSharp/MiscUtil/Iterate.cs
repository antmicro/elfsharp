using System;
using System.Collections.Generic;

namespace MiscUtil
{
    public static class Misc {
        public static IEnumerable<T> Iterate<T>(Func<T> function)
        {
            while(true)
            {
                yield return function();
            }
        }
    }
}
