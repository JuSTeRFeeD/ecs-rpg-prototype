// File: Editor/SceneSwitcherWindow.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class SceneSwitcherWindow : EditorWindow
{
    private Vector2 scrollPos;
    private string searchQuery = "";
    private Dictionary<string, List<string>> categorizedScenes = new();
    private HashSet<string> foldouts = new();

    private const string preferredFolder = "Assets/Scenes";

    [MenuItem("Tools/My/Scene Switcher %#&s")] // Ctrl+Alt+Shift+S
    public static void ShowWindow()
    {
        GetWindow<SceneSwitcherWindow>("Scene Switcher");
    }

    private void OnEnable()
    {
        RefreshScenes();
    }

    private void RefreshScenes()
    {
        categorizedScenes.Clear();
        var guids = AssetDatabase.FindAssets("t:Scene");

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.EndsWith(".unity")) continue;

            string category = GetSceneCategory(path);

            if (!categorizedScenes.ContainsKey(category))
                categorizedScenes[category] = new List<string>();

            categorizedScenes[category].Add(path);
        }
    }

    private string GetSceneCategory(string path)
    {
        if (path.StartsWith(preferredFolder))
            return "📁 My Scenes";
        if (path.StartsWith("Packages"))
            return "📦 Packages";
        if (path.Contains("Plugins") || path.Contains("ThirdParty"))
            return "🔌 Third-Party";
        return "📂 Other";
    }

    private void OnGUI()
    {
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("🔄 Refresh", GUILayout.Width(80)))
            RefreshScenes();

        GUILayout.Space(5);
        searchQuery = EditorGUILayout.TextField("🔍 Search", searchQuery);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var category in categorizedScenes.Keys.OrderBy(k => k))
        {
            var scenes = categorizedScenes[category]
                .Where(s => string.IsNullOrEmpty(searchQuery) || Path.GetFileNameWithoutExtension(s).ToLower().Contains(searchQuery.ToLower()))
                .ToList();

            if (scenes.Count == 0) continue;

            // Foldout state
            bool isOpen = foldouts.Contains(category);
            bool newIsOpen = EditorGUILayout.Foldout(isOpen, category, true);

            if (newIsOpen != isOpen)
            {
                if (newIsOpen) foldouts.Add(category);
                else foldouts.Remove(category);
            }

            if (!newIsOpen) continue;

            EditorGUI.indentLevel++;
            foreach (var path in scenes.OrderBy(p => Path.GetFileNameWithoutExtension(p)))
            {
                string sceneName = Path.GetFileNameWithoutExtension(path);
                if (GUILayout.Button(sceneName, EditorStyles.miniButton))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(path);
                }
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndScrollView();
    }
}
