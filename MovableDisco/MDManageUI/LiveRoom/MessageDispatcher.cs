using BiliBiliLiveRoom.Live.Lib;
using BiliBiliLiveRoom.Live.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDManageUI
{
    internal class MessageDispatcher : IMessageDispatcher
    {
        public event Action<string> ReceivedMessage;
        public async Task DispatchAsync(JObject message, IMessageHandler messageHandler)
        {
            try
            {
                switch (message["cmd"].ToString())
                {
                    case "DANMU_MSG":
                        ReceivedMessage?.Invoke(message.ToString());
                        await messageHandler.DanmuMessageHandlerAsync(DanmuMessage.JsonToDanmuMessage(message));
                        break;
                    case "SEND_GIFT":
                        ReceivedMessage?.Invoke(message.ToString());
                        await messageHandler.GiftMessageHandlerAsync(GiftMessage.JsonToGiftMessage(message));
                        break;
                    case "WELCOME":
                        ReceivedMessage?.Invoke(message.ToString());
                        await messageHandler.WelcomeMessageHandlerAsync(WelcomeMessage.JsonToWelcomeMessage(message));
                        break;
                    case "INTERACT_WORD":
                        ReceivedMessage?.Invoke(message.ToString());
                        await messageHandler.InteractWordMessageHandlerAsync(InteractWordMessage.JsonToInteractWordMessage(message));
                        break;
                    default:
                        //Debug.WriteLine("未记录的信息");
                        //Debug.WriteLine(message);
                        break;
                }
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}
