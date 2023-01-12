using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
using UCBSocket.Extension;
using WPFUtils.Extension;

namespace TestServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketServer socketServer;
        SocketConnection socketConnection;
        public MainWindow()
        {
            InitializeComponent();
            socketServer = new SocketServer(txtIP.Text, int.Parse(txtPort.Text));
            socketServer.ClientReceivedMessage += OnRecMsg;
            socketServer.DebugMessage += OnDebugMsg;
            socketServer.ClientConnected += OnClientConnected;
        }

        private void OnClientConnected(SocketServer arg1, SocketConnection arg2)
        {
            socketConnection = arg2;
        }

        private void OnDebugMsg(string obj)
        {
            this.AppendText(rtxLog, obj + "\n");
        }

        private void OnRecMsg(byte[] arg1, SocketEndPoint arg2)
        {
            string txt = Encoding.UTF8.GetString(arg1);
            this.AppendText(rtxLog, $"{arg2.endPoint}：{txt}\n");
        }
        private void btnListen_Click(object sender, RoutedEventArgs e)
        {
            if (socketServer.StartServer())
            {
                rtxLog.AppendText("On Listening\n");
            }
        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (socketConnection != null)
            {
                socketConnection.Send(txtMsg.Text);
            }
        }
        private void btnSendJson_Click(object sender, RoutedEventArgs e)
        {
            if (socketConnection != null)
            {
                socketConnection.SendAsJson(TestJsonData.GetRandomDatas(3));
            }
        }
    }
}
