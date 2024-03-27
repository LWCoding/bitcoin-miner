using System.Collections;
using System.Collections.Generic;

public class OpenCSCommand : BaseCSCommand
{

    public override string GetCommandUsageEx() => "open [filename]";
    public override string GetCommandDescription() =>
@"Opens a created file in the current directory.";
    public override int GetPermissionReq() => 0;

    public override void RunCommand(List<string> args, ref CSInterpreterResponse res)
    {
        if (args.Count < 1)
        {
            res.text = "Must specify file name as first parameter";
            res.status = CSStatus.ERROR;
            return;
        }
        Open(args[0], ref res);
    }

    private void Open(string fileName, ref CSInterpreterResponse res)
    {
        int idx = GameState.CreatedFiles.FindIndex((gf) => gf.FileName == fileName);
        if (idx == -1)
        {
            res.text = "Could not find specified file";
            res.status = CSStatus.ERROR;
            return;
        }
        GameFile foundFile = GameState.CreatedFiles[idx];
        if (foundFile.IsOpen)
        {
            res.text = "File is already open";
            res.status = CSStatus.ERROR;
            return;
        }
        WindowManager.Instance.OpenFileWindow(idx);  // Open the file
        res.text = "Opened file '" + fileName + "' successfully";
        res.status = CSStatus.OK;
    }

}
