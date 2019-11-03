using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DisCore.Core.Commands.Timeouts
{
    public static class Timeout
    {
        private static readonly Dictionary<string, TimeSpan> _timeouts = new Dictionary<string, TimeSpan>();

        public static void SetTimeout(TimeSpan timeSpan, [CallerMemberName] string memberName = "") =>_timeouts.Add(memberName, timeSpan);

        public static IEnumerable<string> GetTimeouts() =>_timeouts.Keys;

        public static TimeSpan? GetTimeout(string key) => _timeouts.TryGetValue(key, out TimeSpan timespan) ? timespan : (TimeSpan?)null;
        public static void RemoveTimeout(string key) => _timeouts.Remove(key);
    }
}
