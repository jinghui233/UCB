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
using WPFUtils.Extension;

namespace TestServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketServer socketServer;
        public MainWindow()
        {
            InitializeComponent();
            socketServer = new SocketServer(txtIP.Text, int.Parse(txtPort.Text));
            socketServer.HandleRecMsg = OnRecMsg;
        }
        private void OnRecMsg(byte[] arg1, SocketConnection arg2, SocketServer arg3)
        {
            string txt = Encoding.UTF8.GetString(arg1);
            this.AppendText(rtxLog, $"Receive:{txt}\n");
        }
        private void btnListen_Click(object sender, RoutedEventArgs e)
        {
            if (socketServer.StartServer())
            {
                rtxLog.AppendText("On Listening\n");
            }
        }
    }
}
