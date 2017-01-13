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
            await MessageReceivedAsync(context, null);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var CultureInfo = BusinessLogic.LoadCulture(context);

            var Menu = new List<string>{
                LocRM.GetString("LotteryMenu1", CultureInfo),
                LocRM.GetString("LotteryMenu2", CultureInfo),
                LocRM.GetString("LotteryMenu3", CultureInfo),
                LocRM.GetString("BackMenu", CultureInfo)
            };

            var Lottery = BusinessLogic.LoadCurrentLottery(context);
            if (Lottery != null)
            {
                await context.PostAsync("تاریخ شروع: " + Lottery.StartDate + ", تاریخ پایان: " + Lottery.EndDate);

            }
            else
            {
                await context.PostAsync("لاتاری در حال برگزاری نیست");
            }

            PromptDialog.Choice(
                context,
                AfterSelectAsync,
                Menu,
                LocRM.GetString("LotteryMenuMessage", CultureInfo),
                LocRM.GetString("LotteryMenuMessage", CultureInfo) + LocRM.GetString("BotPrompt_TryAgain", CultureInfo),
                10);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;

            var CultureInfo = BusinessLogic.LoadCulture(context);

            if (SelectedMenu == LocRM.GetString("LotteryMenu1", CultureInfo))
            {
                await context.PostAsync("test");
                await MessageReceivedAsync(context, null);
            }
            else if (SelectedMenu == LocRM.GetString("LotteryMenu2", CultureInfo))
            {
                await context.PostAsync(LocRM.GetString("LotteryMenu2", CultureInfo));
                await MessageReceivedAsync(context, null);
            }
            else if (SelectedMenu == LocRM.GetString("LotteryMenu3", CultureInfo))
            {
                await context.PostAsync(LocRM.GetString("LotteryMenu3", CultureInfo));
                await MessageReceivedAsync(context, null);
            }
            else if (SelectedMenu == LocRM.GetString("BackMenu", CultureInfo))
                context.Done("Back from Lottery");
        }
    }
}