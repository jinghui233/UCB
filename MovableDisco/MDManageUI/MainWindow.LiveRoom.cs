using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows;
using WPFUtils.Extension;
using BiliBiliLiveRoom.Live.Message;

namespace MDManageUI
{
    public partial class MainWindow
    {
        BiliBiliLiveRoom.Live.LiveRoom room;
        LiveHandler liveHandler;
        MessageDispatcher messageDispatcher;
        public void CloseRoom()
        {
            if (room != null)
            {
                liveHandler.MessageLog -= OnMessageLog;
                liveHandler.DanmuReceived -= OnDanmuReceived;
                liveHandler.AudienceEntered -= OnAudienceEntered;
                messageDispatcher.ReceivedMessage -= OnReceivedMessage;
                room.Disconnect();
                this.AppendText(rtxDanmu, "Room已关闭", true, true, 100);
            }
        }
        public async void OpenRoom()
        {
            if (room != null && room.Connected()) return;
            liveHandler = new LiveHandler();
            messageDispatcher = new MessageDispatcher();
            liveHandler.MessageLog += OnMessageLog;
            liveHandler.DanmuReceived += OnDanmuReceived;
            liveHandler.AudienceEntered += OnAudienceEntered;
            messageDispatcher.ReceivedMessage += OnReceivedMessage;
            room = new BiliBiliLiveRoom.Live.LiveRoom(int.Parse(txtRoomID.Text), liveHandler, messageDispatcher);
            if (!await room.ConnectAsync())
            {
                this.AppendText(rtxDanmu, "Room连接失败", true, true, 100);
                return;
            }
            this.AppendText(rtxDanmu, "Room已连接", true, true, 100);
            try
            {
                await room.ReadMessageLoop();
            }
            catch (Exception)
            {
                CloseRoom();
                OpenRoom();
            }
        }

        private void OnReceivedMessage(string obj)
        {
            biliLiveRoomContext.liveRoomOrigLogs.Add(new Models.LiveRoomOrigLog() { Message = obj, Time = DateTime.Now });
            biliLiveRoomContext.SaveChangesAsync();
        }

        private void OnMessageLog(string obj)
        {
            this.AppendText(rtxDanmu, obj + "\n", true, true, 100);
        }
    }
}
