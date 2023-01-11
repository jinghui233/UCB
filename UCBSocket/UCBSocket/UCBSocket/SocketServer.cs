using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public Action<SocketServer, SocketConnection> HandleNewClientConnected;
        public Action<Exception> HandleException;
        public Action<byte[], SocketConnection, SocketServer> HandleRecMsg;
        public Action<SocketConnection, SocketServer> HandleClientClose;
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
            SocketConnection socketConnection = new SocketConnection(newSocket, this)
            {
                RecMsgHandler = HandleRecMsg == null ? null : new Action<byte[], SocketConnection, SocketServer>(HandleRecMsg),
                ClientCloseHandler = HandleClientClose == null ? null : new Action<SocketConnection, SocketServer>(HandleClientClose),
                HandleException = HandleException == null ? null : new Action<Exception>(HandleException)
            };
            socketConnection.StartRecMsg();
            AddConnection(socketConnection);
            HandleNewClientConnected?.Invoke(this, socketConnection);
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
                theConnection.Close();
                clientList.Remove(theConnection);
            }
            finally
            {
                RWLock_ClientList.ExitWriteLock();
            }
        }
        public int GetConnectionCount()
        {
            RWLock_ClientList.EnterReadLock();
            try
            {
                return clientList.Count;
            }
            finally
            {
                RWLock_ClientList.ExitReadLock();
            }
        }
        public void Close()
        {
            foreach (var client in clientList)
            {
                RemoveConnection(client);
            }
        }
    }
}
