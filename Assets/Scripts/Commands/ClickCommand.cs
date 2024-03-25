using System;
using System.Collections;
using System.Collections.Generic;

public class ClickCommand : BaseCommand
{
   
    public override string GetCommandUsageEx() => "mine";
    public override string GetCommandDescription() =>
@"Mines bitcoin. Adds it to your persistent wallet.";

    private Random _random = new();

    public override void RunCommand(List<string> args, ref InterpreterResponse res)
    {
        Click(ref res);
    }

    private void Click(ref InterpreterResponse res)
    {
        float earnedMoney = (float)_random.NextDouble() / 1000;
        GameState.Clicks += earnedMoney;
        res.text = "Mining attempt earned " + earnedMoney.ToString("F5") + " bitcoin";
        res.status = Status.OK;
    }

}
