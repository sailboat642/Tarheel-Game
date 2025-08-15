using UnityEngine;
using UnityEditor;
using System;

public class DialogueNode: IGraphNode

{
    private Rect _rect;
    public Rect rect 
    { 
        get => _rect; 
        set => _rect = value; 
    }
    private bool _isSelected;
    public bool isSelected
    {
        get => _isSelected;
        set => _isSelected = value;
    }
    public GenericGraphEditor<DialogueNode> EditorRef { get; set; }
    public bool isDragged;
    public DialogueNode connectedNode;

    public string title;
    public string speakerName;
    public string dialogueText;

    public Action<DialogueNode> OnClickConnect;  

    public DialogueNode(Vector2 position, float width = 250, float height = 170)
    {
        rect = new Rect(position.x, position.y, width, height);
        title = "Dialogue Node";
        dialogueText = "Enter dialogue here...";
        speakerName = "Speaker";
    }

    public void Drag(Vector2 delta)
    {
        var r = rect;
        r.position += delta;
        rect = r;

    }

    public void Draw()
    {
        GUILayout.BeginArea(rect, GUI.skin.window);
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Speaker:");
        speakerName = EditorGUILayout.TextField(speakerName);

        EditorGUILayout.LabelField("Dialogue:");
        dialogueText = dialogueText = EditorGUILayout.TextArea(dialogueText, GUILayout.ExpandHeight(true));

        if (GUILayout.Button("Connect"))
        {
            OnClickConnect?.Invoke(this);
        }

        GUILayout.EndArea();


        // Measure how tall the text area should be
        float textHeight = EditorStyles.textArea.CalcHeight(new GUIContent(dialogueText), rect.width - 20);
        // Base height: title + speaker + dialogue + button + padding
        float baseHeight = 150f;  // You can tweak this as needed
        // Set rect height to fit the text + base height
        var r = rect;
        r.height = baseHeight + textHeight;
        rect = r;

        if (isSelected)
        {
            // Make a slightly bigger rect for the border
            Rect borderRect = new Rect(rect.x - 2, rect.y - 2, rect.width + 4, rect.height + 4);

            Handles.BeginGUI();
            Handles.color = Color.white;
            Handles.DrawSolidRectangleWithOutline(borderRect, Color.clear, Color.white);
            Handles.EndGUI();
        }

    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;  
                        EditorRef?.SetSelectedNode(this); 
                    }
                }

                
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Delete"), false, () => DialogueNodeEditor.OnRequestNodeDelete(this));
        genericMenu.ShowAsContext();
    }
}
