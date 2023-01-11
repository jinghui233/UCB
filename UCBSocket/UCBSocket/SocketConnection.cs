using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UCBSocket
{
    public class SocketConnection
    {
        private Socket socket;
        private SocketServer server = null;
        private byte[] buffer = new byte[1024 * 1024 * 4];
        private bool isRec = true;
        public Action<byte[], SocketConnection, SocketServer> RecMsgHandler { get; set; }
        public Action<SocketConnection, SocketServer> ClientCloseHandler { get; set; }
        public Action<Exception> HandleException { get; set; }
        public SocketConnection(Socket socket, SocketServer server)
        {
            this.socket = socket;
            this.server = server;
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
        private bool IsSocketConnected()
        {
            bool part1 = socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
        public bool StartRecMsg()
        {
            try
            {
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecCallBack, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
            return true;
        }
        private void RecCallBack(IAsyncResult asyncResult)
        {
            try
            {
                int length = socket.EndReceive(asyncResult);
                byte[] recBytes = new byte[length];
                Array.Copy(buffer, 0, recBytes, 0, length);
                if (length > 0 && isRec && IsSocketConnected())
                {
                    StartRecMsg();
                    RecMsgHandler?.Invoke(recBytes, this, server);
                }
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }
        public void Close()
        {
            try
            {
                isRec = false;
                socket.Disconnect(false);
                server.RemoveConnection(this);
                ClientCloseHandler?.Invoke(this, server);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
            finally
            {
                socket.Dispose();
                GC.Collect();
            }
        }
    }
}
