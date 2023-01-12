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
        public string FuncName { get; set; }
        public BaseCommand FuncData { get; set; }
        public TransferredData()
        {

        }
        public TransferredData(BaseCommand baseCommand)
        {
            FuncName = baseCommand.GetType().Name;
            FuncData = baseCommand;
        }
    }
    public class BaseCommand
    {
        public int aaa { get; set; }
        public string ToJson()
        {
            return JsonHelper.ToJson(this);
        }
    }
    public class MessageData : BaseCommand
    {
        public string message { get; set; }
    }
    public class AudienceEnter : BaseCommand
    {
        public string UName { get; set; }
    }

    public class AddCharacter : BaseCommand
    {
        public string UName { get; set; }
    }
    public class RemoveCharacter : BaseCommand
    {
        public string UName { get; set; }
    }
    public class MoveCircle : BaseCommand
    {
        public string UName { get; set; }
    }
    public class MoveSimple : BaseCommand
    {
        public string UName { get; set; }
    }
}
