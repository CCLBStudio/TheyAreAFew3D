using UnityEngine;

namespace CCLBStudio.ScriptablePooling
{
    public static class ScriptablePoolObjectExtender
    {
        public static T At<T>(this T obj, Vector3 position) where T : IScriptablePooledObject
        {
            obj.ObjectTransform.position = position;
            return obj;
        }
        
        public static T AtLocal<T>(this T obj, Vector3 localPosition) where T : IScriptablePooledObject
        {
            obj.ObjectTransform.localPosition = localPosition;
            return obj;
        }
        
        public static T WithRot<T>(this T obj, Quaternion rotation) where T : IScriptablePooledObject
        {
            obj.ObjectTransform.rotation = rotation;
            return obj;
        }

        public static T WithLocalRot<T>(this T obj, Quaternion localRotation) where T : IScriptablePooledObject
        {
            obj.ObjectTransform.localRotation = localRotation;
            return obj;
        }
        
        public static T AlignWith<T>(this T obj, Transform other) where T : IScriptablePooledObject
        {
            obj.ObjectTransform.forward = other.forward;
            return obj;
        }

        public static T Parent<T>(this T obj, Transform parent) where T : IScriptablePooledObject
        {
            obj.ObjectTransform.SetParent(parent);
            return obj;
        }
    }
}
