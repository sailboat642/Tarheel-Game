using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class DialogueNodeEditor : GenericGraphEditor<DialogueNode>
{
    private DialogueNode startNode;

    [MenuItem("Window/Dialogue Node Editor")]
    private static void ShowWindow()
    {
        DialogueNodeEditor window = GetWindow<DialogueNodeEditor>();
        window.titleContent = new GUIContent("Dialogue Editor");
    }

    protected override string WindowTitle => "Dialogue Editor";

    protected override void OnGUI()
    {
        // draw everything from the base (grid, nodes, connections, zoom, pan, events…)
        base.OnGUI();

        // now draw our mini-toolbar at the top
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("Save Dialogue…", EditorStyles.toolbarButton))
            ExportToAsset();
        GUILayout.FlexibleSpace();
        if (startNode != null)
            GUILayout.Label($"Start → \"{startNode.speakerName}: {startNode.dialogueText.Split('\n')[0]}\"", EditorStyles.miniLabel);
        GUILayout.EndHorizontal();
    }

    protected override void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Dialogue Node"), false, () => OnClickAddNode(mousePosition));

        var node = GetNodeAt(mousePosition);
        if (node != null)
        {
            menu.AddItem(new GUIContent("Set As Start Node"), node == startNode, () => startNode = node);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Delete"), false, () => OnRequestNodeDelete(node));
        }

        menu.ShowAsContext();
    }

    private DialogueNode GetNodeAt(Vector2 mousePosition)
    {
        return nodes.Find(n => n.rect.Contains(mousePosition));
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        var node = new DialogueNode(mousePosition);
        node.EditorRef = this; 
        nodes.Add(node);
    }


    public static void OnRequestNodeDelete(DialogueNode node)
    {
        var editor = GetWindow<DialogueNodeEditor>();
        editor.nodes.Remove(node);
        foreach (var n in editor.nodes)
        {
            if (n.connectedNode == node)
            {
                n.connectedNode = null;
            }
        }
    }

    private void ExportToAsset()
    {
        if (startNode == null)
        {
            Debug.LogError("You must set a Start Node before saving.");
            return;
        }

        // 1) Build a linear traversal from startNode → ... → null
        var ordered    = new List<DialogueNode>();
        var visited    = new HashSet<DialogueNode>();
        var current    = startNode;
        while (current != null && !visited.Contains(current))
        {
            visited.Add(current);
            ordered.Add(current);
            current = current.connectedNode;
        }

        // 2) Ask the user where to save
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Dialogue Data",
            "NewDialogueData",
            "asset",
            "Choose a save location"
        );
        if (string.IsNullOrEmpty(path)) return;

        // 3) Create & populate the asset
        var asset = ScriptableObject.CreateInstance<DialogueData>();
        for (int i = 0; i < ordered.Count; i++)
        {
            var node = ordered[i];
            var entry = new DialogueData.Entry
            {
                speaker   = node.speakerName,
                text      = node.dialogueText,
                nextIndex = (i + 1 < ordered.Count) ? i + 1 : -1
            };
            asset.entries.Add(entry);
        }

        // 4) Save it
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }


}
