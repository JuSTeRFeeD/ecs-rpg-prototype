using System.Collections.Generic;
using System.IO;
using Heavenage.Scripts.ECS.Runtime.AbilitySystem.Attributes;
using UnityEditor;
using UnityEngine;

namespace Heavenage.Scripts.ECS.Editor.Attributes
{
    public class AttributeTypeEditorWindow : EditorWindow {
        private Vector2 _scroll;
        private List<AttributeType> _attributeTypes;

        private string _newAttributeName = "NewAttribute";

        [MenuItem("Tools/RPG/Attribute Types Window")]
        public static void ShowWindow() => GetWindow<AttributeTypeEditorWindow>("Attribute Types");

        private void OnEnable() {
            RefreshList();
        }

        private void RefreshList() {
            _attributeTypes = new List<AttributeType>(Resources.LoadAll<AttributeType>("AttributeTypes"));
        }

        private void OnGUI() {
            GUILayout.Label("Attribute Types", EditorStyles.boldLabel);

            GUILayout.Space(8);

            GUILayout.BeginHorizontal();
            _newAttributeName = GUILayout.TextField(_newAttributeName);

            if (GUILayout.Button("Create New", GUILayout.Width(100))) {
                CreateNewAttributeType(_newAttributeName);
                _newAttributeName = "NewAttribute";
                RefreshList();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            _scroll = GUILayout.BeginScrollView(_scroll);

            foreach (var attr in _attributeTypes) {
                if (attr == null) continue;

                EditorGUILayout.BeginVertical("box");
            
                EditorGUILayout.LabelField("Filename", attr.name);

                EditorGUILayout.BeginHorizontal();
                
                var serializedAttr = new SerializedObject(attr);
                var idProp = serializedAttr.FindProperty("id");
                EditorGUILayout.PropertyField(idProp, new GUIContent("ID"));
                serializedAttr.ApplyModifiedProperties();

                if (GUILayout.Button("Delete", GUILayout.Width(60))) {
                    DeleteAttributeType(attr);
                    RefreshList();
                    break;
                }

                EditorGUILayout.EndHorizontal();

                EditorUtility.SetDirty(attr); // Ensure changes are saved

                EditorGUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
        }

        private void CreateNewAttributeType(string name) {
            string path = "Assets/Resources/Attributes";
            if (!AssetDatabase.IsValidFolder(path)) {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            var newAttr = ScriptableObject.CreateInstance<AttributeType>();
            
            var serializedAttr = new SerializedObject(newAttr);
            var idProp = serializedAttr.FindProperty("id");
            idProp.stringValue = name;
            serializedAttr.ApplyModifiedProperties();

            string assetPath = $"{path}/{name}.asset";
            AssetDatabase.CreateAsset(newAttr, assetPath);
            AssetDatabase.SaveAssets();
        }

        private void DeleteAttributeType(AttributeType attr) {
            string path = AssetDatabase.GetAssetPath(attr);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
        }
    }
}
