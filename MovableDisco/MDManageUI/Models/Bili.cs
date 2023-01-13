using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDManageUI.Models
{
    public class BiliUser
    {
        [Key]
        public long UID { get; set; }
        public string UName { get; set; }
    }
    public class BiliDanmu
    {
        [Key]
        public long UID { get; set; }
        public string UName { get; set; }
        public string Content { get; set; }
    }
    public class LiveRoomOrigLog
    {
        [Key]
        public long ID { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
