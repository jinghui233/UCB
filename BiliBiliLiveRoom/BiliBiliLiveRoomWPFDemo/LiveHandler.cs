using BiliBiliLiveRoom.Live.Lib;
using BiliBiliLiveRoom.Live.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BiliBiliLiveRoomWPFDemo
{
    public class LiveHandler : IMessageHandler
    {
        //可以放置自己的参数用来使用,比如WPF的window对象
        public bool Param;
        public System.Windows.Controls.RichTextBox richTextBox;
        private void AppendTxT(string txt)
        {
            int max = 100;
            while (richTextBox.Document.Blocks.Count > max)
            {
                richTextBox.Document.Blocks.Remove(richTextBox.Document.Blocks.FirstBlock);
            }
            richTextBox.AppendText(txt + "\n");
            richTextBox.ScrollToEnd();
        }

        public async Task DanmuMessageHandlerAsync(DanmuMessage danmuMessage)
        {
            AppendTxT($"发送者:{danmuMessage.Username},内容:{danmuMessage.Content}");
        }

        public async Task AudiencesHandlerAsync(int audiences)
        {
            AppendTxT($"当前人气值:{audiences}");
        }

        public async Task NoticeMessageHandlerAsync(NoticeMessage noticeMessage)
        {
            AppendTxT("通知信息未处理");
        }

        public async Task GiftMessageHandlerAsync(GiftMessage giftMessage)
        {
            //如果礼物不是辣条
            if (giftMessage.GiftId != 1)
            {
                AppendTxT($"{giftMessage.Username}送出了{giftMessage.GiftNum}个{giftMessage.GiftName},价值:{giftMessage.TotalCoin}个{giftMessage.CoinType}");
            }
        }

        public async Task WelcomeMessageHandlerAsync(WelcomeMessage welcomeMessage)
        {
            AppendTxT($"欢迎{welcomeMessage.Username}进入直播间");
        }

        public async Task ComboEndMessageHandlerAsync(ComboEndMessage comboEndMessage)
        {
            AppendTxT($"{comboEndMessage.Username}的{comboEndMessage.GiftName}连击结束了,送出了{comboEndMessage.ComboNum}个,总价值{comboEndMessage.Price}个金瓜子");
        }

        public async Task RoomUpdateMessageHandlerAsync(RoomUpdateMessage roomUpdateMessage)
        {
            AppendTxT($"UP当前粉丝数量{roomUpdateMessage.Fans}");
        }

        public async Task WelcomeGuardMessageHandlerAsync(WelcomeGuardMessage welcomeGuardMessage)
        {
            AppendTxT($"房管{welcomeGuardMessage.Username}进入直播间");
        }

        public async Task LiveStartMessageHandlerAsync(int roomId)
        {
            AppendTxT("直播开始");
        }

        public async Task LiveStopMessageHandlerAsync(int roomId)
        {
            AppendTxT("直播关闭");
        }

        public async Task EntryEffectMessageHandlerAsync(EntryEffectMessage entryEffectMessage)
        {
            AppendTxT($"⚡⚡⚡<特效>⚡⚡⚡{entryEffectMessage.CopyWriting}⚡⚡⚡<特效>⚡⚡⚡");
        }

        public async Task GuardBuyMessageHandlerAsync(GuardBuyMessage guardBuyMessage)
        {
            AppendTxT($"{guardBuyMessage.Username}购买了{guardBuyMessage.Num}月的{guardBuyMessage.GiftName}");
        }

        public async Task UserToastMessageHandlerAsync(UserToastMessage userToastMessage)
        {
            AppendTxT($"{userToastMessage.Username}购买了{userToastMessage.Num}{userToastMessage.Unit}的{userToastMessage.RoleName}");
        }

        public async Task InteractWordMessageHandlerAsync(InteractWordMessage message)
        {
            if (!string.IsNullOrEmpty(message.Medal))
                AppendTxT($"{message.Medal}.{message.MedalLevel}  {message.Username} 进入直播间");
            else
                AppendTxT($"{message.Username} 进入直播间");
        }

        public async Task UnProcessMessageHandlerAsync(JObject message)
        {
            AppendTxT(message.ToString());
        }
    }
}
