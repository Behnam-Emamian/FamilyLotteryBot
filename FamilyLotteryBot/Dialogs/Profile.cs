using FamilyLotteryBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class Profile : IDialog<object>
    {
        readonly ResourceManager LocRM = new ResourceManager("FamilyLotteryBot.App_GlobalResources.Strings", typeof(Lottery).Assembly);
        public async Task StartAsync(IDialogContext context)
        {
            var Menu = new List<string>{
                LocRM.GetString("ProfileMenu1"),
                LocRM.GetString("ProfileMenu2"),
                LocRM.GetString("ProfileMenu3"),
                LocRM.GetString("ProfileMenu4")
            };

            var db = new Entities();
            var TelegramId = context.Activity.From.Id;
            var Profile = db.Profiles.Where(p => p.TelegramId == TelegramId).SingleOrDefault();
            if (Profile == null)
            {
                Profile = new Model.Profile()
                {
                    TelegramId = TelegramId,
                    Lang = "fa"
                };
                db.Profiles.Add(Profile);
                db.SaveChanges();
            }

            var ProfileMenuMessage = new StringBuilder();
            ProfileMenuMessage.AppendLine(LocRM.GetString("ProfileMenuMessage"));
            ProfileMenuMessage.AppendLine("نام: " + Profile.Name);
            ProfileMenuMessage.AppendLine("شماره حساب بانکی: " + Profile.BankAccount);

            PromptDialog.Choice(
                context,
                AfterSelectAsync,
                Menu,
                LocRM.GetString("ProfileMenuMessage") + "\n" + "نام: " + Profile.Name,
                LocRM.GetString("ProfileMenuMessage") + LocRM.GetString("BotPrompt"),
                10, PromptStyle.AutoText);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string SelectedMenu = await argument;

            if (SelectedMenu == LocRM.GetString("ProfileMenu1"))
                ;
            else if (SelectedMenu == LocRM.GetString("ProfileMenu2"))
                ;
            else if (SelectedMenu == LocRM.GetString("ProfileMenu3"))
                ;
            else if (SelectedMenu == LocRM.GetString("ProfileMenu4"))
                ;
        }
    }
}