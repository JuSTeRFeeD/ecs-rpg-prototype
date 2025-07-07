using System.Linq;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class AttributeStartSetEditorWindow : EditorWindow
{
    private Vector2 leftScroll, middleScroll, rightScroll;
    private string selectedFolder = "";
    private AttributeStartSet selectedSet;
    private List<string> folders = new List<string>();
    private List<AttributeStartSet> setsInFolder = new List<AttributeStartSet>();
    private string newFolderName = "NewCategory";
    private AttributeType newAttributeToAdd;
    private string tempSetName;

    [MenuItem("Tools/RPG/Attribute Start Set Editor")]
    public static void ShowWindow() => GetWindow<AttributeStartSetEditorWindow>("Attribute Start Sets");

    private void OnEnable()
    {
        RefreshFolders();
        if (folders.Count > 0)
        {
            selectedFolder = folders[0];
            RefreshSets();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        DrawFoldersPanel();
        DrawAttributeSetListPanel();
        DrawAttributeSetEditorPanel();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawAttributeSetEditorPanel()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true));
        if (selectedSet != null)
        {
            GUILayout.Label("Editing: " + selectedSet.name, EditorStyles.boldLabel);
            rightScroll = EditorGUILayout.BeginScrollView(rightScroll, GUILayout.ExpandWidth(true));

            SerializedObject so = new SerializedObject(selectedSet);
            so.Update();

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Set Name:", EditorStyles.boldLabel, GUILayout.Width(80));
            tempSetName = EditorGUILayout.TextField(tempSetName, GUILayout.Width(200));
            if (GUILayout.Button("Apply Name", GUILayout.Width(100)))
            {
                if (!string.IsNullOrEmpty(tempSetName) && tempSetName != selectedSet.name)
                {
                    string assetPath = AssetDatabase.GetAssetPath(selectedSet);
                    string assetDirectory = Path.GetDirectoryName(assetPath);
                    string newPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(assetDirectory, tempSetName + ".asset"));
                    AssetDatabase.RenameAsset(assetPath, Path.GetFileNameWithoutExtension(newPath));
                    AssetDatabase.SaveAssets();
                    selectedSet.name = Path.GetFileNameWithoutExtension(newPath);
                    tempSetName = selectedSet.name;
                    EditorUtility.SetDirty(selectedSet);
                }
            }
            EditorGUILayout.EndHorizontal();

            SerializedProperty attributesProp = so.FindProperty("attributes");

            if (attributesProp != null && attributesProp.isArray)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUILayout.Label("Attribute Type", EditorStyles.boldLabel, GUILayout.Width(250));
                GUILayout.Label("Value", EditorStyles.boldLabel, GUILayout.Width(100));
                GUILayout.Label("", GUILayout.Width(30));
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);

                for (int i = 0; i < attributesProp.arraySize; i++)
                {
                    SerializedProperty element = attributesProp.GetArrayElementAtIndex(i);
                    if (element == null) continue;

                    SerializedProperty typeProp = element.FindPropertyRelative("type");
                    SerializedProperty valueProp = element.FindPropertyRelative("baseValue");

                    if (typeProp == null || valueProp == null)
                    {
                        Debug.LogError($"Invalid properties in element at index {i}. Check AttributeStartSet.Entry structure.");
                        continue;
                    }

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    EditorGUILayout.PropertyField(typeProp, GUIContent.none, GUILayout.Width(250));
                    GUILayout.Space(10);
                    EditorGUILayout.PropertyField(valueProp, GUIContent.none, GUILayout.Width(100));
                    if (GUILayout.Button("X", GUILayout.Width(30)))
                    {
                        attributesProp.DeleteArrayElementAtIndex(i);
                        so.ApplyModifiedProperties();
                        so.Update();
                        EditorUtility.SetDirty(selectedSet);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5);
                }

                GUILayout.Space(10);
                if (GUILayout.Button("Add Attribute", GUILayout.Height(25)))
                {
                    var allTypes = Resources.LoadAll<AttributeType>("AttributeTypes");
                    var existing = new HashSet<AttributeType>();

                    for (int i = 0; i < attributesProp.arraySize; i++)
                    {
                        var typeProp = attributesProp.GetArrayElementAtIndex(i).FindPropertyRelative("type");
                        if (typeProp != null && typeProp.objectReferenceValue is AttributeType existingType)
                        {
                            existing.Add(existingType);
                        }
                    }

                    var available = allTypes.Where(t => !existing.Contains(t)).ToList();

                    if (available.Count > 0)
                    {
                        GenericMenu menu = new GenericMenu();
                        foreach (var type in available)
                        {
                            menu.AddItem(new GUIContent(type.name), false, () =>
                            {
                                EditorApplication.delayCall += () =>
                                {
                                    so.Update();
                                    attributesProp.arraySize++;
                                    var newElement = attributesProp.GetArrayElementAtIndex(attributesProp.arraySize - 1);
                                    var typeProp = newElement.FindPropertyRelative("type");
                                    var valueProp = newElement.FindPropertyRelative("baseValue");

                                    if (typeProp != null && valueProp != null)
                                    {
                                        typeProp.objectReferenceValue = type;
                                        valueProp.floatValue = 0f;
                                        so.ApplyModifiedProperties();
                                        so.Update();
                                        EditorUtility.SetDirty(selectedSet);
                                    }
                                    else
                                    {
                                        Debug.LogError("Failed to add attribute: type or baseValue property is missing.");
                                    }
                                };
                            });
                        }
                        menu.ShowAsContext();
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("No available attributes", "All attributes are already added.", "OK");
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Attributes array is not found or invalid in the selected set.", MessageType.Error);
            }

            so.ApplyModifiedProperties();
            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.HelpBox("Select an Attribute Set to edit its values.", MessageType.Info);
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawFoldersPanel()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
        GUILayout.Label("Folders", EditorStyles.boldLabel);

        leftScroll = EditorGUILayout.BeginScrollView(leftScroll);
        foreach (var folder in folders)
        {
            string relativePath = folder.Replace("Assets/AttributeStartSets", "");
            int depth = relativePath.Split('/', '\\').Length - 1;
            string indent = new string('-', depth * 2) + " ";
            string displayName = indent + Path.GetFileName(folder);

            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = (folder == selectedFolder) ? Color.yellow : Color.white },
                hover = { textColor = Color.yellow }
            };

            var selected = folder == selectedFolder ? " \u2666" : "";
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(displayName), style);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            if (GUI.Button(rect, displayName + selected, style))
            {
                selectedFolder = folder;
                RefreshSets();
                selectedSet = null;
                tempSetName = "";
            }
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        GUILayout.Label("Create SubFolder:", EditorStyles.boldLabel);
        newFolderName = EditorGUILayout.TextField("", newFolderName);

        if (GUILayout.Button("Create Folder"))
        {
            string fullPath = Path.Combine(string.IsNullOrEmpty(selectedFolder) ? "Assets/AttributeStartSets" : selectedFolder, newFolderName);
            if (!AssetDatabase.IsValidFolder(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                AssetDatabase.Refresh();
                RefreshFolders();
                selectedFolder = fullPath;
                newFolderName = "NewCategory";
                RefreshSets();
            }
            else
            {
                EditorUtility.DisplayDialog("Folder Exists", "A folder with this name already exists.", "OK");
            }
        }

        if (!string.IsNullOrEmpty(selectedFolder) && selectedFolder != "Assets/AttributeStartSets")
        {
            if (GUILayout.Button("Delete Folder"))
            {
                if (EditorUtility.DisplayDialog("Delete Folder", "Are you sure?", "Yes", "Cancel"))
                {
                    AssetDatabase.DeleteAsset(selectedFolder);
                    AssetDatabase.Refresh();
                    RefreshFolders();
                    selectedFolder = folders.Count > 0 ? folders[0] : "";
                    RefreshSets();
                    selectedSet = null;
                    tempSetName = "";
                }
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawAttributeSetListPanel()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
        GUILayout.Label("Attribute Sets", EditorStyles.boldLabel);

        middleScroll = EditorGUILayout.BeginScrollView(middleScroll);
        if (setsInFolder != null)
        {
            foreach (var set in setsInFolder)
            {
                if (set == null) continue;

                string displayName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(set));
                GUIStyle style = set == selectedSet ? new GUIStyle(EditorStyles.boldLabel) { normal = { textColor = Color.green } } : EditorStyles.label;

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(displayName, style))
                {
                    selectedSet = set;
                    tempSetName = set.name;
                }

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    string path = AssetDatabase.GetAssetPath(set);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                    RefreshSets();
                    selectedSet = null;
                    tempSetName = "";
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();

        if (!string.IsNullOrEmpty(selectedFolder))
        {
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Set (Empty)"))
            {
                CreateNewStartSet(false);
                RefreshSets();
            }
            if (GUILayout.Button("Create New Set (All Attributes)"))
            {
                CreateNewStartSet(true);
                RefreshSets();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void CreateNewStartSet(bool includeAllAttributes)
    {
        string path = selectedFolder;
        string name = "NewAttributeSet";
        string fullPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, name + ".asset"));
        var asset = ScriptableObject.CreateInstance<AttributeStartSet>();

        if (includeAllAttributes)
        {
            var allTypes = Resources.LoadAll<AttributeType>("AttributeTypes");
            asset.attributes = new List<AttributeStartSet.Entry>();
            foreach (var type in allTypes)
            {
                asset.attributes.Add(new AttributeStartSet.Entry
                {
                    type = type,
                    baseValue = 0f
                });
            }
        }

        AssetDatabase.CreateAsset(asset, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        selectedSet = asset;
        tempSetName = asset.name;
        EditorGUIUtility.PingObject(asset);
        Selection.activeObject = asset;
    }

    private void RefreshFolders()
    {
        folders = new List<string>();
        string root = "Assets/AttributeStartSets";
        if (!Directory.Exists(root))
        {
            Directory.CreateDirectory(root);
            AssetDatabase.Refresh();
        }

        var all = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);
        folders.Add(root);
        folders.AddRange(all);
    }

    private void RefreshSets()
    {
        setsInFolder = new List<AttributeStartSet>();
        if (string.IsNullOrEmpty(selectedFolder)) return;

        var guids = AssetDatabase.FindAssets("t:AttributeStartSet", new[] { selectedFolder });
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<AttributeStartSet>(path);
            if (asset != null)
            {
                setsInFolder.Add(asset);
            }
        }
    }
}
