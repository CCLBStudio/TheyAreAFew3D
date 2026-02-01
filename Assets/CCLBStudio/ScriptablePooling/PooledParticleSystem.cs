using CCLBStudio.ScriptablePooling;
using UnityEngine;

public class PooledParticleSystem : MonoBehaviour, IScriptablePooledObject
{
    public Transform ObjectTransform => transform;
    public ScriptablePool Pool { get; set; }

    [SerializeField] private ParticleSystem system;

    private bool _checkForRelease;
    private Quaternion _initialRotation;

    private void Update()
    {
        if (!_checkForRelease)
        {
            return;
        }

        if (!system.IsAlive())
        {
            Pool.ReleaseObject(this);
        }
    }

    public void Play()
    {
        if (!system)
        {
            Pool.ReleaseObject(this);
            return;
        }

        system.Play(true);
        _checkForRelease = true;
    }
    
    public void OnObjectCreated()
    {
        if (!system)
        {
            system = GetComponent<ParticleSystem>();
            if (!system)
            {
                Debug.LogError("No particle system on objet !");
            }
        }

        _initialRotation = transform.rotation;
    }

    public void OnObjectRequested()
    {
    }

    public void OnObjectReleased()
    {
        _checkForRelease = false;
        transform.position = Vector3.zero;
        transform.rotation = _initialRotation;
    }
}
