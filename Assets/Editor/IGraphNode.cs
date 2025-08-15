using UnityEngine;

public interface IGraphNode
{
    Rect rect { get; set; }
    bool isSelected { get; set; }
    void Draw();
    bool ProcessEvents(Event e);
}
