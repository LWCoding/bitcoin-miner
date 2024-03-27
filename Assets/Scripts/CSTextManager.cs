using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CSTextManager : MonoBehaviour
{

    [Header("Prefab Assignments")]
    [SerializeField] private GameObject _consoleTextPrefab;
    [SerializeField] private GameObject _adminConsoleTextPrefab;
    [Header("Object Assignments")]
    [SerializeField] private TMP_InputField _consoleInput;
    [SerializeField] private Transform _consoleTextParent;

    private readonly CSInterpreter _interpreter = new();

    private void Awake()
    {
        _consoleInput.onSubmit.AddListener((s) => SubmitTextToConsole(s));
        _consoleInput.ActivateInputField();
        _consoleInput.Select();
    }

    //private void Start()
    //{
    //    AnnounceInConsole("Welcome, user. Type <b>help</b> for assistance.", "green");
    //}

    /// <summary>
    /// Toggles whether the user can type something into the console.
    /// </summary>
    public void ToggleConsoleInput(bool inputEnabled)
    {
        _consoleInput.interactable = inputEnabled;
        // If disabling, make sure the user isn't selecting the input field
        if (!inputEnabled)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// Sends a non-user generated message in the console. Color
    /// can be optionally adjusted.
    /// </summary>
    public TextMeshProUGUI AnnounceInConsole(string text, string color = "white")
    {
        GameObject textObj = Instantiate(_adminConsoleTextPrefab, _consoleTextParent);
        string textToSend = "<color=\"" + color + "\">" + text + "</color>";
        textObj.GetComponent<TextMeshProUGUI>().text = textToSend;
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
        _consoleInput.ActivateInputField();
        _consoleInput.Select();  // Reselect the console input
        GameObject textObj = Instantiate(_consoleTextPrefab, _consoleTextParent);
        textObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cmd;  // Child object has actual text
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

}
