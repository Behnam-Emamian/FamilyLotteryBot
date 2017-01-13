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
        public static Model.Profile LoadProfile(IDialogContext context, bool Reload = false)
        {
            Profile Profile;
            if (!context.UserData.TryGetValue("Profile", out Profile) || Reload)
            {
                //context.Activity.ChannelId == "telegram"
                Profile = db.Profiles.Where(p => p.TelegramId == context.Activity.From.Id).SingleOrDefault();
                if (Profile == null)
                {
                    Profile = new Model.Profile()
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

        public static Lottery LoadCurrentLottery(IDialogContext context)
        {
            var Lottery = db.Lotteries.Where(l => l.StartDate >= DateTime.Now && l.EndDate <= DateTime.Now).SingleOrDefault();


            return Lottery;
        }

        #region Profile
        public static void UpdateProfileName(IDialogContext context, string Name)
        {
            var Profile = db.Profiles.Where(p => p.TelegramId == context.Activity.From.Id).SingleOrDefault();
            Profile.Name = Name;
            db.SaveChanges();
            LoadProfile(context, true);
        }
        public static void UpdateProfileBankAccount(IDialogContext context, string BankAccount)
        {
            var Profile = db.Profiles.Where(p => p.TelegramId == context.Activity.From.Id).SingleOrDefault();
            Profile.BankAccount = BankAccount;
            db.SaveChanges();
            LoadProfile(context, true);
        }
        #endregion
    }
}