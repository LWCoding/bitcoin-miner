using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeleteCommand : BaseCommand
{

    public override string GetCommandUsageEx() => "del [filename]";
    public override string GetCommandDescription() =>
@"Deletes a file from your root directory. File must not be open.";

    public override void RunCommand(List<string> args, ref InterpreterResponse res)
    {
        if (args.Count < 1)
        {
            res.text = "Must specify file name as first parameter";
            res.status = Status.ERROR;
            return;
        }
        Delete(args[0], ref res);
    }

    private void Delete(string fileName, ref InterpreterResponse res)
    {
        int idx = GameState.CreatedFiles.FindIndex((gf) => gf.FileName == fileName);
        if (idx == -1)
        {
            res.text = "Could not find specified file";
            res.status = Status.ERROR;
            return;
        }
        if (GameState.CreatedFiles[idx].IsOpen)
        {
            res.text = "Cannot delete file that is currently open";
            res.status = Status.ERROR;
            return;
        }
        GameState.CreatedFiles.RemoveAt(idx);
        res.text = "Successfully deleted file '" + fileName + "'";
        res.status = Status.OK;
    }

}
