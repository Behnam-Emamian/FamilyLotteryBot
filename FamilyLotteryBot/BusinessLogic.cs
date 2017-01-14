using FamilyLotteryBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FamilyLotteryBot
{
    public class BusinessLogic
    {
        static Entities db = new Entities();
        public static Profile LoadProfile(IDialogContext context, bool Reload = false)
        {
            Profile Profile;
            if (!context.UserData.TryGetValue("Profile", out Profile) || Reload)
            {
                //context.Activity.ChannelId == "telegram"
                Profile = db.Profiles.Where(p => p.TelegramId == context.Activity.From.Id).SingleOrDefault();
                if (Profile == null)
                {
                    Profile = new Profile()
                    {
                        TelegramId = context.Activity.From.Id
                    };
                    db.Profiles.Add(Profile);
                    db.SaveChanges();
                }
                context.UserData.SetValue("Profile", Profile);
            }

            return Profile;
        }


        public static CultureInfo LoadCulture(IDialogContext context)
        {
            CultureInfo CultureInfo;
            context.UserData.TryGetValue("CultureInfo", out CultureInfo);
            return CultureInfo;
        }

        #region Lottery
        public static Lottery LoadCurrentLottery(IDialogContext context)
        {
            Lottery CurrentLottery;
            if (!context.UserData.TryGetValue("CurrentLottery", out CurrentLottery))
                CurrentLottery = db.Lotteries.Where(l => l.StartDate <= DateTime.Now && DateTime.Now <= l.EndDate).SingleOrDefault();
            return CurrentLottery;
        }

        public static void LotteryElection(Lottery CurrentLottery)
        {



        }
        #endregion

        #region Participant
        public static void AddParticipant(int LotteryId, int ProfileId, int Value, string ReciepNo)
        {
            var Participant = db.Participants.Where(p => p.LotteryId == LotteryId && p.ProfileId == ProfileId).SingleOrDefault();

            if (Participant != null)
            {
                Participant.Value = Value;
                Participant.ReciepNo = ReciepNo;
                Participant.IsAccepted = false;
                db.SaveChanges();
            }
            else
            {
                db.Participants.Add(new Participant
                {
                    LotteryId = LotteryId,
                    ProfileId = ProfileId,
                    Value = Value,
                    ReciepNo = ReciepNo,
                    IsWinner = false,
                    IsAccepted = false
                });
                db.SaveChanges();
            }

        }
        #endregion

        #region Profile
        public static void UpdateProfileName(int ProfileId, string Name)
        {
            db.Profiles.Where(p => p.ProfileId == ProfileId).SingleOrDefault().Name = Name;
            db.SaveChanges();
        }
        public static void UpdateProfileBankAccount(int ProfileId, string BankAccount)
        {
            db.Profiles.Where(p => p.ProfileId == ProfileId).SingleOrDefault().BankAccount = BankAccount;
            db.SaveChanges();
        }
        #endregion
    }
}