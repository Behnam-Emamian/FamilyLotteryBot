using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using FamilyLotteryBot.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using log4net;
using System.Web.Script.Serialization;

namespace FamilyLotteryBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null)
            {
                logger.Info("Activity:\n" + JsonConvert.SerializeObject(activity));

                switch (activity.Type)
                {
                    case ActivityTypes.Message:
                        if (activity.Text == "/start")
                        {
                            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                            await connector.Conversations.ReplyToActivityAsync(activity.CreateReply("Welcome to Family Lottery Bot!  \nبه ربات لاتاری خانوادگی خوش آمدید"));
                        }

                        await Conversation.SendAsync(activity, () => new Language());
                        break;

                    case ActivityTypes.ConversationUpdate:
                        break;
                    case ActivityTypes.DeleteUserData:
                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.Ping:

                        break;
                }
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}