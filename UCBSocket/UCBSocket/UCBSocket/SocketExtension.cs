using System.Text;
namespace UCBSocket.Extension
{
    public static class SocketExtension
    {
        public static void Send(this SocketEndPoint socket, string str)
        {
            socket.Send(Encoding.UTF8.GetBytes(str));
        }
        public static void Send(this SocketEndPoint socket, string str, Encoding encoding)
        {
            socket.Send(encoding.GetBytes(str));
        }
        public static void SendAsJson(this SocketEndPoint socket, object obj)
        {
            socket.Send(Utils.JsonHelper.ToJson(obj));
        }
    }
}
