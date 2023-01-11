using BiliBiliLiveRoom.Live.Lib;
using BiliBiliLiveRoom.Live.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliBiliLiveRoomWPFDemo
{
    internal class MessageDispatcher : IMessageDispatcher
    {
        public async Task DispatchAsync(JObject message, IMessageHandler messageHandler)
        {
            try
            {
                switch (message["cmd"].ToString())
                {
                    case "DANMU_MSG":
                        await messageHandler.DanmuMessageHandlerAsync(DanmuMessage.JsonToDanmuMessage(message));
                        break;
                    case "SEND_GIFT":
                        await messageHandler.GiftMessageHandlerAsync(GiftMessage.JsonToGiftMessage(message));
                        break;
                    case "WELCOME":
                        await messageHandler.WelcomeMessageHandlerAsync(WelcomeMessage.JsonToWelcomeMessage(message));
                        break;
                    case "INTERACT_WORD":
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
