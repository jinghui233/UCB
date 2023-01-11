using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UCBSocket
{
    public class SocketClient
    {
        public Action<byte[], SocketClient> RecMsgHandler;
        public Action<SocketClient> ClientCloseHandler;
        public Action<Exception> ExceptionHandler;
        public SocketClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//实例化 套接字 （ip4寻址协议，流式传输，TCP协议）
            IPAddress address = IPAddress.Parse(ip);//创建 ip对象
            endPoint = new IPEndPoint(address, port);//创建网络节点对象 包含 ip和port
        }
        private Socket socket;
        private string ip = "";
        private int port = 0;
        private bool isRec = true;
        private byte[] buffer = new byte[1024 * 1024 * 4];
        private IPEndPoint endPoint;
        private bool IsSocketConnected()
        {
            bool part1 = socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
        public bool Connect()
        {
            if (socket.Connected)
            {
                return true;
            }
            socket.Connect(endPoint); //将 监听套接字  绑定到 对应的IP和端口
            StartRecMsg();//开始接受服务器消息
            return true;
        }
        public bool StartRecMsg()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnRecCallBack, null);
            return true;
        }
        private void OnRecCallBack(IAsyncResult asyncResult)
        {
            int length = socket.EndReceive(asyncResult);
            byte[] recBytes = new byte[length];
            Array.Copy(buffer, 0, recBytes, 0, length);
            if (length > 0 && isRec && IsSocketConnected())
            {
                StartRecMsg();
                RecMsgHandler?.Invoke(recBytes, this);
            }
        }
        public bool Send(string msgStr)
        {
            socket.Send(Encoding.UTF8.GetBytes(msgStr));
            return true;
        }
        public bool Send(string msgStr, Encoding encoding)
        {
            socket.Send(encoding.GetBytes(msgStr));
            return true;
        }
        public void Close()
        {
            try
            {
                isRec = false;
                socket.Disconnect(false);
                ClientCloseHandler?.Invoke(this);
            }
            catch (Exception ex)
            {
                ExceptionHandler?.Invoke(ex);
            }
            finally
            {
                socket.Dispose();
                GC.Collect();
            }
        }
    }
}