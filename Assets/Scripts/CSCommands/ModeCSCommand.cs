using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeCSCommand : BaseCSCommand
{

    public override string GetCommandUsageEx() => "mode";
    public override string GetCommandDescription() =>
@"Toggles between default and mining mode. Mining mode is 0.01x of your `mine` efficiency.";
    public override int GetPermissionReq() => 1;

    public override void RunCommand(List<string> args, ref CSInterpreterResponse res)
    {
        SetMode(ref res);
    }

    private void SetMode(ref CSInterpreterResponse res)
    {
        GameState.CurrentMode = Mode.MINING;
        ClickCSCommand.ClickMultiplier *= 0.01f;
        res.status = CSStatus.OK;
        res.text = "Switched mode to " + GameState.CurrentMode + ". Press Z to exit";
    }

}
