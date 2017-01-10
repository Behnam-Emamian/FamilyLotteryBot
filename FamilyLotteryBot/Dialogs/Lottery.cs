using FamilyLotteryBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Web;

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class Lottery : IDialog<object>
    {
        readonly ResourceManager LocRM = new ResourceManager("FamilyLotteryBot.App_GlobalResources.Strings", typeof(Lottery).Assembly);
        public async Task StartAsync(IDialogContext context)
        {
            var Menu = new List<string>{
                LocRM.GetString("LotteryMenu1"),
                LocRM.GetString("LotteryMenu2"),
                LocRM.GetString("LotteryMenu3")
            };

            var db = new Entities();
            var Test = db.Lotteries;

            PromptDialog.Choice(
                context,
                AfterSelectAsync,
                Menu,
                LocRM.GetString("LotteryMenuMessage"),
                LocRM.GetString("LotteryMenuMessage") + LocRM.GetString("BotPrompt"),
                10);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;

            if (SelectedMenu == LocRM.GetString("MainMenu1"))
                ;
        }
    }
}