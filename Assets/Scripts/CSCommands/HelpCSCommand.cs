using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

public class HelpCSCommand : BaseCSCommand
{

    public override string GetCommandUsageEx() => "help";
    public override string GetCommandDescription() =>
@"Displays this help desk.";

    public override void RunCommand(List<string> args, ref CSInterpreterResponse res)
    {
        Help(ref res);
    }

    private void Help(ref CSInterpreterResponse res)
    {
        //res.text = HelpString;
        res.status = CSStatus.OK;
        res.text = GetAllCommands();
    }

    public string GetAllCommands()
    {
        // Get all types that are subclass of BaseCSCommand and can be instantiated
        var commandTypes = Assembly
            .GetAssembly(typeof(BaseCSCommand)) // Adjust this if your BaseCSCommand is in a different assembly
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseCSCommand)) && !t.IsAbstract);

        string finalStr = "";
        int i = 0;

        foreach (Type type in commandTypes)
        {
            // Create an instance of the command type
            BaseCSCommand commandInstance = (BaseCSCommand)Activator.CreateInstance(type);

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
