using System;
using System.Collections;
using System.Collections.Generic;

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

    public static float ClicksPerSecond = 0;

    private static int _commandsRun = 0;
    public static int CommandsRun
    {
        get => _commandsRun;
        set
        {
            _commandsRun = value;
            OnChangeCommandsRun?.Invoke(value);
        }
    }

    private static int _purchasesMade = 0;
    public static int PurchasesMade
    {
        get => _purchasesMade;
        set
        {
            _purchasesMade = value;
            OnPurchasedFromShop?.Invoke(value);
        }
    }

    public static int PermissionCount = 0;  // Dictates what commands the user can call

    public static Action<float> OnChangeBitcoin = null;  // Calls when clicks are set
    public static Action<int> OnChangeCommandsRun = null;  // Calls when commands are run
    public static Action<int> OnPurchasedFromShop = null;  // Calls when purchase is made

    public static Dictionary<PurchaseType, int> Purchases = new();  // Stores state of all purchases

    public static List<GameFile> CreatedFiles = new();  // Empty list to hold created files
    public const int FILENAME_MAX_LENGTH = 12;  // Max name length of any file

}
