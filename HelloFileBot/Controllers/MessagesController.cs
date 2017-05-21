using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;

namespace HelloFileBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //Path where files will be saved. Please provide the path where you would like to
                //save the files.
                string saveFileInPath = @"C:\BotDirectory\";
                //Loop through all attachments sent by user
                foreach (Attachment attachment in activity.Attachments)
                {
                    //Get the url from where to download the file that user has sent
                    string url = attachment.ContentUrl;
                    //Code to download file
                    Uri uri = new Uri(url);
                    using (var webClient = new WebClient())
                    {
                        //attachment.Name gives the name of the file. Save the file with the smae name
                        webClient.DownloadFile(url, saveFileInPath+attachment.Name);
                    }
                }
                Activity message = activity.CreateReply("Thank you. We have received the files.");
                //Send Reply message
                await connector.Conversations.ReplyToActivityAsync(message);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}