using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MDManageUI.Command
{
    public class DanmuInterpreter
    {
        public static TransferredData AudienceEnter(string uName)
        {
            return new TransferredData(new NormAction() { UName = uName, ActionName = "AudienceEnter" });
        }
        //创建角色，转圈圈，向上下左右移动，换装
        public static TransferredData Interpret(string danmu, string uName)
        {
            BaseCommand baseCommand = null;
            danmu = danmu.Trim();
            if (danmu == "创建角色")
                baseCommand = new NormAction() { UName = uName, ActionName = "AddCharacter" };
            else if (danmu == "转圈圈")
                baseCommand = new NormAction() { UName = uName, ActionName = "MoveCircle" };
            else if (danmu == "换装")
                baseCommand = new NormAction() { UName = uName, ActionName = "ChangePendant" };
            else if (Regex.Matches(danmu, @"向\S+移动").Count > 0)
            {
                MoveSimple moveSimple = new MoveSimple();
                moveSimple.Up = danmu.Contains("上");
                moveSimple.Down = danmu.Contains("下");
                moveSimple.Left = danmu.Contains("左");
                moveSimple.Right = danmu.Contains("右");
                baseCommand = moveSimple;
            }
            else
            {
                baseCommand = new BaseCommand() { UName = uName, OrigDanmu = danmu };
            }
            return new TransferredData(baseCommand);
        }
    }
}
