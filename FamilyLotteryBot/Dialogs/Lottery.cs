using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using PersianDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Web;
using FamilyLotteryBot.Forms;
using System.Globalization;

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class Lottery : IDialog<object>
    {
        readonly ResourceManager LocRM = new ResourceManager("FamilyLotteryBot.App_GlobalResources.Strings", typeof(Lottery).Assembly);
        CultureInfo CultureInfo;
        Model.Lottery CurrentLottery;
        Model.Profile MyProfile;
        public async Task StartAsync(IDialogContext context)
        {
            CultureInfo = BusinessLogic.LoadCulture(context);
            CurrentLottery = BusinessLogic.LoadCurrentLottery(context);
            MyProfile = BusinessLogic.LoadProfile(context);
            await MessageReceivedAsync(context, null);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var Menu = new List<string>{
                LocRM.GetString("LotteryMenu1", CultureInfo),
                LocRM.GetString("LotteryMenu2", CultureInfo),
                LocRM.GetString("LotteryMenu3", CultureInfo),
                LocRM.GetString("LotteryMenu4", CultureInfo),
                LocRM.GetString("BackMenu", CultureInfo)
            };

            if (CurrentLottery != null)
            {
                await context.PostAsync(
                    "تاریخ شروع: " + ConvertDate.ToFa(CurrentLottery.StartDate.Value.Date) +
                    "  \nتاریخ پایان: " + ConvertDate.ToFa(CurrentLottery.EndDate.Value.Date) +
                    "  \nحداقل مبلغ: " + CurrentLottery.MinValue +
                    "  \nحداکثر مبلغ: " + CurrentLottery.MaxValue +
                    "  \nتعداد برنده: " + CurrentLottery.Winners);
            }
            else
            {
                await context.PostAsync(LocRM.GetString("Lottery_NoCurrentLottery", CultureInfo));
                context.Done("Back from Lottery");
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

            if (SelectedMenu == LocRM.GetString("LotteryMenu1", CultureInfo))
                PromptDialog.Number(context, AfterGettingValeu, LocRM.GetString("Lottery_Value_Enter", CultureInfo), LocRM.GetString("BotPrompt_EnterAgain", CultureInfo), 1);
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
            else if (SelectedMenu == LocRM.GetString("LotteryMenu4", CultureInfo))
                PromptDialog.Text(context, AfterGettingPassword, LocRM.GetString("Lottery_Password_Enter", CultureInfo), LocRM.GetString("BotPrompt_EnterAgain", CultureInfo), 1);
            else
                context.Done("Back from Lottery");
        }

        public async Task AfterParticipantForm(IDialogContext context, IAwaitable<object> argument)
        {
            var SelectedMenu = await argument;

            await MessageReceivedAsync(context, null);
        }

        public async Task AfterGettingPassword(IDialogContext context, IAwaitable<string> argument)
        {
            var LotteryPassword = await argument;

            if (CurrentLottery.Password.Trim() == LotteryPassword)
            {
                BusinessLogic.LotteryElection(CurrentLottery);
                await context.PostAsync(LocRM.GetString("Lottery_Successful", CultureInfo));
            }
            else
                await context.PostAsync(LocRM.GetString("Lottery_Password_Error", CultureInfo));

            await MessageReceivedAsync(context, null);
        }

        int ParticipantValue;
        public async Task AfterGettingValeu(IDialogContext context, IAwaitable<long> argument)
        {
            ParticipantValue = (int)await argument;

            PromptDialog.Text(context, AfterGettingReciepNo, LocRM.GetString("Lottery_ReciepNo_Enter", CultureInfo), LocRM.GetString("BotPrompt_EnterAgain", CultureInfo), 1);
        }

        string ParticipantReciepNo;
        public async Task AfterGettingReciepNo(IDialogContext context, IAwaitable<string> argument)
        {
            ParticipantReciepNo = await argument;

            BusinessLogic.AddParticipant(CurrentLottery.LotteryId, MyProfile.ProfileId, ParticipantValue, ParticipantReciepNo);
            await MessageReceivedAsync(context, null);
        }
    }
}