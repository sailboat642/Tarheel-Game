using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class GenericGraphEditor<T> : EditorWindow where T : IGraphNode
{
    protected float zoom = 1.0f;
    private const float zoomMin = 0.2f;
    private const float zoomMax = 2.0f;
    private Vector2 panOffset = Vector2.zero;


    protected List<T> nodes = new List<T>();
    private Vector2 drag;
    protected Vector2 offset;
    private T selectedNodeForConnection = default;
    protected T selectedNode = default;

    protected abstract string WindowTitle { get; }

    protected virtual void OnEnable()
    {
        titleContent = new GUIContent(WindowTitle);
    }

    protected virtual void OnGUI()
    {
        //HandleZoomEvents(Event.current);
        // Zoom block
        //Rect zoomArea = new Rect(0, 21, position.width, position.height-21);
        //EditorZoomArea.Begin(zoom, zoomArea);
        // Your normal drawing:
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);
        DrawConnections();
        DrawNodes();

        //EditorZoomArea.End();


        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        // DrawZoomLabel();
        if (GUI.changed) Repaint();
    }

    private void DrawZoomLabel()
    {
        float padding = 10f;
        string zoomText = $"Zoom: {(zoom * 100):F0}%";

        Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(zoomText));
        Rect labelRect = new Rect(position.width - textSize.x - padding, padding, textSize.x, textSize.y);

        GUI.Label(labelRect, zoomText);
    }


    protected virtual void DrawNodes()
    {
        foreach (var node in nodes)
        {
            if (node is DialogueNode dialogueNode)
            {
                dialogueNode.OnClickConnect = OnClickConnect;
            }

            node.Draw();
        }
    }

    public void SetSelectedNode(T node)
    {
        foreach (var n in nodes)
        {
            n.isSelected = false;
        }

        selectedNode = node;
        node.isSelected = true;
    }

    private void HandleZoomEvents(Event e)
    {
        if (e.type == EventType.ScrollWheel)
        {
            float old = zoom;
            zoom = Mathf.Clamp(zoom - e.delta.y * 0.05f, zoomMin, zoomMax);

            // optional: zoom towards mouse
            Vector2 m = e.mousePosition;
            offset += (m - offset) * (1 - (zoom / old));

            e.Use();
        }
    }

    protected virtual void ProcessNodeEvents(Event e)
    {
        for (int i = nodes.Count - 1; i >= 0; i--)
        {
            bool guiChanged = nodes[i].ProcessEvents(e);

            if (guiChanged)
                GUI.changed = true;
        }
    }

    protected virtual void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    ShowContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    protected abstract void ShowContextMenu(Vector2 mousePosition);

    protected virtual void OnDrag(Vector2 delta)
    {
        drag = delta;

        foreach (var node in nodes)
        {
            var r = node.rect;
            r.position += delta;
            node.rect = r;
        }

        GUI.changed = true;
    }

    protected virtual void DrawConnections()
    {
        foreach (var node in nodes)
        {
            if (node is DialogueNode dialogueNode && dialogueNode.connectedNode != null)
            {
                DrawNodeConnection(dialogueNode.rect, dialogueNode.connectedNode.rect);
            }
        }
    }

    protected void DrawNodeConnection(Rect fromRect, Rect toRect)
    {
        Vector3 startPos = new Vector3(fromRect.x + fromRect.width, fromRect.y + fromRect.height / 2f);
        Vector3 endPos = new Vector3(toRect.x, toRect.y + toRect.height / 2f);

        Vector3 startTangent = startPos + Vector3.right * 50f;
        Vector3 endTangent = endPos + Vector3.left * 50f;

        Handles.BeginGUI();
        Handles.DrawBezier(
            startPos,
            endPos,
            startTangent,
            endTangent,
            Color.white,
            null,
            2f
        );
        Handles.EndGUI();
    }

    protected virtual void OnClickConnect(DialogueNode node)
    {
        if (selectedNodeForConnection == null)
        {
            selectedNodeForConnection = (T)(IGraphNode)node;
        }
        else if (!selectedNodeForConnection.Equals(node))
        {
            if (selectedNodeForConnection is DialogueNode selectedDialogueNode)
            {
                selectedDialogueNode.connectedNode = node;
            }
            selectedNodeForConnection = default;
        }
    }

    protected void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                            new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                            new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

}
