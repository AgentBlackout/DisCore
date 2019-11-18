using System;

namespace DisCore.Runner.Helpers
{
    public static class MessageHelper
    {

        public static string StipMentions(String message) =>
            message.Replace(@"<", @"\<").Replace(@">", @"\>");
    }
}
