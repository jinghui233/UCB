using MDManageUI;
using System;
using System.Text;
using System.Windows;
using UCBSocket;
using UCBSocket.Extension;
using Utils;
using WPFUtils.Extension;

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
            socketClient = new SocketClient(txtIP.Text, int.Parse(txtPort.Text), true);
            socketClient.ReceivedMessage += OnRecMsg;
            socketClient.DebugMessage += OnDebugMsg;
        }

        private void OnDebugMsg(string obj)
        {
            this.AppendText(rtxLog, obj + "\n");
        }

        private void OnRecMsg(byte[] arg1, SocketEndPoint arg2)
        {
            try
            {
                TransferredData transferredData = TransferredData.FromJson(arg1);
                if (transferredData.TypeName == "NormAction")
                {
                    NormAction audienceEnter = transferredData.GetCommand<NormAction>();
                    this.AppendText(rtxLog, audienceEnter.ActionName + "   success");
                }
                else if (transferredData.TypeName == "MoveSimple")
                {
                    MoveSimple moveSimple = transferredData.GetCommand<MoveSimple>();
                    this.AppendText(rtxLog, moveSimple.Up + "   success");
                }
                this.AppendText(rtxLog, "转换成功");
            }
            catch (Exception)
            {
                throw;
            }
            string txt = Encoding.UTF8.GetString(arg1);
            this.AppendText(this.rtxLog, $"{arg2.endPoint}：{txt}\n");
        }
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            socketClient.Connect();
        }
        private void btnDisConnect_Click(object sender, RoutedEventArgs e)
        {
            socketClient.DisConnect();
        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            socketClient.Send(txtMsg.Text);
        }

        private void btnSendJson_Click(object sender, RoutedEventArgs e)
        {
            socketClient.SendAsJson(TestJsonData.GetRandomDatas(3));
        }
    }
}
