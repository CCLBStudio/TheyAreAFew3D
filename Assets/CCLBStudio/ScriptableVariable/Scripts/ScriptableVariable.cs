using System;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CCLBStudio.ScriptableVariable.Scripts
{
    public abstract class ScriptableVariable<T> : ScriptableObject
    {
        public T Value { get => GetValue(); set => SetValue(value); }
        public event Action<T> OnValueChanged;
        
        [SerializeField, EnableIf(nameof(EnableValueCondition))] private T value;
        [SerializeField, HideInInspector] private T savedValue;

        private void SetValue(T newValue)
        {
            value = newValue;
            OnValueChanged?.Invoke(value);
        }
        
        private T GetValue()
        {
            return value;
        }
        
        /// <summary>
        /// Method to define the conditions to enable the modification of 'value' inside the editor.
        /// </summary>
        /// <returns></returns>
        protected virtual bool EnableValueCondition() {return !Application.isPlaying;}
        
        #region Editor

#if UNITY_EDITOR
        private void OnEnable()
        {
            Debug.Log($"[{name}] ➜ Listening to playmode events");
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    savedValue = Clone(value);
                    break;
                
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                
                case PlayModeStateChange.ExitingPlayMode:
                    value = savedValue;
                    break;
                
                case PlayModeStateChange.EnteredEditMode:
                    break;
            }
        }
        
        private static T Clone(T source)
        {
            if (source is UnityEngine.Object unityObject)
            {
                return unityObject ? source : default;
            }
            
            if (ReferenceEquals(source, null))
            {
                return default;
            }

            Type type = typeof(T);

            if (type.IsPrimitive || type.IsEnum || type.IsValueType)
            {
                return source;
            }

            return JsonUtility.FromJson<T>(JsonUtility.ToJson(source));
        }

#endif
        #endregion
    }
}