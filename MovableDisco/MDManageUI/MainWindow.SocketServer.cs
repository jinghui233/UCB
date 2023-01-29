using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCBSocket;
using UCBSocket.Extension;
using WPFUtils.Extension;

namespace MDManageUI
{
    public partial class MainWindow
    {
        SocketServer socketServer;
        SocketConnection socketConnection;
        private void InitSocketServer()
        {
            socketServer = new SocketServer(txtIP.Text, int.Parse(txtPort.Text));
            socketServer.StartServer();
            socketServer.ClientReceivedMessage += OnRecMsg;
            socketServer.DebugMessage += OnDebugMsg;
            socketServer.ClientConnected += OnClientConnected;
        }
        private void OnClientConnected(SocketServer arg1, SocketConnection arg2)
        {
            socketConnection = arg2;
            UpdateAllPlayers();
        }
        private void OnDebugMsg(string obj)
        {
            this.AppendText(rtxCommand, obj + "\n");
        }
        private void OnRecMsg(byte[] arg1, SocketEndPoint arg2)
        {
            string txt = Encoding.UTF8.GetString(arg1);
            this.AppendText(rtxCommand, $"{arg2.endPoint}：{txt}\n");
        }
        private void SendMessage(string msg)
        {
            if (socketConnection == null)
            {
                this.AppendText(rtxCommand, "SocketServer-信息未发送，无连接的Client\n");
                return;
            }
            socketConnection.Send(msg);
            this.AppendText(rtxCommand, "SocketServer-信息发送成功\n");
        }
        private void SendJson(object obj)
        {
            if (socketConnection == null)
            {
                this.AppendText(rtxCommand, "SocketServer-数据未发送，无连接的Client\n");
                return;
            }
            socketConnection.SendAsJson(obj);
            this.AppendText(rtxCommand, "SocketServer-数据发送成功\n");
        }
    }
}
