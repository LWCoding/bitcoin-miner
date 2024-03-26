using System.Collections;
using System.Collections.Generic;

public class CloseCSCommand : BaseCSCommand
{

    public override string GetCommandUsageEx() => "close [filename]";
    public override string GetCommandDescription() =>
@"Closes the window of a currently opened file.";

    public override void RunCommand(List<string> args, ref CSInterpreterResponse res)
    {
        if (args.Count < 1)
        {
            res.text = "Must specify file name as first parameter";
            res.status = CSStatus.ERROR;
            return;
        }
        Close(args[0], ref res);
    }

    private void Close(string fileName, ref CSInterpreterResponse res)
    {
        int idx = GameState.CreatedFiles.FindIndex((gf) => gf.FileName == fileName);
        if (idx == -1)
        {
            res.text = "Could not find specified file";
            res.status = CSStatus.ERROR;
            return;
        }
        GameFile foundFile = GameState.CreatedFiles[idx];
        if (!foundFile.IsOpen)
        {
            res.text = "File is not open";
            res.status = CSStatus.ERROR;
            return;
        }
        WindowManager.Instance.CloseFileWindow(idx);  // Close the file
        res.text = "Closed file '" + fileName + "' successfully";
        res.status = CSStatus.OK;
    }

}
