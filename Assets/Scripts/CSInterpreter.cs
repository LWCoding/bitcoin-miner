using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    OK = 0, ERROR = 1
}

public struct InterpreterResponse
{
    public string text;  // Contains text to print to console
    public Status status;  // Contains status of response
}

public class CSInterpreter
{

    /// <summary>
    /// Given a string input `text` to evaluate, tokenizes and
    /// renders the appropriate command.
    /// </summary>
    public InterpreterResponse Interpret(string text)
    {
        // Prepare the response variable.
        InterpreterResponse res = new();
        BaseCommand commandToExec = null;

        // Parse the incoming text.
        text = text.Trim();  // Trim any leading whitespace
        List<string> tokens = new(text.Split(' '));
        string cmd = tokens[0];  // Store the command name
        tokens.RemoveAt(0);  // Remove the command

        // Locate corresponding function based on the command.
        switch (cmd)
        {
            case "help":
                commandToExec = new HelpCommand();
                break;
            case "click":
            case "mine":
                commandToExec = new ClickCommand();
                break;
            case "create":
                commandToExec = new CreateCommand();
                break;
            case "list":
            case "ls":
                commandToExec = new ListCommand();
                break;
            case "delete":
            case "del":
            case "close":
                commandToExec = new DeleteCommand();
                break;
            case "open":
                commandToExec = new OpenCommand();
                break;
            default:
                res.text = "Invalid command specified";
                res.status = Status.ERROR;  // If not identified, ERROR
                break;
        }

        // Run the command and return response afterwards.
        commandToExec?.RunCommand(tokens, ref res);

        return res;
    }

}
