using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScheduler : MonoBehaviour
{

    [Header("Dialogue Assignments")]
    [SerializeField] private Dialogue _firstDialogueWhenCommandsRan;
    [SerializeField] private Dialogue _firstDialogueWhenPurchaseMade;

    private void Start()
    {
        GameState.OnChangeCommandsRun += OnCommandsRun;
        GameState.OnPurchasedFromShop += OnPurchasesMade;
    }

    private void OnCommandsRun(int commandsRun)
    {
        // On five commands, broadcast dialogue.
        if (commandsRun == 5)
        {
            CSDialogueScripter.Instance.InterpretDialogue(_firstDialogueWhenCommandsRan);
            return;
        }
    }

    private void OnPurchasesMade(int purchasesMade)
    {
        // On one purchase, broadcast dialogue.
        if (purchasesMade == 1)
        {
            CSDialogueScripter.Instance.InterpretDialogue(_firstDialogueWhenPurchaseMade);
            return;
        }
    }

}
