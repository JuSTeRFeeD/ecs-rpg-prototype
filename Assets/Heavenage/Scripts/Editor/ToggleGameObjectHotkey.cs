using UnityEditor;
using UnityEngine;

public static class ToggleSelectedActiveState
{
    // Ctrl + Shift + E
    [MenuItem("Tools/My/Hotkeys/Toggle Activation %#e")]
    private static void ToggleActivation()
    {
        var selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects selected to toggle.");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selectedObjects, "Toggle Activation");

        foreach (var obj in selectedObjects)
        {
            obj.SetActive(!obj.activeSelf);
            EditorUtility.SetDirty(obj);
        }

        Debug.Log($"[Toggle] Toggled {selectedObjects.Length} object(s).");
    }

    [MenuItem("Tools/Hotkeys/Toggle Activation %#e", true)]
    private static bool ValidateToggleActivation()
    {
        return Selection.gameObjects.Length > 0;
    }
}