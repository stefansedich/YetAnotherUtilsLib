using System;

namespace YetAnotherUtilsLib.Core.Common
{
    public static class Extensions
    {
        public static TResult NullSafe<T, TResult>(this T obj, Func<T, TResult> func)
        {
            try
            {
                return func(obj);
            } 
            catch(NullReferenceException)
            {
                return default(TResult);
            }
        }
    }
}