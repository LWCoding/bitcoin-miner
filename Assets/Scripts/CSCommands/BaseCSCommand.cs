using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCSCommand
{

    public abstract string GetCommandUsageEx();  // Ex: help [arg1] [arg2]
    public abstract string GetCommandDescription();

    /// <summary>
    /// This command should run the command dictated by the
    /// class. (e.g., HelpCommand) with the specified arguments
    /// `args`, and correctly modify the response `res`.
    /// </summary>
    /// <param name="args">Arguments to pass into the function.</param>
    /// <param name="res">Response, passed in by reference, to modify.</param>
    public abstract void RunCommand(List<string> args, ref CSInterpreterResponse res);

}
