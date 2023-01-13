using BiliBiliLiveRoom.Live.Message;
using MDManageUI.Command;
using MDManageUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public MainWindow()
        {
            InitializeComponent();
            biliLiveRoomContext = new BiliLiveRoomContext();
            playerManager = new PlayerManager();
            playerManager.MaxPlayerNumber = 100;
            dataGrid1.DataContext = playerManager.Players;
            InitSocketServer();
        }
        private void OnAudienceEntered(InteractWordMessage obj)
        {
            Player player = playerManager.RemoveGet((int)obj.UserId);
            if (player != null)
            {
                player.LastOperationTime = DateTime.Now;
            }
            player = new Player() { UID = (int)obj.UserId, UName = obj.Username, LastOperationTime = DateTime.Now };
            playerManager.Add(player);
            SendJson(DanmuInterpreter.AudienceEnter(obj.Username));
        }
        private void OnDanmuReceived(DanmuMessage obj)
        {
            Player player = playerManager.RemoveGet((int)obj.UserId);
            if (player == null)
            {
                player = new Player() { UID = (int)obj.UserId, UName = obj.Username, LastOperationTime = DateTime.Now };
                SendJson(DanmuInterpreter.Interpret(obj.Content, obj.Username));
            }
            //创建角色，转圈圈，向上下左右移动，换装
            player.LastOperationTime = DateTime.Now;
            playerManager.Add(player);
        }
        private void btnOpenRoom_Click(object sender, RoutedEventArgs e)
        {
            OpenRoom();
        }

        private void btnMenuSend_Click(object sender, RoutedEventArgs e)
        {
            liveHandler.DanmuMessageHandlerAsync(new DanmuMessage() { UserId = 0, Username = "", Content = txtMenuSend.Text });
        }
    }
}
