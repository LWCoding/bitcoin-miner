using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScheduler : MonoBehaviour
{

    [Header("Dialogue Assignments")]
    [SerializeField] private Dialogue _firstDialogueWhenCommandsRan;

    private void Start()
    {
        GameState.OnChangeCommandsRun += OnCommandsTyped;
    }

    private void OnCommandsTyped(int commandsRun)
    {
        // On five commands, broadcast dialogue.
        if (commandsRun == 5)
        {
            CSDialogueScripter.Instance.InterpretDialogue(_firstDialogueWhenCommandsRan);
            return;
        }
    }

}
