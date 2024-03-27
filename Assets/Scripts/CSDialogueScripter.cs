using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CSDialogueScripter : MonoBehaviour
{

    public static CSDialogueScripter Instance;

    [Header("Object Assignments")]
    [SerializeField] private CSTextManager _csTextManager;
    [Header("Dialogue Assignment")]
    [SerializeField] private Dialogue _dialogueToPlayAtStart;

    private Queue<DialogueLine> _dialogueQueue;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        InterpretDialogue(_dialogueToPlayAtStart);
    }

    public void InterpretDialogue(Dialogue d)
    {
        if (d == null) { return; }
        _csTextManager.ToggleConsoleInput(false);  // Disallow user from typing
        _dialogueQueue = new Queue<DialogueLine>(d.DialogueLines);
        StartCoroutine(RenderAllDialogueInQueue());
    }

    // Render all the dialogue in `_dialogueQueue` until it is empty.
    private IEnumerator RenderAllDialogueInQueue()
    {
        DialogueLine next = _dialogueQueue.Dequeue();
        yield return new WaitForSeconds(next.DelayBefore);

        if (!next.SendInstantly)
        {
            TextMeshProUGUI _text = _csTextManager.AnnounceInConsole("|");

            // Procedurally display the text
            string currText = "";
            string renderedText = "";
            while (renderedText.Length != next.Text.Length)
            {
                char nextChar = next.Text[renderedText.Length];
                renderedText += nextChar;
                if (nextChar == '<')
                {
                    // For backspace char, remove a character
                    currText = currText[..(currText.Length - 1)];
                    yield return new WaitForSeconds(0.06f);  // Wait a bit too so it's natural
                } else
                {
                    // Or else, add the character
                    currText += nextChar;
                }
                _text.text = "<color=\"" + next.Color + "\">" + currText + "</color>";
                if (renderedText.Length != next.Text.Length)
                {
                    _text.text += "|";
                }
                // For punctuation, wait slightly longer
                if (nextChar == '.' || nextChar == '!')
                {
                    yield return new WaitForSeconds(0.2f);
                }
                yield return new WaitForSeconds(0.06f);
            }
        } else
        {
            _csTextManager.AnnounceInConsole(next.Text, next.Color);
        }

        if (next.EventToPlayAfter != DialogueEvent.NONE)
        {
            switch (next.EventToPlayAfter)
            {
                case DialogueEvent.SHOW_BITCOIN_COUNT:
                    CSMoneyUpdater.Instance.ToggleTextVisibility(true);
                    break;
                case DialogueEvent.UPGRADE_PERMISSIONS:
                    GameState.PermissionCount++;
                    break;
                case DialogueEvent.CREATE_SHOP_FILE:
                    GameFile shopFile = new GameFile("bt.shop", "", false);
                    GameState.CreatedFiles.Add(shopFile);
                    break;
            }
        }

        if (_dialogueQueue.Count == 0) {
            _csTextManager.ToggleConsoleInput(true);  // Allow user to type after finish
        } else
        {
            StartCoroutine(RenderAllDialogueInQueue());  // Continue rendering if not empty
        }
    }

}
