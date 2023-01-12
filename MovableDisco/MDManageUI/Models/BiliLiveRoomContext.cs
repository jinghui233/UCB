using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MDManageUI.Models
{
    public class BiliLiveRoomContext : DbContext
    {
        public DbSet<BiliUser> biliUsers { get; set; }
        public DbSet<BiliDanmu> biliDanmus { get; set; }
        public DbSet<LiveRoomOrigLog> liveRoomOrigLogs { get; set; }
        public string DbPath { get; }
        public BiliLiveRoomContext()
        {
            var path = Environment.CurrentDirectory;
            DbPath = System.IO.Path.Join(path, "BiliLiveRoomDB.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
