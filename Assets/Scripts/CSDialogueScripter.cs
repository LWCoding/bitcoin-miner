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
        Instance = null;
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
            while (currText.Length != next.Text.Length)
            {
                currText = next.Text[..(currText.Length + 1)];
                _text.text = "<color=\"" + next.Color + "\">" + currText + "</color>";
                if (currText.Length != next.Text.Length)
                {
                    _text.text += "|";
                }
                yield return new WaitForSeconds(0.06f);
            }
        } else
        {
            _csTextManager.AnnounceInConsole(next.Text, next.Color);
        }

        if (_dialogueQueue.Count == 0) {
            _csTextManager.ToggleConsoleInput(true);  // Allow user to type after finish
        } else
        {
            StartCoroutine(RenderAllDialogueInQueue());  // Continue rendering if not empty
        }
    }

}
