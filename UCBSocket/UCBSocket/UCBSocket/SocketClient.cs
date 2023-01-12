namespace UCBSocket
{
    public class SocketClient : SocketEndPoint
    {
        private bool autoReconnect;
        private bool menuClose;
        public SocketClient(string ip, int port, bool autoReconnect) : base(ip, port)
        {
            this.autoReconnect = autoReconnect;
            UnexpectedDisconnection += OnUnexpectedDisconnection;
        }
        public override bool Connect()
        {
            menuClose = false;
            return base.Connect();
        }
        public override void DisConnect()
        {
            base.DisConnect();
            menuClose = true;
        }
        protected async void AutoReconnect()
        {
            PrintDebugMessage("自动重连");
            await Task.Run(() =>
            {
                int i = 0;
                PrintDebugMessage($"第{i}此尝试");
                while (!Connect())
                {
                    i++;
                    PrintDebugMessage($"第{i}此尝试");
                    Thread.Sleep(1000);
                }
            });
        }
        private void OnUnexpectedDisconnection(SocketEndPoint socketEndPoint)
        {
            if (autoReconnect && !menuClose)
            {
                AutoReconnect();
            }
        }
    }
}