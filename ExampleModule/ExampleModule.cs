using System;
using System.Threading.Tasks;
using DisCore.Shared.Events;
using DisCore.Shared.Modules;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace ExampleModule
{
    public class ExampleModule : IModule
    {
        public string Name => "ExampleModule";
        public String Version => "0.0.1";
        public String Author => "AgentBlackout";
        public string Summary => "Brief description about the module? I suppose it's a test module.";

        public async Task OnLoad()
        {
            await Task.Delay(200);
        }

        public async Task OnUnload()
        {
            await Task.Delay(200);
        }

        public async Task OnLoadGuild(DiscordGuild guild)
        {

            //TODO
            throw new NotImplementedException();
        }

        [Listener(true)]
        public async Task OnMessage(MessageCreateEventArgs e)
        {
            //Filter words and somesuch
            var start = DateTime.UtcNow;
            await e.Channel.SendMessageAsync($"Got message {e.Message.Content}");
            await Task.Delay(5000);

            var end = DateTime.UtcNow;
            var delta = end - start;

            await e.Channel.SendMessageAsync($"Delta was {delta.TotalMilliseconds}, expected {5000}, diff {5000 - delta.TotalMilliseconds}");
            return;
        }


        [Listener(true)]
        public async Task OnMessageExtra(MessageCreateEventArgs e)
        {
            //Filter words and somesuch
            await e.Channel.SendMessageAsync($"MessageExtra no delay {new DateTime()}");
            return;
        }
    }
}
