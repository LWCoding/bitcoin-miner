using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExecuteCSCommand : BaseCSCommand
{

    public override string GetCommandUsageEx() => "run [filename]";
    public override string GetCommandDescription() =>
@"Interprets a file as an executable, running any code inside.";

    private readonly FileInterpreter _fileInterpreter = new();

    public override void RunCommand(List<string> args, ref CSInterpreterResponse res)
    {
        if (args.Count < 1)
        {
            res.text = "Must specify file name as first parameter";
            res.status = CSStatus.ERROR;
            return;
        }
        Run(args[0], ref res);
    }

    private void Run(string fileName, ref CSInterpreterResponse res)
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

        // Run interpreter on the file
        FileInterpreterResponse fileRes = _fileInterpreter.InterpretFile(foundFile.FileContents);

        res.text = (fileRes.status == FileStatus.ERROR) ? fileRes.errorText : string.Join("\n", fileRes.resList); ;
        res.status = (fileRes.status == FileStatus.ERROR) ? CSStatus.ERROR : CSStatus.OK;
    }

}
