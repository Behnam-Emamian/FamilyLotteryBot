using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyLotteryBot.Forms
{
    [Serializable]

    public class Participant
    {
        [Prompt("لطفا مبلغ شرکت در لاتاری را وارد نمایید")]
        public int Value;

        public static IForm<Participant> BuildForm()
        {
            return new FormBuilder<Participant>()
                    .Message("Welcome to the simple sandwich order bot!")
                    .Build();
        }
    }
}