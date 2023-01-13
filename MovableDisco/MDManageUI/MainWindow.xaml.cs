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
            if (player == null)
            {
                player = new Player() { UID = obj.UserId, UName = obj.Username, LastOperationTime = DateTime.Now };
                player.LastOperationTime = DateTime.Now;
                SendJson(DanmuInterpreter.AudienceEnter(obj.Username));
            }
            playerManager.Add(player);
        }
        private void OnDanmuReceived(DanmuMessage obj)
        {
            Player player = playerManager.RemoveGet((int)obj.UserId);
            if (player == null)
            {
                player = new Player() { UID = obj.UserId, UName = obj.Username, LastOperationTime = DateTime.Now };
            }
            player.LastOperationTime = DateTime.Now;
            playerManager.Add(player);
            SendJson(DanmuInterpreter.Interpret(obj.Content, obj.Username));
        }
        private void btnOpenRoom_Click(object sender, RoutedEventArgs e)
        {
            OpenRoom();
        }

        private void btnMenuSend_Click(object sender, RoutedEventArgs e)
        {
            if (liveHandler != null)
            {
                liveHandler.DanmuMessageHandlerAsync(new DanmuMessage() { UserId = 0, Username = "", Content = txtMenuSend.Text });
            }
        }
    }
}
