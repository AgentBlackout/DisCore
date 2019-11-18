using System;

namespace DisCore.Shared.Helpers
{
    public static class MessageHelper
    {

        public static string StipMentions(String message) =>
            message.Replace(@"<", @"\<").Replace(@">", @"\>");
    }
}
