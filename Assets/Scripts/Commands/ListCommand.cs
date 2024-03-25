using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListCommand : BaseCommand
{

    public override string GetCommandUsageEx() => "ls";
    public override string GetCommandDescription() =>
@"Lists all of the files in the current directory.";

    public override void RunCommand(List<string> args, ref InterpreterResponse res)
    {
        List(ref res);
    }

    private void List(ref InterpreterResponse res)
    {
        string resText = "";

        // Loop over all files and append to resText.
        foreach (GameFile gf in GameState.CreatedFiles)
        {
            resText += gf.FileName;
            resText += new string(' ', (GameState.FILENAME_MAX_LENGTH + 2) - gf.FileName.Length);
        }

        res.text = (resText == "") ? "No files in current directory" : resText;
        res.status = Status.OK;
    }

}
