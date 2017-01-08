using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class LotteriesArchive : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //PromptDialog.Choice(
            //    context: context,
            //    resume: AfterSelectAsync,
            //    options: Enum.GetValues(typeof(LotteriesMenu)).Cast<LotteriesMenu>().ToArray(),
            //    prompt: "Welcome to Family Lottery Bot! Please select from menu:",
            //    retry: "I didn't understand. Please try again.");
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;
        }
    }
}