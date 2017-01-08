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
                        await Conversation.SendAsync(activity, () => new Main());
                        break;

                    case ActivityTypes.DeleteUserData:
                    case ActivityTypes.ConversationUpdate:
                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.Ping:

                        break;

                }
            }

            /*
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
            await connector.Conversations.ReplyToActivityAsync(reply);

            {
                HandleSystemMessage(activity);
            }*/

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}