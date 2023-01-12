using System.Net;
using System.Net.Sockets;
using UCBSocket.Extension;

namespace UCBSocket
{
    public class SocketConnection : SocketEndPoint
    {
        public SocketConnection(Socket socket) : base(socket)
        {
        }
    }
}
