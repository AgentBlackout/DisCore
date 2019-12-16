using System;

namespace DisCore.Shared.Helpers
{
    public static class MessageHelper
    {

        public static string StripMentions(String message) =>
            message.Replace(@"<", @"\<").Replace(@">", @"\>");
    }
}
