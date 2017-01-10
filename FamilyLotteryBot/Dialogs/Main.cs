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

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class Main : IDialog<object>
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly ResourceManager LocRM = new ResourceManager("FamilyLotteryBot.App_GlobalResources.Strings", typeof(Main).Assembly);


        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            logger.Info(message);

            var Menu = new List<string>{
                LocRM.GetString("MainMenu1"),
                //LocRM.GetString("MainMenu2"),
                LocRM.GetString("MainMenu3")
            };

            PromptDialog.Choice(
                context,
                AfterSelectAsync,
                Menu,
                LocRM.GetString("MainMenuMessage"),
                LocRM.GetString("BotPrompt"),
                10);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;

            if (SelectedMenu == LocRM.GetString("MainMenu1"))
                await new Lottery().StartAsync(context);
            else if (SelectedMenu == LocRM.GetString("MainMenu2"))
                await new LotteriesArchive().StartAsync(context);
            else if (SelectedMenu == LocRM.GetString("MainMenu3"))
                await new Profile().StartAsync(context);
        }
    }
}