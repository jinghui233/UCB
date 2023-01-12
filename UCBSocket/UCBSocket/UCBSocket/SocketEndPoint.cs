using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UCBSocket
{
    public class SocketEndPoint
    {
        private Socket socket;
        private bool isRec = false;
        private byte[] buffer = new byte[1024 * 1024 * 4];//4MB空间
        public IPEndPoint endPoint;
        public event Action<SocketEndPoint> DisConnected;
        public event Action<SocketEndPoint> UnexpectedDisconnection;
        public event Action<string> DebugMessage;
        public event Action<byte[], SocketEndPoint> ReceivedMessage;
        public SocketEndPoint(string ip, int port)
        {
            IPAddress address = IPAddress.Parse(ip);//创建 ip对象
            endPoint = new IPEndPoint(address, port);//创建网络节点对象 包含 ip和port
        }
        public SocketEndPoint(Socket socket)
        {
            this.socket = socket;
            endPoint = socket.RemoteEndPoint as IPEndPoint;
        }
        protected void PrintDebugMessage(string msg)
        {
            DebugMessage?.Invoke(msg);
        }
        public bool IsSocketConnected()
        {
            try
            {
                bool part1 = socket.Poll(1000, SelectMode.SelectRead);
                bool part2 = (socket.Available == 0);
                if (part1 && part2)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public virtual bool Connect()
        {
            DisConnectInternal();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//实例化 套接字 （ip4寻址协议，流式传输，TCP协议）
            try { socket.Connect(endPoint); } catch (Exception) { return false; }
            PrintDebugMessage($"连接成功{endPoint}");
            StartRecMsg();//开始接受服务器消息
            return true;
        }
        private void DisConnectInternal()
        {
            if (socket != null)
            {
                PrintDebugMessage($"关闭连接-{endPoint}");
                try { socket.Shutdown(SocketShutdown.Both); } catch (Exception) { }
                try { socket.Disconnect(false); } catch (Exception) { }
                try { socket.Close(); } catch (Exception) { }
                try { socket.Dispose(); } catch (Exception) { }
                DisConnected?.Invoke(this);
            }
        }
        public virtual void DisConnect()
        {
            DisConnectInternal();
        }
        public void StartRecMsg()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnRecCallBack, null);
            isRec = true;
        }
        private void OnRecCallBack(IAsyncResult asyncResult)
        {
            if (!IsSocketConnected())
            {
                PrintDebugMessage($"连接意外断开{endPoint}");
                DisConnectInternal();
                UnexpectedDisconnection?.Invoke(this);
                return;
            }
            int length = socket.EndReceive(asyncResult);
            byte[] recBytes = new byte[length];
            Array.Copy(buffer, 0, recBytes, 0, length);
            if (length > 0 && isRec && IsSocketConnected())
            {
                StartRecMsg();
                ReceivedMessage?.Invoke(recBytes, this);
            }
        }
        public bool Send(byte[] data)
        {
            if (!IsSocketConnected())
            {
                PrintDebugMessage($"连接意外断开{endPoint}");
                DisConnectInternal();
                UnexpectedDisconnection?.Invoke(this);
                return false;
            }
            socket.Send(data);
            return true;
        }
    }
}
