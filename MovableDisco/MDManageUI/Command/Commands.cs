using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace MDManageUI
{
    public class TransferredData
    {
        public string TypeName { get; set; }
        public string CommandData { get; set; }
        public TransferredData() { }
        public TransferredData(BaseCommand baseCommand)
        {
            TypeName = baseCommand.GetType().Name;
            CommandData = baseCommand.ToJson();
        }
        public static TransferredData FromJson(byte[] jsonData)
        {
            return JsonHelper.JsonTo<TransferredData>(jsonData);
        }
        public static TransferredData FromJson(string jsonData)
        {
            return JsonHelper.JsonTo<TransferredData>(jsonData);
        }
        public T GetCommand<T>() where T : BaseCommand
        {
            return JsonHelper.JsonTo<T>(CommandData);
        }
    }
    public class BaseCommand
    {
        public string UName { get; set; }
        public string ToJson()
        {
            return JsonHelper.ToJson(this);
        }
    }
    public class NormAction : BaseCommand
    {
        public string ActionName { get; set; }
    }
    public class MoveSimple : BaseCommand
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }
        public bool Down { get; set; }
    }
}
