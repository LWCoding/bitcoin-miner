using UnityEngine;

public class DiscordController : MonoBehaviour
{
    public long applicationID;
    [Space]
    public string largeImage;
    public string largeText;

    private float time;
    private float _timeSinceLastUpdate;

    public Discord.Discord discord;

    void Start()
    {
        // Log in with the Application ID
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    void Update()
    {
        // Destroy the GameObject if Discord isn't running
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(this);
        }
        // Update the status every now and then.
        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate > 1)
        {
            UpdateStatus();
            _timeSinceLastUpdate = 0;
        }
    }

    void UpdateStatus()
    {
        // Update Status every frame
        try
        {
            string details, currState;
            currState = "Getting rich, quick!";
            details = "Balance: " + GameState.Clicks.ToString("F5") + " BTC";
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = details,
                State = currState,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
                Timestamps =
                {
                    Start = (long)time
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            // If updating the status fails, Destroy the GameObject
            Destroy(this);
        }
    }
}