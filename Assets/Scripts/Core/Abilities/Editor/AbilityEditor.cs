using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;

namespace Core.Abilities.Editor
{
    [CustomEditor(typeof(Ability))]
    public class AbilityEditor : UnityEditor.Editor
    {
        private readonly List<string> _unlinkedComponentsPaths = new List<string>();
        private SerializedProperty _components;
        private GenericMenu _addComponentMenu;
        private string _componentsFolder;

        public override void OnInspectorGUI()
        {
            DrawComponents();
            DrawAddComponent();
            DrawUnlinkedComponents();
        }

        private void OnEnable()
        {
            _components = serializedObject.FindProperty("_components");
            _addComponentMenu = new GenericMenu();

            IEnumerable<Type> componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(AbilityComponent)) && type.IsAbstract == false);

            foreach (Type type in componentTypes)
                _addComponentMenu.AddItem(new GUIContent(type.Name), false, () => AddComponent(type));

            string abilityPath = AssetDatabase.GetAssetPath(target);
            string directory = Path.GetDirectoryName(abilityPath);
            _componentsFolder = Path.Combine(directory, target.name);

            if (AssetDatabase.IsValidFolder(_componentsFolder) == false)
                AssetDatabase.CreateFolder(directory, target.name);

            RefreshUnlinkedComponents();
            Undo.undoRedoPerformed += RefreshUnlinkedComponents;

        }

        private void OnDisable() => Undo.undoRedoPerformed -= RefreshUnlinkedComponents;

        private void DrawComponents()
        {
            serializedObject.Update();

            for (int i = 0; i < _components.arraySize; i++)
                DrawComponent(i);

            if (serializedObject.ApplyModifiedProperties())
                RefreshUnlinkedComponents();
        }

        private void DrawComponent(int index)
        {
            EditorGUILayout.Space();

            SerializedProperty element = _components.GetArrayElementAtIndex(index);
            if (element.objectReferenceValue == null)
            {
                GUILayout.BeginVertical("box");
                {
                    EditorGUILayout.HelpBox("Component doesn't exist", MessageType.Error);

                    if (GUILayout.Button("Remove", EditorStyles.toolbarButton))
                        _components.DeleteArrayElementAtIndex(index);
                }
                EditorGUILayout.EndHorizontal();
                return;
            }

            GUILayout.BeginVertical("box");
            {
                bool removed = false;

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(element.objectReferenceValue.GetType().Name, EditorStyles.boldLabel);

                    if (GUILayout.Button("Up", EditorStyles.toolbarButton) && index > 0)
                        _components.MoveArrayElement(index, index - 1);

                    if (GUILayout.Button("Down", EditorStyles.toolbarButton) && index < _components.arraySize - 1)
                        _components.MoveArrayElement(index, index + 1);

                    if (GUILayout.Button("Remove", EditorStyles.toolbarButton))
                    {
                        _components.DeleteArrayElementAtIndex(index);
                        removed = true;
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (removed)
                    return;

                UnityEditor.Editor editor = CreateEditor(element.objectReferenceValue);
                editor.OnInspectorGUI();
            }
            GUILayout.EndVertical();
        }

        private void DrawAddComponent()
        {

            EditorGUILayout.Separator();

            if (GUILayout.Button("Add Component"))
                _addComponentMenu.ShowAsContext();
        }

        private void AddComponent(Type componentType)
        {
            string assetPath = Path.Combine(_componentsFolder, $"{componentType.Name}_{Guid.NewGuid().ToString()}.asset");
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            AbilityComponent newComponent = ScriptableObject.CreateInstance(componentType) as AbilityComponent;
            AssetDatabase.CreateAsset(newComponent, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            serializedObject.Update();

            _components.arraySize++;
            _components.GetArrayElementAtIndex(_components.arraySize - 1).objectReferenceValue = newComponent;

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawUnlinkedComponents()
        {
            if (_unlinkedComponentsPaths.Count == 0)
                return;

            EditorGUILayout.Space();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.HelpBox($"Unlinked components count: {_unlinkedComponentsPaths.Count}", MessageType.Info);
                if (GUILayout.Button("Delete unlinked"))
                {
                    if (EditorUtility.DisplayDialog(
                        "Delete Unlinked Components",
                        "Are you sure you want to delete all unlinked components? This action cannot be undone.",
                        "Delete",
                        "Cancel"))
                    {
                        DeleteUnlinkedComponents();
                    }
                }

            }
            EditorGUILayout.EndVertical();
        }

        private void RefreshUnlinkedComponents()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(AbilityComponent)}", new[] { _componentsFolder });
            HashSet<string> linkedAssets = new HashSet<string>();

            serializedObject.Update();

            for (int i = 0; i < _components.arraySize; i++)
            {
                SerializedProperty element = _components.GetArrayElementAtIndex(i);
                if (element.objectReferenceValue == null)
                    continue;

                string path = AssetDatabase.GetAssetPath(element.objectReferenceValue);
                linkedAssets.Add(path);
            }

            _unlinkedComponentsPaths.Clear();
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (linkedAssets.Contains(path))
                    continue;

                _unlinkedComponentsPaths.Add(path);
            }
        }

        private void DeleteUnlinkedComponents()
        {
            if (_unlinkedComponentsPaths.Count == 0)
                return;

            for (int i = 0; i < _unlinkedComponentsPaths.Count; i++)
                AssetDatabase.DeleteAsset(_unlinkedComponentsPaths[i]);

            AssetDatabase.Refresh();
            _unlinkedComponentsPaths.Clear();
        }
    }
}