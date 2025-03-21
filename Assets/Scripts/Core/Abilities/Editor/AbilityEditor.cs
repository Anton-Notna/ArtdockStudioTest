using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;

namespace Core.Abilities.Editor
{
    public class DerivedTypesMenu
    {
        private readonly GenericMenu _menu;

        public DerivedTypesMenu(Type baseType, Action<Type> onSelected)
        {
            _menu = new GenericMenu();

            IEnumerable<Type> componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(baseType) && type.IsAbstract == false);

            foreach (Type type in componentTypes)
                _menu.AddItem(new GUIContent(type.Name), false, () => onSelected.Invoke(type));
        }

        public void ShowAsContext() => _menu.ShowAsContext();
    }

    [CustomEditor(typeof(Ability))]
    public class AbilityEditor : UnityEditor.Editor
    {
        private readonly List<string> _unlinkedComponentsPaths = new List<string>();
        private readonly List<string> _unlinkedSelectorsPaths = new List<string>();

        private SerializedProperty _selector;
        private SerializedProperty _components;
        private DerivedTypesMenu _changeSelector;
        private DerivedTypesMenu _addComponent;
        private string _previousTargetName;
        private string _assetsFolder;
        private bool _assetsFolderInited;

        private int UnlinkedAssetsCount => _unlinkedComponentsPaths.Count + _unlinkedSelectorsPaths.Count;

        public override void OnInspectorGUI()
        {
            if (_previousTargetName != target.name)
                InitAssetsFolder();

            if (_assetsFolderInited == false)
                return;

            serializedObject.Update();

            DrawSelector();
            DrawComponents();

            if (serializedObject.ApplyModifiedProperties())
                RefreshUnlinkedAssets();

            DrawAddComponent();
            DrawUnlinkedAssets();
        }

        private void OnEnable()
        {
            _selector = serializedObject.FindProperty("_selectorPreset");
            _components = serializedObject.FindProperty("_components");
            _changeSelector = new DerivedTypesMenu(typeof(SelectorPreset), ChangeSelector);
            _addComponent = new DerivedTypesMenu(typeof(AbilityComponent), AddComponent);

            InitAssetsFolder();
            Undo.undoRedoPerformed += RefreshUnlinkedAssets;
        }

        private void OnDisable() => Undo.undoRedoPerformed -= RefreshUnlinkedAssets;

        private void InitAssetsFolder()
        {
            _assetsFolderInited = false;

            if (target == null)
                return;

            if (string.IsNullOrEmpty(target.name))
                return;

            string abilityPath = AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(abilityPath))
                return;

            string directory = Path.GetDirectoryName(abilityPath);
            _assetsFolder = Path.Combine(directory, target.name);
            if (AssetDatabase.IsValidFolder(_assetsFolder) == false)
                AssetDatabase.CreateFolder(directory, target.name);

            _previousTargetName = target.name;
            _assetsFolderInited = true;

            RefreshUnlinkedAssets();
        }

        private void DrawSelector()
        {
            EditorGUILayout.LabelField("Selector Preset:", EditorStyles.miniLabel);

            GUILayout.BeginVertical("box");
            {
                if (_selector.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("Empty Selector Preset. Ability requires Selector Preset.", MessageType.Error);
                }
                else
                {
                    EditorGUILayout.LabelField(_selector.objectReferenceValue.GetType().Name, EditorStyles.boldLabel);
                    UnityEditor.Editor editor = CreateEditor(_selector.objectReferenceValue);
                    editor.OnInspectorGUI();
                }

                if (GUILayout.Button("Change Selector Preset", EditorStyles.toolbarButton))
                    _changeSelector.ShowAsContext();
            }
            GUILayout.EndVertical();
        }

        private void ChangeSelector(Type selectorType)
        {
            SelectorPreset selector = CreateAsset<SelectorPreset>(selectorType);

            serializedObject.Update();
            _selector.objectReferenceValue = selector;
            serializedObject.ApplyModifiedProperties();
            RefreshUnlinkedAssets();
        }

        private void DrawComponents()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Components:", EditorStyles.miniLabel);

            for (int i = 0; i < _components.arraySize; i++)
                DrawComponent(i);
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
            EditorGUILayout.Space();

            if (GUILayout.Button("Add Component"))
                _addComponent.ShowAsContext();
        }

        private void AddComponent(Type componentType)
        {
            AbilityComponent component = CreateAsset<AbilityComponent>(componentType);

            serializedObject.Update();
            _components.arraySize++;
            _components.GetArrayElementAtIndex(_components.arraySize - 1).objectReferenceValue = component;
            serializedObject.ApplyModifiedProperties();
        }

        private TBase CreateAsset<TBase>(Type type) where TBase : ScriptableObject
        {
            string assetPath = Path.Combine(_assetsFolder, $"{type.Name}_{Guid.NewGuid().ToString()}.asset");
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            TBase result = ScriptableObject.CreateInstance(type) as TBase;
            AssetDatabase.CreateAsset(result, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return result;
        }

        private void DrawUnlinkedAssets()
        {
            if (UnlinkedAssetsCount == 0)
                return;

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.HelpBox($"Unlinked components count: {UnlinkedAssetsCount}", MessageType.Info);
                if (GUILayout.Button("Delete unlinked"))
                {
                    if (EditorUtility.DisplayDialog(
                        "Delete Unlinked Components",
                        "Are you sure you want to delete all unlinked components? This action cannot be undone.",
                        "Delete",
                        "Cancel"))
                    {
                        DeleteUnlinkedAssets();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void RefreshUnlinkedAssets()
        {
            if (_assetsFolderInited == false)
                return;

            serializedObject.Update();
            RefreshUnlinkedSelectors();
            RefreshUnlinkedComponents();
        }

        private void RefreshUnlinkedSelectors()
        {
            IEnumerable<string> unlinkedSelectors = FindRelatedAssetsPaths<SelectorPreset>();
            if (_selector.objectReferenceValue != null)
            {
                string currentSelector = AssetDatabase.GetAssetPath(_selector.objectReferenceValue);
                unlinkedSelectors = unlinkedSelectors.Where(path => path != currentSelector);
            }
            _unlinkedSelectorsPaths.Clear();
            _unlinkedSelectorsPaths.AddRange(unlinkedSelectors);
        }

        private void RefreshUnlinkedComponents()
        {
            HashSet<string> linkedComponents = new HashSet<string>();
            for (int i = 0; i < _components.arraySize; i++)
            {
                SerializedProperty element = _components.GetArrayElementAtIndex(i);
                if (element.objectReferenceValue == null)
                    continue;

                string path = AssetDatabase.GetAssetPath(element.objectReferenceValue);
                linkedComponents.Add(path);
            }
            IEnumerable<string> unlinkedComponents = FindRelatedAssetsPaths<AbilityComponent>().Where(path => linkedComponents.Contains(path) == false);
            _unlinkedComponentsPaths.Clear();
            _unlinkedComponentsPaths.AddRange(unlinkedComponents);
        }

        private void DeleteUnlinkedAssets()
        {
            if (UnlinkedAssetsCount == 0)
                return;

            for (int i = 0; i < _unlinkedSelectorsPaths.Count; i++)
                AssetDatabase.DeleteAsset(_unlinkedSelectorsPaths[i]);
            _unlinkedSelectorsPaths.Clear();

            for (int i = 0; i < _unlinkedComponentsPaths.Count; i++)
                AssetDatabase.DeleteAsset(_unlinkedComponentsPaths[i]);
            _unlinkedComponentsPaths.Clear();

            AssetDatabase.Refresh();
        }

        private IEnumerable<string> FindRelatedAssetsPaths<T>() where T : ScriptableObject =>
            AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { _assetsFolder }).Select(guid => AssetDatabase.GUIDToAssetPath(guid));

    }
}