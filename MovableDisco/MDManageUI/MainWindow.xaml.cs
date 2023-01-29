using BiliBiliLiveRoom.Live.Message;
using MDManageUI.Command;
using MDManageUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UCBSocket;
using Utils;
using WPFUtils.Extension;

namespace MDManageUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BiliLiveRoomContext biliLiveRoomContext;
        PlayerManager playerManager;
        public static int nemaCount;
        public MainWindow()
        {
            InitializeComponent();
            biliLiveRoomContext = new BiliLiveRoomContext();
            playerManager = new PlayerManager();
            playerManager.MaxPlayerNumber = 100;
            dataGrid1.DataContext = playerManager.Players;
            InitSocketServer();
        }
        public async void UpdateAllPlayers()
        {
            SendJson(DanmuInterpreter.Interpret("ClrAllPlyrs", ""));
            foreach (Player item in CollectionsHelper.Copy(playerManager.Players))
            {
                await Task.Run(() => { Thread.Sleep(300); });
                SendJson(DanmuInterpreter.AudienceEnter(item.UName, true));
            }
        }
        public void UpdatePlayerNum()
        {
            curPlayerNum.Content = playerManager.PlayerList.Count.ToString();
        }
        private void OnAudienceEntered(InteractWordMessage obj)
        {
            Player player = playerManager.RemoveGet((int)obj.UserId);
            if (player == null)
            {
                if (cbxEnterAdd.IsChecked.Value)
                {
                    player = new Player() { UID = obj.UserId, UName = obj.Username, LastOperationTime = DateTime.Now };
                    player.LastOperationTime = DateTime.Now;
                    SendJson(DanmuInterpreter.AudienceEnter(obj.Username, false));
                    SendJson(DanmuInterpreter.AudienceEnter(obj.Username, cbxEnterAdd.IsChecked.Value));
                }
                else
                {
                    SendJson(DanmuInterpreter.AudienceEnter(obj.Username, cbxEnterAdd.IsChecked.Value));
                }
            }
            if (player != null)
            {
                playerManager.Add(player);
            }
            UpdatePlayerNum();
            if (playerManager.PlayerList.Count > 30)
            {
                cbxEnterAdd.IsChecked = false;
            }
        }
        private void OnDanmuReceived(DanmuMessage obj)
        {
            Player player = playerManager.RemoveGet(obj.UserId);
            if (player == null)
            {
                SendJson(DanmuInterpreter.Interpret("AddCharacter", obj.Username));
                player = new Player() { UID = obj.UserId, UName = obj.Username, LastOperationTime = DateTime.Now };
                Player removePlayer = playerManager.LimitCount();
                if (removePlayer != null)
                {
                    SendJson(DanmuInterpreter.Interpret("删除角色", removePlayer.UName));
                }
            }
            player.LastOperationTime = DateTime.Now;
            playerManager.Add(player);
            SendJson(DanmuInterpreter.Interpret(obj.Content, obj.Username));
            if (obj.Content.Trim() == "删除角色")
            {
                playerManager.RemoveGet(obj.UserId);
            }
            UpdatePlayerNum();
        }
        private void btnOpenRoom_Click(object sender, RoutedEventArgs e)
        {
            OpenRoom();
        }
        private void btnMenuSend_Click(object sender, KeyEventArgs e)
        {
            if (liveHandler != null)
            {
                liveHandler.DanmuMessageHandlerAsync(new DanmuMessage() { UserId = 0, Username = txtMenuSendUName.Text, Content = txtMenuSendDanmu.Text });
            }
        }
        private void btnMenuAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (liveHandler != null)
            {
                int a = nemaCount++;
                liveHandler.DanmuMessageHandlerAsync(new DanmuMessage() { UserId = a, Username = "nema" + a, Content = "创建角色" });
            }
        }
        private void btnNormText_Click(object sender, RoutedEventArgs e)
        {
            if (liveHandler != null)
            {
                foreach (Player item in CollectionsHelper.Copy(dataGrid1.SelectedItems))
                {
                    liveHandler.DanmuMessageHandlerAsync(new DanmuMessage() { UserId = item.UID, Username = item.UName, Content = (sender as Button).Content.ToString() });
                }
            }
        }
        private void btnDebugDanmu_Click(object sender, RoutedEventArgs e)
        {
            if (liveHandler != null)
            {
                liveHandler.DanmuMessageHandlerAsync(new DanmuMessage() { Content = (sender as Button).Content.ToString() });
            }
        }
        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (liveHandler != null)
            {
                foreach (Player item in CollectionsHelper.Copy(dataGrid1.SelectedItems))
                {
                    liveHandler.DanmuMessageHandlerAsync(new DanmuMessage() { UserId = item.UID, Username = item.UName, Content = "向下移动" });
                }
            }
        }

        private async void btnUseNumLimit_Click(object sender, RoutedEventArgs e)
        {
            playerManager.MaxPlayerNumber = int.Parse(txtMaxPlayerNum.Text);
            while (true)
            {
                Player removePlayer = playerManager.LimitCount();
                if (removePlayer != null)
                {
                    await Task.Run(() => { Thread.Sleep(100); });
                    SendJson(DanmuInterpreter.Interpret("删除角色", removePlayer.UName));
                }
                else
                {
                    break;
                }
            }
        }
    }
}
