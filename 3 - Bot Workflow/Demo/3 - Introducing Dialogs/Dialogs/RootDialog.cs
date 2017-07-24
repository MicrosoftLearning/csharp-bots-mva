using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Call<UserProfile>(new EnsureProfileDialog(), ProfileEnsured);

            return Task.CompletedTask;
        }

        private async Task ProfileEnsured(IDialogContext context, IAwaitable<UserProfile> result)
        {
            var profile = await result;

            context.UserData.SetValue(@"profile", profile);

            await context.PostAsync($@"Hello {profile.Name}, I love {profile.Company}!");

            context.Wait(MessageReceivedAsync);
        }
    }

}
