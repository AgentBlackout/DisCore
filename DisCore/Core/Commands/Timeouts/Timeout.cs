using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DisCore.Core.Commands.Timeouts
{
    public static class Timeout
    {
        private static readonly Dictionary<string, TimeSpan> Timeouts = new Dictionary<string, TimeSpan>();

        public static void SetTimeout(TimeSpan timeSpan, [CallerMemberName] string memberName = "") =>Timeouts.Add(memberName, timeSpan);

        public static IEnumerable<string> GetTimeouts() =>Timeouts.Keys;

        public static TimeSpan? GetTimeout(string key) => Timeouts.TryGetValue(key, out TimeSpan timespan) ? timespan : (TimeSpan?)null;
        public static void RemoveTimeout(string key) => Timeouts.Remove(key);
    }
}
