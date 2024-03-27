using System;
using System.Collections;
using System.Collections.Generic;

public class ClickCSCommand : BaseCSCommand
{
   
    public override string GetCommandUsageEx() => "mine";
    public override string GetCommandDescription() =>
@"Mines bitcoin. Adds it to your persistent wallet.";
    public override int GetPermissionReq() => 0;

    private readonly Random _random = new();

    public override void RunCommand(List<string> args, ref CSInterpreterResponse res)
    {
        Click(ref res);
    }

    private void Click(ref CSInterpreterResponse res)
    {
        float earnedMoney = (float)_random.NextDouble() / 1000;
        GameState.Clicks += earnedMoney;
        res.text = "Mining attempt earned " + earnedMoney.ToString("F5") + " BTC";
        res.status = CSStatus.OK;
    }

}
