using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FileStatus
{
    OK = 0, ERROR = 1
}

public struct FileInterpreterResponse
{
    public List<string> resList;  // Contains response strings from indiv. function lines
    public FileStatus status;  // Contains status of response
    public string errorText;  // Contains error message if error exists
}

public class FileInterpreter
{

    static bool isInRunCommand = false;
    static int currRecursion = 0;
    private const int RECURSIVE_DEPTH_LIMIT = 10;  // Max number of recursive calls allowed
    
    private readonly CSInterpreter _consoleInterpreter = new();

    /// <summary>
    /// Given a string `text` separated by '\n' characters,
    /// splits the string into multiple lines and runs the
    /// `InterpretLine` function on each line.
    /// </summary>
    /// <param name="text"></param>
    public FileInterpreterResponse InterpretFile(string text)
    {
        if (!isInRunCommand)
        {
            currRecursion = 0;  // Reset current recursion count if not running within `run`
        }
        isInRunCommand = true;

        // Prepare the response variable.
        FileInterpreterResponse res = new()
        {
            status = FileStatus.OK, 
            resList = new()
        };

        // Parse text into multiple commands.
        text = text.Trim();  // Trim any leading whitespace
        List<string> lines = new(text.Split('\n'));

        for (int i = 0; i < lines.Count; i++)
        {
            // Stop if we've exceeded the recursion limit
            if (++currRecursion > RECURSIVE_DEPTH_LIMIT)
            {
                res.status = FileStatus.ERROR;
                res.errorText = "Maximum recursion depth reached";
                break;
            }
            // Parse the current line; if it errors, stop
            string line = lines[i];
            CSInterpreterResponse lineRes = InterpretLine(line);
            if (lineRes.status == CSStatus.ERROR)
            {
                res.status = FileStatus.ERROR;
                res.errorText = "Error on line " + (i + 1).ToString() + ": " + lineRes.text;
                break;
            }
            res.resList.Add(lineRes.text);
        }

        isInRunCommand = false;

        return res;
    }

    /// <summary>
    /// Given a string input `text` to evaluate, tokenizes and
    /// renders the appropriate command.
    /// </summary>
    public CSInterpreterResponse InterpretLine(string text)
    {
        return _consoleInterpreter.Interpret(text);
    }

}
