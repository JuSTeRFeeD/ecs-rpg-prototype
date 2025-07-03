// File: Editor/ColliderExplorerWindow.cs

using UnityEditor;
using UnityEngine;

public class ColliderExplorerWindow : EditorWindow
{
    [MenuItem("Tools/My/Collider Explorer %#&c")] // Ctrl+Alt+Shift+C
    public static void ShowWindow()
    {
        GetWindow<ColliderExplorerWindow>("Collider Explorer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Collider Explorer", EditorStyles.boldLabel);

        bool newEnabled = EditorGUILayout.Toggle("Enable Visualization", ColliderVisualizer.Enabled);
        if (newEnabled != ColliderVisualizer.Enabled)
        {
            ColliderVisualizer.Enabled = newEnabled;
            ColliderVisualizer.SavePrefs();
            SceneView.RepaintAll();
        }

        GUILayout.Space(10);
        GUILayout.Label("Filter By Collider Type", EditorStyles.label);

        ToggleType(typeof(BoxCollider), "Box Collider");
        ToggleType(typeof(SphereCollider), "Sphere Collider");
        ToggleType(typeof(CapsuleCollider), "Capsule Collider");
        ToggleType(typeof(MeshCollider), "Mesh Collider");

        GUILayout.Space(10);
        bool newShowNames = EditorGUILayout.Toggle("Show Object Names", ColliderVisualizer.ShowNames);
        if (newShowNames != ColliderVisualizer.ShowNames)
        {
            ColliderVisualizer.ShowNames = newShowNames;
            ColliderVisualizer.SavePrefs();
            SceneView.RepaintAll();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.HelpBox("Ctrl+Alt+Shift+C to open this panel.", MessageType.Info);
    }

    private void ToggleType(System.Type type, string label)
    {
        bool enabled = ColliderVisualizer.EnabledTypes.Contains(type);
        bool newEnabled = EditorGUILayout.Toggle(label, enabled);
        if (newEnabled && !enabled) ColliderVisualizer.EnabledTypes.Add(type);
        else if (!newEnabled && enabled) ColliderVisualizer.EnabledTypes.Remove(type);
    }
}