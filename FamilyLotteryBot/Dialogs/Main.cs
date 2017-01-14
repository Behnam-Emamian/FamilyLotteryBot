using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using System.Resources;
using log4net;
using FamilyLotteryBot.Model;
using System.Globalization;

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class Main : IDialog<object>
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly ResourceManager LocRM = new ResourceManager("FamilyLotteryBot.App_GlobalResources.Strings", typeof(Main).Assembly);
        CultureInfo CultureInfo;
        public async Task StartAsync(IDialogContext context)
        {
            CultureInfo = BusinessLogic.LoadCulture(context);
            await MessageReceivedAsync(context, null);
        }
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var Profile = BusinessLogic.LoadProfile(context);

            var Menu = new List<string>{
                LocRM.GetString("MainMenu1", CultureInfo),
                //LocRM.GetString("MainMenu2", CultureInfo),
                LocRM.GetString("MainMenu3", CultureInfo)
            };

            if(Profile.Name == null || Profile.BankAccount == null)
                await context.PostAsync(LocRM.GetString("Profile_Alert", CultureInfo));

            PromptDialog.Choice(
                context,
                AfterSelectAsync,
                Menu,
                LocRM.GetString("MainMenuMessage", CultureInfo),
                LocRM.GetString("MainMenuMessage", CultureInfo) + LocRM.GetString("BotPrompt_TryAgain", CultureInfo),
                10);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;

            if (SelectedMenu == LocRM.GetString("MainMenu1", CultureInfo))
                context.Call(new Lottery(), AfterSubMenu);
            else if (SelectedMenu == LocRM.GetString("MainMenu2", CultureInfo))
                context.Call(new LotteriesArchive(), AfterSubMenu);
            else if (SelectedMenu == LocRM.GetString("MainMenu3", CultureInfo))
                context.Call(new Profile(), AfterSubMenu);
        }

        public async Task AfterSubMenu(IDialogContext context, IAwaitable<object> argument)
        {
            var SelectedMenu = await argument;

            await MessageReceivedAsync(context, null);
        }
    }
}