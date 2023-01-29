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
        public static TransferredData AudienceEnter(string uName, bool EnterAdd)
        {
            if (EnterAdd)
            {
                return new TransferredData(new NormAction() { ActionName = "AddCharacter", UName = uName });
            }
            return new TransferredData(new NormAction() { ActionName = "AudienceEnter", UName = uName });
        }
        //创建角色，转圈圈，向上下左右移动，换装
        public static TransferredData Interpret(string danmu, string uName)
        {
            danmu = danmu.Trim();
            BaseCommand baseCommand;
            if (danmu == "AddCharacter")
                baseCommand = new NormAction() { ActionName = "AddCharacter", ShowDanmu = false };
            else if (danmu == "删除角色")
                baseCommand = new NormAction() { ActionName = "RemoveCharacter", ShowDanmu = false };
            else if (danmu.Contains("转圈"))
                baseCommand = new NormAction() { ActionName = "MoveCircle", ShowDanmu = true };
            else if (danmu == "换装")
                baseCommand = new NormAction() { ActionName = "ChangePendant", ShowDanmu = true };
            else if (danmu == "甩头")
                baseCommand = new NormAction() { ActionName = "ShakeHead", ShowDanmu = true };
            else if (danmu == "SwitchMusic")
                baseCommand = new NormAction() { ActionName = "SwitchMusic", ShowDanmu = false };
            else if (danmu == "ClrAllPlyrs")
                baseCommand = new NormAction() { ActionName = "ClrAllPlyrs", ShowDanmu = false };
            else if (danmu.Contains("加速"))
            {
                baseCommand = new ChangeSpeed() { dSpeed = Regex.Matches(danmu, @"加速").Count * 2, ShowDanmu = true };
            }
            else if (danmu.Contains("减速"))
            {
                baseCommand = new ChangeSpeed() { dSpeed = -Regex.Matches(danmu, @"减速").Count, ShowDanmu = true };
            }
            else if (Regex.Matches(danmu, @"向[上下左右]").Count > 0 || (danmu.Length == 1 && Regex.Matches(danmu, @"[上下左右]").Count > 0))
            {
                MoveSimple moveSimple = new MoveSimple() { ShowDanmu = true };
                moveSimple.Up = danmu.Contains("上");
                moveSimple.Down = danmu.Contains("下");
                moveSimple.Left = danmu.Contains("左");
                moveSimple.Right = danmu.Contains("右");
                baseCommand = moveSimple;
            }
            else
            {
                baseCommand = new BaseCommand() { UName = uName, OrigDanmu = danmu, ShowDanmu = true };
            }
            baseCommand.OrigDanmu = danmu;
            baseCommand.UName = uName;
            return new TransferredData(baseCommand);
        }
    }
}
