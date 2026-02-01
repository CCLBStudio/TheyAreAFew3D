using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CCLBStudio.Facade
{
    public abstract class MonoFacade<TB> : MonoBehaviour
    {
        [SerializeField] private BehaviourSearchOption searchOption = BehaviourSearchOption.Self;
        
        private Dictionary<Type, TB> _behaviours;
        
        private enum BehaviourSearchOption {Self, InChildren, InParent}
        protected List<TB> Behaviours => _behaviours?.Values.ToList();

        protected virtual void Awake()
        {
            SearchForBehaviours();
        }

        private void SearchForBehaviours()
        {
            _behaviours = searchOption switch
            {
                BehaviourSearchOption.Self => GetComponents<TB>()
                    .ToDictionary(component => component.GetType(), component => component),
                BehaviourSearchOption.InChildren => GetComponentsInChildren<TB>()
                    .ToDictionary(component => component.GetType(), component => component),
                BehaviourSearchOption.InParent => GetComponentsInParent<TB>()
                    .ToDictionary(component => component.GetType(), component => component),
                _ => new Dictionary<Type, TB>()
            };
        }

        public T GetBehaviour<T>() where T : TB
        {
            Type type = typeof(T);
            if (!_behaviours.TryGetValue(type, out var behaviour))
            {
                Debug.LogError($"No behaviour of type {type} found on {gameObject.name}");
                return default;
            }
            
            return (T)behaviour;
        }
    }
}
