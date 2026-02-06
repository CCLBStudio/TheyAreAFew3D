using System;
using UnityEngine;

namespace CCLBStudio.ScriptablePooling
{
    public static class ScriptablePoolObjectExtender
    {
        public static T At<T>(this T obj, Vector3 position) where T : IScriptablePooledObject
        {
            if (obj is not MonoBehaviour b)
            {
                throw new Exception($"Object {obj} is not a monoBehaviour.");
            }

            b.transform.position = position;
            return obj;
        }
        
        public static T AlignWith<T>(this T obj, Transform other) where T : IScriptablePooledObject
        {
            if (obj is not MonoBehaviour b)
            {
                throw new Exception($"Object {obj} is not a monoBehaviour.");
            }

            b.transform.forward = other.forward;
            return obj;
        }
    }
}
