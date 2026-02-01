using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CCLBStudio.ScriptableVariable.Scripts
{
    public class ScriptableVariableCreator : EditorWindow
    {
        private ScriptableVariableCreatorSettings _settings;
        private string _typeFilter;
        private bool _createTypeList;
        private bool _createComponentSetter;
        private Type _selectedType;
        private readonly Dictionary<string, Type> _typeMap = new Dictionary<string, Type>();
        private string[] _filteredTypeNames;
        private int _selectedIndex = 0;
        private bool _selectedComponent;

        private void OnEnable()
        {
            _settings = LoadScriptableAsset<ScriptableVariableCreatorSettings>();
            
            BuildTypeMap();
        }
        
        private void OnGUI()
        {
            if (!_settings)
            {
                EditorGUILayout.HelpBox("Unable to find a setting file. Please create one (CCLB Studio/Scriptable Variable/Editor/Settings)", MessageType.Warning);
                _settings = LoadScriptableAsset<ScriptableVariableCreatorSettings>();
                return;
            }

            EditorGUI.BeginChangeCheck();
            _typeFilter = EditorGUILayout.TextField("Type Filter", _typeFilter);
            if (EditorGUI.EndChangeCheck())
            {
                UpdateFilteredTypes();
            }
            
            ShowTypePopup();
            
            _createTypeList = EditorGUILayout.Toggle("Create Type List", _createTypeList);
            _createComponentSetter = _selectedComponent && EditorGUILayout.Toggle("Create Component Setter", _createComponentSetter);

            if (_selectedType == null)
            {
                return;
            }
            
            string typeName = CapitalizeFirstLetter(_selectedType.Name);
            string btnTitle = $"Create: {typeName}Variable{(_createTypeList ? $", {typeName}ListVariable" : string.Empty)} {(_createComponentSetter ? $", Scriptable{typeName}Setter" : string.Empty)}";
                
            if (GUILayout.Button(btnTitle))
            {
                CreateNewType(_selectedType);

                if (_createTypeList)
                {
                    CreateTypeList(_selectedType);
                }
                    
                if (_createComponentSetter)
                {
                    CreateNewComponentSetter(_selectedType);
                }
                    
                AssetDatabase.Refresh();
            }
        }
        
        [MenuItem("CCLB Studio/Scriptable Variable/Script Generator")]
        public static void ShowWindow()
        {
            GetWindow<ScriptableVariableCreator>("Scriptable Variable Creator");
        }

        #region Types Popup Display

        private void ShowTypePopup()
        {
            if (string.IsNullOrEmpty(_typeFilter))
            {
                return;
            }
            
            if (_filteredTypeNames.Length <= 0)
            {
                EditorGUILayout.LabelField("No matching types.");
                _selectedType = null;
                _selectedComponent = false;
                return;
            }
            
            _selectedIndex = EditorGUILayout.Popup("Create Variable For", _selectedIndex, _filteredTypeNames);
            string selectedTypeName = _filteredTypeNames[_selectedIndex];
            _selectedType = _typeMap.TryGetValue(selectedTypeName, out _selectedType) ? _selectedType : null;
            _selectedComponent = _selectedType != null && _selectedType.IsSubclassOf(typeof(Component));
        }
        
        #endregion
        
        #region Type Search Methods

        private void BuildTypeMap()
        {
            _typeMap.Clear();
            string[] guids = AssetDatabase.FindAssets("t:MonoScript"/*,new[] { "Assets" }*/);
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (script == null)
                {
                    continue;
                }
                
                Type type = script.GetClass();
                if (type != null)
                {
                    _typeMap.TryAdd(type.Name, type);
                }
            }
            
            AddBuiltinTypesFromAssemblies();
        }

        private void AddBuiltinTypesFromAssemblies()
        {
            var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a =>
                    a.FullName.StartsWith("System") ||
                    a.FullName.StartsWith("mscorlib") ||
                    a.FullName.StartsWith("UnityEngine"));

            foreach (var assembly in assembliesToScan)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsPublic || type.IsAbstract) continue;
                    if (string.IsNullOrEmpty(type.Name)) continue;
                    if (_typeMap.ContainsKey(type.Name)) continue;

                    try
                    {
                        // Ignore generic type definitions like List<>
                        if (type.IsGenericTypeDefinition) continue;
                        _typeMap.Add(type.Name, type);
                        //_typeMap[type.Name] = type;
                    }
                    catch
                    {
                         /* some types may throw */
                         Debug.LogError("Unable to add type " + type.Name + " to the type map.");
                    }
                }
            }
        }
        
        private void UpdateFilteredTypes()
        {
            _filteredTypeNames = _typeMap.Keys
                .Where(x => x.StartsWith(_typeFilter, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            _selectedIndex = 0;
        }

        private HashSet<string> GetUsingsForType(Type type)
        {
            HashSet<string> usings = new HashSet<string>();

            // Add the namespace of the type
            if (!string.IsNullOrEmpty(type.Namespace))
            {
                usings.Add($"using {type.Namespace};");
            }

            // Add usings for any base types or interfaces
            Type baseType = type.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                if (!string.IsNullOrEmpty(baseType.Namespace))
                {
                    usings.Add($"using {baseType.Namespace};");
                }
                baseType = baseType.BaseType;
            }

            foreach (var iface in type.GetInterfaces())
            {
                if (!string.IsNullOrEmpty(iface.Namespace))
                {
                    usings.Add($"using {iface.Namespace};");
                }
            }

            // Display the result in the console
            Debug.Log($"Usings for {type.FullName}:\n" + string.Join("\n", usings));
            return usings;
        }
        
        #endregion

        private void CreateNewType(Type from)
        {
            string fullTypeName = $"{from.Name}Variable.cs";
            string savePath = Application.dataPath + _settings.saveTypePath + fullTypeName;
            if (File.Exists(savePath))
            {
                Debug.LogError($"C# file of type {fullTypeName} already exists !");
                return;
            }
            
            string fileContent = ProcessTemplate(_settings.simpleTypeTemplate, from);
            File.WriteAllText(savePath, string.Join("\r\n", fileContent));
        }
        
        private void CreateTypeList(Type from)
        {
            string fullTypeName = $"{CapitalizeFirstLetter(from.Name)}ListVariable.cs";
            string savePath = Application.dataPath + _settings.saveTypePath + fullTypeName;
            if (File.Exists(savePath))
            {
                Debug.LogError($"C# file of type {fullTypeName} already exists !");
                return;
            }

            string fileContent = ProcessTemplate(_settings.typeListTemplate, from);
            File.WriteAllText(savePath, string.Join("\r\n", fileContent));
        }
        
        private void CreateNewComponentSetter(Type from)
        {
            string fullTypeName = $"Scriptable{CapitalizeFirstLetter(from.Name)}Setter.cs";
            string savePath = Application.dataPath + _settings.saveTypePath + fullTypeName;
            if (File.Exists(savePath))
            {
                Debug.LogError($"C# file of type {fullTypeName} already exists !");
                return;
            }

            string fileContent = ProcessTemplate(_settings.componentSetterTemplate, from);
            File.WriteAllText(savePath, string.Join("\r\n", fileContent));
        }
        
        private string ProcessTemplate(string template, Type from)
        {
            string[] lines = template.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int tokenLength = ScriptableVariableCreatorSettings.Token.Length;
            var usingMap = GetUsingsForType(from);
            
            for (int i = 0; i < lines.Length; i++)
            {
                if (usingMap.Contains(lines[i]))
                {
                    usingMap.Remove(lines[i]);
                }
                
                int security = 100;
                bool search = true;
                while (search)
                {
                    security--;
                    if (security <= 0)
                    {
                        Debug.LogError("ALERT ! Security breach !");
                        break;
                    }
                    
                    int index = lines[i].IndexOf(ScriptableVariableCreatorSettings.Token, StringComparison.Ordinal);
                    search = index >= 0;
                    if (!search)
                    {
                        continue;
                    }

                    string subLine = lines[i].Substring(index + tokenLength);
                    string tagFound = string.Empty;
                    
                    foreach (var tag in ScriptableVariableCreatorSettings.Tags)
                    {
                        if (subLine.StartsWith(tag) && tag.Length > tagFound.Length)
                        {
                            tagFound = tag;
                        }
                    }

                    string tagReplacement = ConvertTag(tagFound, from.Name, CapitalizeFirstLetter(from.Name));
                    if (string.IsNullOrEmpty(tagReplacement))
                    {
                        Debug.Log($"Unknown tag {tagFound}");
                        continue;
                    }

                    lines[i] = lines[i].Substring(0, index) + tagReplacement +
                               lines[i].Substring(index + tokenLength + tagFound.Length);
                }
            }

            return string.Join("\n", usingMap) + "\n" + string.Join("\r\n", lines);
        }

        private string ConvertTag(string tag, string type, string capitalType)
        {
            return tag switch
            {
                "TYPE" => type,
                "CAPITALTYPE" => capitalType,
                _ => string.Empty
            };
        }
        
        private static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input[1..];
        }

        private static T LoadScriptableAsset<T>() where T : ScriptableObject
        {
            string[] assetPath = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new []{"Assets"});

            if (assetPath.Length <= 0)
            {
                Debug.LogError($"There is no asset of type {typeof(T).Name} in the project.");
                return null;
            }

            T result = (T)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assetPath[0]), typeof(T));
            return result;
        }
    }
}