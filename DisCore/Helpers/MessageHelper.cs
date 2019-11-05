using System;
using System.Collections.Generic;
using System.Text;

namespace DisCore.Helpers
{
    public static class MessageHelper
    {

        public static string StipMentions(String message) =>
            message.Replace(@"<", @"\<").Replace(@">", @"\>");
    }
}
