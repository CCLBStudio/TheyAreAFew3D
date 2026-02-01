using System;
using UnityEngine;

namespace CCLBStudio.ScriptableVariable.Scripts
{
    public abstract class ScriptableComponentSetter<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected ScriptableVariable<T> scriptableVariable;
        [SerializeField] protected AutomaticSetEvent autoSetOn = AutomaticSetEvent.Start;
        
        [Flags]
        protected enum AutomaticSetEvent {Awake = 1, OnEnable = 2, Start = 4}

        protected virtual void Awake()
        {
            if ((autoSetOn & AutomaticSetEvent.Awake) == AutomaticSetEvent.Awake)
            {
                if (!TryGetComponent(out T c))
                {
                    Debug.LogError($"No component of type {typeof(T).Name} on object {name} !");
                    return;
                }
                
                SetVariable(c);
            }
        }
        
        protected virtual void OnEnable()
        {
            if ((autoSetOn & AutomaticSetEvent.OnEnable) == AutomaticSetEvent.OnEnable)
            {
                if (!TryGetComponent(out T c))
                {
                    Debug.LogError($"No component of type {typeof(T).Name} on object {name} !");
                    return;
                }
                
                SetVariable(c);
            }
        }
        
        protected virtual void Start()
        {
            if ((autoSetOn & AutomaticSetEvent.Start) == AutomaticSetEvent.Start)
            {
                if (!TryGetComponent(out T c))
                {
                    Debug.LogError($"No component of type {typeof(T).Name} on object {name} !");
                    return;
                }
                
                SetVariable(c);
            }
        }

        protected virtual void SetVariable(T newValue)
        {
            if (!newValue || !scriptableVariable)
            {
                Debug.LogError("Unable to perform assignation because at least one data is null !");
                return;
            }

            scriptableVariable.Value = newValue;
        }
    }
}