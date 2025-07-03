// File: Editor/ColliderVisualizer.cs
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[InitializeOnLoad]
public static class ColliderVisualizer
{
    static ColliderVisualizer()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        LoadPrefs(); // Загружаем при запуске
    }

    private const string EnabledKey = "ColliderVisualizer.Enabled";
    private const string ShowNamesKey = "ColliderVisualizer.ShowNames";

    public static bool Enabled = true;
    public static bool ShowNames = true;

    public static HashSet<Type> EnabledTypes = new()
    {
        typeof(BoxCollider),
        typeof(SphereCollider),
        typeof(CapsuleCollider),
        typeof(MeshCollider)
    };

    public static void SavePrefs()
    {
        EditorPrefs.SetBool(EnabledKey, Enabled);
        EditorPrefs.SetBool(ShowNamesKey, ShowNames);
    }

    private static void LoadPrefs()
    {
        Enabled = EditorPrefs.GetBool(EnabledKey, true);
        ShowNames = EditorPrefs.GetBool(ShowNamesKey, true);
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (!Enabled) return;

        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

        foreach (Collider col in GameObject.FindObjectsOfType<Collider>())
        {
            if (!EnabledTypes.Contains(col.GetType())) continue;

            Handles.color = GetColorForCollider(col);

            if (col is BoxCollider box)
                DrawBox(box);
            else if (col is SphereCollider sphere)
                DrawSphere(sphere);
            else if (col is CapsuleCollider capsule)
                DrawCapsule(capsule);
            else if (col is MeshCollider mesh)
                DrawMesh(mesh);

            if (ShowNames)
            {
                Vector3 pos = col.bounds.center + Vector3.up * 0.5f;
                GUIStyle style = new GUIStyle(EditorStyles.boldLabel)
                {
                    normal = { textColor = Handles.color },
                    fontSize = 11
                };

                Handles.BeginGUI();
                Vector2 guiPos = HandleUtility.WorldToGUIPoint(pos);
                GUI.Label(new Rect(guiPos.x, guiPos.y, 200, 20), col.name, style);
                Handles.EndGUI();
            }
        }
    }

    private static Color GetColorForCollider(Collider col)
    {
        return col switch
        {
            BoxCollider => Color.green,
            SphereCollider => Color.cyan,
            CapsuleCollider => Color.yellow,
            MeshCollider => Color.red,
            _ => Color.white
        };
    }

    private static void DrawBox(BoxCollider box)
    {
        Matrix4x4 m = Matrix4x4.TRS(box.transform.position, box.transform.rotation, box.transform.lossyScale);
        using (new Handles.DrawingScope(m))
        {
            Handles.DrawWireCube(box.center, box.size);
        }
    }

    private static void DrawSphere(SphereCollider sphere)
    {
        Handles.DrawWireDisc(sphere.transform.position + sphere.center, Vector3.up, sphere.radius * sphere.transform.lossyScale.x);
    }

    private static void DrawCapsule(CapsuleCollider cap)
    {
        // Упростим — нарисуем сферу на концах и цилиндр между
        Vector3 center = cap.transform.position + cap.center;
        float height = cap.height * cap.transform.lossyScale.y;
        float radius = cap.radius * cap.transform.lossyScale.x;

        Handles.DrawWireArc(center + Vector3.up * (height / 2 - radius), Vector3.right, Vector3.back, 180, radius);
        Handles.DrawWireArc(center - Vector3.up * (height / 2 - radius), Vector3.right, Vector3.forward, 180, radius);
        Handles.DrawLine(center + Vector3.forward * radius + Vector3.up * (height / 2 - radius),
                         center + Vector3.forward * radius - Vector3.up * (height / 2 - radius));
        Handles.DrawLine(center - Vector3.forward * radius + Vector3.up * (height / 2 - radius),
                         center - Vector3.forward * radius - Vector3.up * (height / 2 - radius));
    }
    
    private static Material _wireMaterial;
    
    private static void DrawMesh(MeshCollider mesh)
    {
        if (mesh.sharedMesh == null) return;

        if (_wireMaterial == null)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            _wireMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            _wireMaterial.SetInt("_ZWrite", 0);
            _wireMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
        }

        GL.wireframe = true;
        _wireMaterial.SetPass(0);
        Graphics.DrawMeshNow(mesh.sharedMesh, mesh.transform.localToWorldMatrix);
        GL.wireframe = false;
    }
}
