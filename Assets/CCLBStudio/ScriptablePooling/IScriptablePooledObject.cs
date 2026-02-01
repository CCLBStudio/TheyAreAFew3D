using UnityEngine;

namespace CCLBStudio.ScriptablePooling
{
    public interface IScriptablePooledObject
    {
        public Transform ObjectTransform { get; }
        public ScriptablePool Pool { get; set; }
        public void OnObjectCreated();
        public void OnObjectRequested();
        public void OnObjectReleased();
    }
}
