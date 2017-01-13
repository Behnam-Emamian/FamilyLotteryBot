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
    public class Admin : IDialog<object>
    {
        readonly ResourceManager LocRM = new ResourceManager("FamilyLotteryBot.App_GlobalResources.Strings", typeof(Lottery).Assembly);
        public async Task StartAsync(IDialogContext context)
        {
            await MessageReceivedAsync(context, null);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var CultureInfo = BusinessLogic.LoadCulture(context);

            var Menu = new List<string>{
                    LocRM.GetString("AdminMenu1", CultureInfo),
                    LocRM.GetString("AdminMenu2", CultureInfo),
                    LocRM.GetString("BackMenu", CultureInfo)
                };

            PromptDialog.Choice(
                context,
                AfterSelectAsync,
                Menu,
                LocRM.GetString("AdminMenuMessage", CultureInfo),
                LocRM.GetString("AdminMenuMessage", CultureInfo) + LocRM.GetString("BotPrompt_TryAgain", CultureInfo),
                10);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;

            var CultureInfo = BusinessLogic.LoadCulture(context);
            var Profile = BusinessLogic.LoadProfile(context);

            if (SelectedMenu == LocRM.GetString("AdminMenu1", CultureInfo))
                ;
            else if (SelectedMenu == LocRM.GetString("AdminMenu1", CultureInfo))
                ;
            else if (SelectedMenu == LocRM.GetString("BackMenu", CultureInfo))
                context.Done("Back from profile");
        }
    }
}