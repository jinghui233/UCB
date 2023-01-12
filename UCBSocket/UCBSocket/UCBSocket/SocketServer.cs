using System.Net;
using System.Net.Sockets;
using UCBSocket;

namespace UCBSocket
{
    public class SocketServer
    {
        private string ip;
        private int port;
        private Socket socket;
        private bool isListen = true;
        private ReaderWriterLockSlim RWLock_ClientList;
        private List<SocketConnection> clientList;
        public event Action<SocketServer, SocketConnection> ClientConnected;
        public event Action<byte[], SocketEndPoint> ClientReceivedMessage;
        public event Action<SocketEndPoint> ClientDisConnected;
        public event Action<string> DebugMessage;
        public SocketServer(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            RWLock_ClientList = new ReaderWriterLockSlim();
            clientList = new List<SocketConnection>();
        }

        public bool StartServer(int backlog = 10)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//实例化套接字（ip4寻址协议，流式传输，TCP协议）
            IPAddress address = IPAddress.Parse(ip);//创建ip对象
            IPEndPoint endpoint = new IPEndPoint(address, port); //创建网络节点对象包含ip和port
            socket.Bind(endpoint);//将 监听套接字绑定到 对应的IP和端口
            socket.Listen(backlog); //设置监听队列长度为Int32最大值(同时能够处理连接请求数量)
            StartListen();//开始监听客户端
            return true;
        }
        private void StartListen()
        {
            socket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult asyncResult)
        {
            Socket newSocket = socket.EndAccept(asyncResult);
            if (isListen)
            {
                StartListen();
            }
            SocketConnection socketConnection = new SocketConnection(newSocket);
            socketConnection.ReceivedMessage += OnClientReceivedMessage;
            socketConnection.DisConnected += OnClientDisConnected;
            socketConnection.DebugMessage += PrintDebugMessage;
            socketConnection.UnexpectedDisconnection += OnUnexpectedDisconnection;
            socketConnection.StartRecMsg();
            AddConnection(socketConnection);
            ClientConnected?.Invoke(this, socketConnection);
            PrintDebugMessage($"{socketConnection.endPoint}已连接");
        }
        private void OnClientReceivedMessage(byte[] arg1, SocketEndPoint arg2)
        {
            ClientReceivedMessage?.Invoke(arg1, arg2);
        }
        private void OnClientDisConnected(SocketEndPoint obj)
        {
            PrintDebugMessage($"{obj.endPoint}断开连接");
            ClientDisConnected?.Invoke(obj);
        }
        protected void PrintDebugMessage(string msg)
        {
            DebugMessage?.Invoke(msg);
        }
        private void OnUnexpectedDisconnection(SocketEndPoint obj)
        {

        }
        public void AddConnection(SocketConnection theConnection)
        {
            RWLock_ClientList.EnterWriteLock();
            try
            {
                clientList.Add(theConnection);
            }
            finally
            {
                RWLock_ClientList.ExitWriteLock();
            }
        }
        public void RemoveConnection(SocketConnection theConnection)
        {
            RWLock_ClientList.EnterWriteLock();
            try
            {
                if (theConnection != null && clientList.Contains(theConnection))
                    clientList.Remove(theConnection);
            }
            finally
            {
                RWLock_ClientList.ExitWriteLock();
            }
        }
        public void DisConnect(SocketConnection theConnection)
        {
            RWLock_ClientList.EnterWriteLock();
            try
            {
                theConnection.DisConnect();
            }
            finally
            {
                RWLock_ClientList.ExitWriteLock();
            }
        }
        public void Close()
        {
            foreach (var client in clientList)
            {
                DisConnect(client);
                RemoveConnection(client);
            }
        }
    }
}
