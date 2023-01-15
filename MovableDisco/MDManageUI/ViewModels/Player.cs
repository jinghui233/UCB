using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MDManageUI
{
    public class Player : INotifyPropertyChanged
    {
        public long UID { get; set; }
        public string UName { get; set; }
        public DateTime LastOperationTime { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class PlayerManager
    {
    }
    public partial class PlayerManager
    {
        public int MaxPlayerNumber = 100;
        public SortedList<DateTime, Player> PlayerList { get; set; }
        public Dictionary<long, Player> PlayerDictionary { get; set; }
        public ObservableCollection<Player> Players;
        public PlayerManager()
        {
            PlayerList = new SortedList<DateTime, Player>();
            PlayerDictionary = new Dictionary<long, Player>();
            Players = new ObservableCollection<Player>();
        }
        public Player RemoveGet(long uid)
        {
            if (PlayerDictionary.ContainsKey(uid))
            {
                Player player = PlayerDictionary[uid];
                Remove(player);
                return player;
            }
            return null;
        }
        public Player LimitCount()
        {
            if (Players.Count > MaxPlayerNumber)
            {
                Player removedPlayer = PlayerList.First().Value;
                Remove(removedPlayer);
                return removedPlayer;
            }
            return null;
        }
        public void Add(Player player)
        {
            if (!PlayerDictionary.ContainsKey(player.UID))
            {
                PlayerDictionary.Add(player.UID, player);
                PlayerList.Add(player.LastOperationTime, player);
                Players.Add(player);
            }
        }
        public void Remove(Player player)
        {
            if (PlayerDictionary.ContainsKey(player.UID))
            {
                PlayerDictionary.Remove(player.UID);
                PlayerList.Remove(player.LastOperationTime);
                Players.Remove(player);
            }
        }
    }
}
