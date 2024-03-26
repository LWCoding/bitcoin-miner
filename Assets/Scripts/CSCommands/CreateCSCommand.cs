using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCSCommand : BaseCSCommand
{

    public override string GetCommandUsageEx() => "create [filename]";
    public override string GetCommandDescription() =>
@"Creates a new file in your root directory.";

    public override void RunCommand(List<string> args, ref CSInterpreterResponse res)
    {
        if (args.Count < 1)
        {
            res.text = "Must specify file name as first parameter";
            res.status = CSStatus.ERROR;
            return;
        }
        Create(args[0], ref res);
    }

    private void Create(string fileName, ref CSInterpreterResponse res)
    {
        if (GameFile.IsValidFileName(fileName))
        {
            res.text = "Successfully created file '" + fileName + "'";
            res.status = CSStatus.OK;
            GameState.CreatedFiles.Add(new GameFile(fileName));
        }
        else
        {
            res.text = "Invalid file name specified (invalid characters, too long, or already exists)";
            res.status = CSStatus.ERROR;
        }

    }

}
