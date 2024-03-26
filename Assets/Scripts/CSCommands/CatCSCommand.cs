using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCSCommand : BaseCSCommand
{

    public override string GetCommandUsageEx() => "cat [filename]";
    public override string GetCommandDescription() =>
@"Prints out all specified file contents to the console.";

    public override void RunCommand(List<string> args, ref CSInterpreterResponse res)
    {
        if (args.Count < 1)
        {
            res.text = "Must specify file name as first parameter";
            res.status = CSStatus.ERROR;
            return;
        }
        Cat(args[0], ref res);
    }

    private void Cat(string fileName, ref CSInterpreterResponse res)
    {
        int idx = GameState.CreatedFiles.FindIndex((gf) => gf.FileName == fileName);
        if (idx == -1)
        {
            res.text = "Could not find specified file";
            res.status = CSStatus.ERROR;
            return;
        }
        WindowManager.Instance.SaveAllFileContents();  // Save all windows' contents
        GameFile foundFile = GameState.CreatedFiles[idx];
        res.text = foundFile.FileContents;
        res.status = CSStatus.OK;
        res.text = res.text == "" ? "[No file output]" : res.text;
    }

}
