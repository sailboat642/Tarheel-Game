using UnityEngine;
using UnityEditor;

public static class EditorZoomArea
{
    private static Matrix4x4 prevGuiMatrix;

    public static Rect Begin(float zoomScale, Rect screenCoordsArea)
    {
        GUI.EndGroup(); // End the group Unity begins for EditorWindow

        Rect clippedArea = ScaleRect(screenCoordsArea, 1f / zoomScale, screenCoordsArea.TopLeft());
        clippedArea.y += 21; // Adjust for title bar height

        GUI.BeginGroup(clippedArea);
        prevGuiMatrix = GUI.matrix;

        Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
        Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1f));
        GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

        return clippedArea;
    }

    public static void End()
    {
        GUI.matrix = prevGuiMatrix;
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(0, 21, Screen.width, Screen.height)); // Reset clipping
    }

    private static Vector2 TopLeft(this Rect rect)
    {
        return new Vector2(rect.x, rect.y);
    }

    private static Rect ScaleRect(Rect rect, float scale, Vector2 pivot)
    {
        Vector2 size = rect.size * scale;
        Vector2 center = rect.center - (rect.center - pivot) * scale;
        return new Rect(center - size * 0.5f, size);
    }

}

