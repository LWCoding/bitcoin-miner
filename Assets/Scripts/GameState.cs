using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct GameFile
{
    public string FileName;
    public string FileContents;
    public bool IsOpen;
   
    public GameFile(string fileName, string fileContents = "")
    {
        FileName = fileName;
        FileContents = fileContents;
        IsOpen = false;
        // If the file name is bad, this shouldn't be
        // properly added. Set it to garbage data.
        if (!IsValidFileName(fileName))
        {
            FileName = "badfilename";
        }
    }
    public static bool IsValidFileName(string fileName)
    {
        // Require that files ONLY consist of alphabetical/numerical chars
        if (!fileName.All(c => "abcdefghijklmnopqrstuvwxyz1234567890".Contains(c)))
        {
            return false;
        }
        // Require that files are less than or equal to max chars
        if (fileName.Length > GameState.FILENAME_MAX_LENGTH)
        {
            return false;
        }
        // Require that files don't already exist.
        if (GameState.CreatedFiles.Any((gf) => gf.FileName == fileName))
        {
            return false;
        }
        return true;
    }

    // Equals comparisons simply checks against `FileName`, since names are unique.
    public readonly override bool Equals(object obj) => FileName == ((GameFile)obj).FileName;
    public readonly override int GetHashCode() => base.GetHashCode();

}

public static class GameState
{

    private static float _clicks = 0;
    public static float Clicks
    {
        get => _clicks;
        set
        {
            _clicks = value;
            OnChangeBitcoin?.Invoke(value);
        }
    }

    public static Action<float> OnChangeBitcoin = null;

    public static List<GameFile> CreatedFiles = new();
    public const int FILENAME_MAX_LENGTH = 12;

}
