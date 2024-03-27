using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueLine
{

    [Tooltip("The delay (in seconds) before sending the message.")]
    public float DelayBefore;
    public string Text;
    public string Color;
    public bool SendInstantly;

}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Game/Dialogue")]
public class Dialogue : ScriptableObject
{

    public List<DialogueLine> DialogueLines;

}
