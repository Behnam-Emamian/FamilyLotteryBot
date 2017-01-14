using FamilyLotteryBot.Model;
using log4net;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.Bot.Builder.Dialogs.PromptDialog;

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class Profile : IDialog<object>
    {
        readonly ResourceManager LocRM = new ResourceManager("FamilyLotteryBot.App_GlobalResources.Strings", typeof(Lottery).Assembly);
        static readonly ILog Logger = LogManager.GetLogger("Errors");
        public async Task StartAsync(IDialogContext context)
        {
            await MessageReceivedAsync(context, null);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var CultureInfo = BusinessLogic.LoadCulture(context);
            var Profile = BusinessLogic.LoadProfile(context);

            var Menu = new List<string>{
                    LocRM.GetString("ProfileMenu1", CultureInfo),
                    LocRM.GetString("ProfileMenu2", CultureInfo),
                    LocRM.GetString("BackMenu", CultureInfo)
                };

            await context.PostAsync(LocRM.GetString("Profile_Name", CultureInfo)+ ": " + Profile.Name + "  \n" + LocRM.GetString("Profile_BankAccount", CultureInfo) + ": " + Profile.BankAccount);

            PromptDialog.Choice(
                context,
                AfterSelectAsync,
                Menu,
                LocRM.GetString("ProfileMenuMessage", CultureInfo),
                LocRM.GetString("ProfileMenuMessage", CultureInfo) + LocRM.GetString("BotPrompt_TryAgain", CultureInfo),
                10);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;

            var CultureInfo = BusinessLogic.LoadCulture(context);
            var Profile = BusinessLogic.LoadProfile(context);

            if (SelectedMenu == LocRM.GetString("ProfileMenu1", CultureInfo))
                PromptDialog.Text(context, AfterGettingName, LocRM.GetString("Profile_Name_Enter", CultureInfo), LocRM.GetString("BotPrompt_EnterAgain", CultureInfo), 1);
            else if (SelectedMenu == LocRM.GetString("ProfileMenu2", CultureInfo))
                PromptDialog.Text(context, AfterGettingBankAccount, LocRM.GetString("Profile_BankAccount_Enter", CultureInfo), LocRM.GetString("BotPrompt_EnterAgain", CultureInfo), 1);
            else
                context.Done("Back from profile");
        }

        public async Task AfterGettingName(IDialogContext context, IAwaitable<string> argument)
        {
            var Name = await argument;
            BusinessLogic.UpdateProfileName(context, Name);
            await MessageReceivedAsync(context, null);
        }

        public async Task AfterGettingBankAccount(IDialogContext context, IAwaitable<string> argument)
        {
            var BankAccount = await argument;
            BusinessLogic.UpdateProfileBankAccount(context, BankAccount);
            await MessageReceivedAsync(context, null);
        }
    }
}