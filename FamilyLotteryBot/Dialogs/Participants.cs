using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FamilyLotteryBot.Dialogs
{
    [Serializable]
    public class Participants : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
        }
    }
}