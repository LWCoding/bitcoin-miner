using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public struct ConsoleLine
{
    public PoolableType Type;
    public GameObject TextObject;
    public ConsoleLine(GameObject to, PoolableType t)
    {
        Type = t;
        TextObject = to;
    }
}

public class CSTextManager : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private TMP_InputField _consoleInput;
    [SerializeField] private Transform _consoleTextParent;

    private readonly CSInterpreter _interpreter = new();

    /// <summary>
    /// Stores a cached list of BOTH admin text prefabs AND user
    /// text prefabs. When it reaches a certain amount, will prune
    /// out old entries for performance reasons.
    /// </summary>
    private readonly Queue<ConsoleLine> _consoleLineObjects = new();
    private const int MAXIMUM_ALLOWED_LINES = 40;

    private void TryFocusInput()
    {
        if (_consoleInput.interactable)
        {
            _consoleInput.ActivateInputField();
            _consoleInput.Select();  // Reselect the console input
        }
    }

    private void Awake()
    {
        _consoleInput.onSubmit.AddListener((s) => SubmitTextToConsole(s));
        _consoleInput.onValueChanged.AddListener((s) => OnTextTyped(s));
        TryFocusInput();
    }

    /// <summary>
    /// Toggles whether the user can type something into the console.
    /// </summary>
    public void ToggleConsoleInput(bool inputEnabled)
    {
        _consoleInput.interactable = inputEnabled;
        // If disabling, make sure the user isn't selecting the input field
        if (!inputEnabled)
        {
            StartCoroutine(UnselectSelectedGameObject());
        } else
        {
            // If enabling, focus the player's input.
            TryFocusInput();
        }
    }

    /// <summary>
    /// Sends a non-user generated message in the console. Color
    /// can be optionally adjusted.
    /// </summary>
    public TextMeshProUGUI AnnounceInConsole(string text, string color = "white")
    {
        GameObject textObj = ObjectFactory.Instance.GetPooledObject(PoolableType.CS_ADMIN_TEXT, _consoleTextParent); 
        string textToSend = "<color=\"" + color + "\">" + text + "</color>";
        textObj.GetComponent<TextMeshProUGUI>().text = textToSend;
        RegisterConsoleLineObject(textObj, PoolableType.CS_ADMIN_TEXT);
        return textObj.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Takes a string input `cmd` and renders it in the console.
    /// Will be properly tokenized and evaluated by `CSInterpreter`.
    /// </summary>
    public void SubmitTextToConsole(string cmd)
    {
        if (_consoleInput.text == "") return;
        _consoleInput.text = "";  // Reset text
        TryFocusInput();
        GameObject textObj = ObjectFactory.Instance.GetPooledObject(PoolableType.CS_USER_TEXT, _consoleTextParent);
        textObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cmd;  // Child object has actual text
        RegisterConsoleLineObject(textObj, PoolableType.CS_USER_TEXT);
        CSInterpreterResponse res = _interpreter.Interpret(cmd);
        // If we have an error, send that message out!
        if (res.status == CSStatus.ERROR)
        {
            AnnounceInConsole("Error when running command", "red");
        }
        if (res.text != "")
        {
            AnnounceInConsole(res.text, (res.status == CSStatus.OK) ? "green" : "red");
        }
    }

    private void OnTextTyped(string newStr)
    {
        switch (GameState.CurrentMode)
        {
            case Mode.DEFAULT:
                break;
            case Mode.MINING:
                // If we're mining, don't render any input
                _consoleInput.text = "";
                // If the key should escape mining mode, leave mining mode.
                if (newStr.ToLower() == "z")
                {
                    GameState.CurrentMode = Mode.DEFAULT;
                    AnnounceInConsole("Exit requested. Returning to default state", "green");
                    break;
                }
                // Instead, add to the user's money value
                CSInterpreterResponse res = new();
                new ClickCSCommand().RunCommand(null, ref res);
                // Animate some money
                Vector2 textSpawnpoint = _consoleInput.transform.position + new Vector3(0.3f, 0.3f);
                CSMoneyUpdater.Instance.AnimateBTCFromPositionToText(textSpawnpoint, 1);
                break;
        }
    }

    private IEnumerator UnselectSelectedGameObject()
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void RegisterConsoleLineObject(GameObject textObj, PoolableType type)
    {
        _consoleLineObjects.Enqueue(new ConsoleLine(textObj, type));
        if (_consoleLineObjects.Count > MAXIMUM_ALLOWED_LINES)
        {
            ConsoleLine lineToRemove = _consoleLineObjects.Dequeue();
            ObjectFactory.Instance.ReturnObjectToPool(lineToRemove.TextObject, lineToRemove.Type);
        }
    }

}
