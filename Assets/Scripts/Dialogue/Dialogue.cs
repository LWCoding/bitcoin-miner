using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueEvent
{
    NONE = 0, SHOW_BITCOIN_COUNT = 1, UPGRADE_PERMISSIONS = 2, CREATE_SHOP_FILE = 3
}

[System.Serializable]
public struct DialogueLine
{

    [Tooltip("The delay (in seconds) before sending the message.")]
    public float DelayBefore;
    [TextArea(2, 4)]
    public string Text;
    public string Color;
    public bool SendInstantly;
    public DialogueEvent EventToPlayAfter;

}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Game/Dialogue")]
public class Dialogue : ScriptableObject
{

    public List<DialogueLine> DialogueLines;

}
