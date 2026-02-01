using System;
using System.Collections.Generic;
using UnityEngine;

namespace CCLBStudio.ScriptableVariable.Scripts
{
    public abstract class ScriptableVariableList<T> : ScriptableVariable<List<T>>
    {
        public int Count => Value?.Count ?? 0;
        public int Capacity => Value?.Capacity ?? 0;
        
        /// <summary>
        /// Event fired every time the collection is modified.
        /// </summary>
        public event Action<List<T>> OnListModified;
        /// <summary>
        /// Event fired every time an element is added to the collection, no matter the method used (Add, AddRange, Insert, InsertRange). For method that add multiple elements, the event will be fired for each element.
        /// </summary>
        public event Action<T> OnElementAdded;
        /// <summary>
        /// Event fired every time an element is removed from the list, no matter the method used (Remove, RemoteAt, RemoveRange, RemoveAll, Clear). For method that remove multiple elements, the event will be fired for each element.
        /// </summary>
        public event Action<T> OnElementRemoved;
        
        private bool IsValid()
        {
            return Value != null;
        }

        public void Add(T elem)
        {
            if (!IsValid())
            {
                Debug.LogError("Fatal error : list is null !");
                return;
            }
            
            Value.Add(elem);
            OnElementAdded?.Invoke(elem);
            OnListModified?.Invoke(Value);
        }

        public void Insert(int index, T elem)
        {
            if (!IsValid())
            {
                Debug.LogError("Fatal error : list is null !");
                return;
            }

            if (index < 0 || index > Value.Count)
            {
                Debug.LogError("Fatal error : index out of range !");
                return;
            }
            
            Value.Insert(index, elem);
            OnElementAdded?.Invoke(elem);
            OnListModified?.Invoke(Value);
        }

        public bool Remove(T elem)
        {
            if (!IsValid())
            {
                Debug.LogError("Fatal error : list is null !");
                return false;
            }
            
            bool removed = Value.Remove(elem);

            if (removed)
            {
                OnElementRemoved?.Invoke(elem);
                OnListModified?.Invoke(Value);
            }

            return removed;
        }
        
        public void RemoveAt(int index)
        {
            if (!IsValid())
            {
                Debug.LogError("Fatal error : list is null !");
                return;
            }
            
            if (index < 0 || index >= Value.Count)
            {
                Debug.LogError("Fatal error : index out of range !");
                return;
            }

            T e = Value[index];
            Value.RemoveAt(index);
            OnElementRemoved?.Invoke(e);
            OnListModified?.Invoke(Value);
        }
        
        public bool Contains(T elem)
        {
            if (!IsValid())
            {
                Debug.LogError("Fatal error : list is null !");
                return false;
            }

            return Value.Contains(elem);
        }

        public void Clear()
        {
            if (!IsValid())
            {
                Debug.LogError("Fatal error : list is null !");
                return;
            }
            
            foreach (var e in Value)
            {
                OnElementRemoved?.Invoke(e);
            }

            Value.Clear();
            OnListModified?.Invoke(Value);
        }
    }
}