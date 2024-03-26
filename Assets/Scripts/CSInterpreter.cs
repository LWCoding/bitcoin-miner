using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CSStatus
{
    OK = 0, ERROR = 1
}

public struct CSInterpreterResponse
{
    public string text;  // Contains text to print to console
    public CSStatus status;  // Contains status of response
}

public class CSInterpreter
{

    /// <summary>
    /// Given a string input `text` to evaluate, tokenizes and
    /// renders the appropriate command.
    /// </summary>
    public CSInterpreterResponse Interpret(string text)
    {
        // Prepare the response variable.
        CSInterpreterResponse res = new();
        BaseCSCommand commandToExec = null;

        // Parse the incoming text.
        text = text.Trim();  // Trim any leading whitespace
        List<string> tokens = new(text.Split(' '));
        string cmd = tokens[0];  // Store the command name
        tokens.RemoveAt(0);  // Remove the command

        // Locate corresponding function based on the command.
        switch (cmd)
        {
            case "help":
                commandToExec = new HelpCSCommand();
                break;
            case "click":
            case "mine":
                commandToExec = new ClickCSCommand();
                break;
            case "create":
                commandToExec = new CreateCSCommand();
                break;
            case "list":
            case "ls":
                commandToExec = new ListCSCommand();
                break;
            case "delete":
            case "del":
                commandToExec = new DeleteCSCommand();
                break;
            case "open":
                commandToExec = new OpenCSCommand();
                break;
            case "run":
                commandToExec = new ExecuteCSCommand();
                break;
            case "cat":
                commandToExec = new CatCSCommand();
                break;
            case "close":
                commandToExec = new CloseCSCommand();
                break;
            default:
                res.text = "Invalid command specified";
                res.status = CSStatus.ERROR;  // If not identified, ERROR
                break;
        }

        // Run the command and return response afterwards.
        commandToExec?.RunCommand(tokens, ref res);

        return res;
    }

}
