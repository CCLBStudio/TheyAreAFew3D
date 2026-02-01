using CCLBStudio.ScriptablePooling;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Gameplay.Stratagem.AirStrike
{
    public class AirstrikeDecalPooledObject : MonoBehaviour, IScriptablePooledObject
    {
        private MMF_Player _feedback;

        public Transform ObjectTransform => transform;
        public ScriptablePool Pool { get; set; }

        public float FeedbackLength()
        {
            return _feedback ? _feedback.TotalDuration : 0f;
        }
        public void OnObjectCreated()
        {
            _feedback = GetComponentInChildren<MMF_Player>();
        }

        public void OnObjectRequested()
        {
            _feedback?.PlayFeedbacks();
        }

        public void OnObjectReleased()
        {
        }
    }
}
