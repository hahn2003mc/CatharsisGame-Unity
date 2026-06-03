using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public Sprite portrait;
    public string text;
}

[CreateAssetMenu(menuName = "Dialogue/Conversation")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] lines;
}