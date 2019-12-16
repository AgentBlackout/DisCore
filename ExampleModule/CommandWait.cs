using System;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Helpers;

namespace ExampleModule
{
    [Command("wait")]
    public class CommandWait : ICommand
    {

        public async Task Wait(CommandContext ctx)
        {
            await ctx.Reply("Waiting 30 for next command");
            var message = await ctx.Handlers.EventHandler.GetNextMessageAsync(ctx.Channel, TimeSpan.FromSeconds(30));
            if (message == null)
            {
                await ctx.Channel.SendMessageAsync("Timed out");
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"You said {MessageHelper.StripMentions(message.Content)}");
            }
        }


        public async Task<string> Usage()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Summary()
        {
            throw new NotImplementedException();
        }
    }
}
