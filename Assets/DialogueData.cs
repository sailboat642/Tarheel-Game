using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public string speaker;
        [TextArea(2,5)]
        public string text;
        public int nextIndex;    // index of the next entry, or -1 for end
    }

    public List<Entry> entries = new List<Entry>();
}
