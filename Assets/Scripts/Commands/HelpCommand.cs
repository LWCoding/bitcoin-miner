using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

public class HelpCommand : BaseCommand
{

    public override string GetCommandUsageEx() => "help";
    public override string GetCommandDescription() =>
@"Displays this help desk.";

    public override void RunCommand(List<string> args, ref InterpreterResponse res)
    {
        Help(ref res);
    }

    private void Help(ref InterpreterResponse res)
    {
        //res.text = HelpString;
        res.status = Status.OK;
        res.text = GetAllCommands();
    }

    public string GetAllCommands()
    {
        // Get all types that are subclass of BaseCommand and can be instantiated
        var commandTypes = Assembly
            .GetAssembly(typeof(BaseCommand)) // Adjust this if your BaseCommand is in a different assembly
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseCommand)) && !t.IsAbstract);

        string finalStr = "";
        int i = 0;

        foreach (Type type in commandTypes)
        {
            // Create an instance of the command type
            BaseCommand commandInstance = (BaseCommand)Activator.CreateInstance(type);

            // Print the command's description
            finalStr += ">>> " + commandInstance.GetCommandUsageEx() + "\n<color=#949494>" + commandInstance.GetCommandDescription() + "</color>";

            // Add line breaks to all except the last command
            if (++i != commandTypes.Count())
            {
                finalStr += '\n';
            }
        }

        return finalStr;
    }

}
