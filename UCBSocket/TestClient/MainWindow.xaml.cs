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

namespace TestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketClient socketClient;
        public MainWindow()
        {
            InitializeComponent();
            socketClient = new SocketClient(txtIP.Text, int.Parse(txtPort.Text));
            socketClient.RecMsgHandler = OnRecMsg;
        }
        private void OnRecMsg(byte[] arg1, SocketClient arg2)
        {
            string txt = Encoding.UTF8.GetString(arg1);
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Action(() => { richtxtLog.AppendText($"Receive:{txt}\n"); }));
            }
            else
            {
                richtxtLog.AppendText($"Receive:{txt}\n");
            }
        }
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (socketClient.Connect())
            {
                richtxtLog.AppendText("connected\n");
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (socketClient.Send(txtMsg.Text))
            {
                //richtxtLog.AppendText($"send:{txtMsg.Text.Length}\n");
            }
        }
    }
}
