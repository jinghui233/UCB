using BiliBiliLiveRoom.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiliBiliLiveRoomWPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        public async void Start()
        {
            var liveHandler = new LiveHandler();
            liveHandler.richTextBox = richTextBox1;
            var sroomid = 102;
            var roomid = sroomid;
            //第一个参数是直播间的房间号
            //第二个参数是自己实现的处理器
            //第三个参数是可选的,可以是默认的消息分发器,也可以是自己实现的消息分发器
            LiveRoom room = new LiveRoom(roomid, liveHandler, new MessageDispatcher());
            //等待连接,该方法会反回是否连接成功
            //或者使用room.Connected,该属性会反馈连接状态
            if (!await room.ConnectAsync())
            {
                Console.WriteLine("连接失败");
                return;
            }
            Console.WriteLine("连接成功");
            Console.WriteLine("按回车结束");
            //消息由Dispatcher分发到对应的MessageHandler中对应方法
            try
            {
                await room.ReadMessageLoop();
            }
            catch (Exception e)
            {
                room.Disconnect();
                Start();
            }
        }
    }
}
