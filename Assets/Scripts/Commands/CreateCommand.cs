using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCommand : BaseCommand
{

    public override string GetCommandUsageEx() => "create [filename]";
    public override string GetCommandDescription() =>
@"Creates a new file in your root directory.";

    public override void RunCommand(List<string> args, ref InterpreterResponse res)
    {
        if (args.Count < 1)
        {
            res.text = "Must specify file name as first parameter";
            res.status = Status.ERROR;
            return;
        }
        Create(args[0], ref res);
    }

    private void Create(string fileName, ref InterpreterResponse res)
    {
        if (GameFile.IsValidFileName(fileName))
        {
            res.text = "Successfully created file '" + fileName + "'";
            res.status = Status.OK;
            GameState.CreatedFiles.Add(new GameFile(fileName));
        }
        else
        {
            res.text = "Invalid file name specified (invalid characters, too long, or already exists)";
            res.status = Status.ERROR;
        }

    }

}
