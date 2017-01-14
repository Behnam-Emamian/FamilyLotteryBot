using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class Language : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            //await MessageReceivedAsync(context, null);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var Menu = new List<string>{
                    "English",
                    "پارسی"
                };

            BusinessLogic.LoadProfile(context, true);

            PromptDialog.Choice(
                    context,
                    AfterSelectAsync,
                    Menu,
                    "Please select your language\n\nلطفا زبان خود را انتخاب نمایید",
                    "I didn't understand. Please try again.\n\nمتوجه نشدم لطفا دوباره تلاش کنید",
                    10);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;

            if (SelectedMenu == "English")
                context.UserData.SetValue("CultureInfo", new CultureInfo("en"));
            else if (SelectedMenu == "پارسی")
                context.UserData.SetValue("CultureInfo", new CultureInfo("fa"));

            await new Main().StartAsync(context);
        }
    }
}