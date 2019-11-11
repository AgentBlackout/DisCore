using System;

namespace DisCore.Helpers
{
    public static class MessageHelper
    {

        public static string StipMentions(String message) =>
            message.Replace(@"<", @"\<").Replace(@">", @"\>");
    }
}
