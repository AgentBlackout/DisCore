using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DisCore.Runner.Helpers
{
    public static class FuncHelper
    {
        public static T MethodInfoToFunc<T>(MethodInfo info, object instance)
        {
            return (T)(object)Delegate.CreateDelegate(typeof(T), instance, info);
        }
    }
}