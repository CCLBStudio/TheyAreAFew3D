using CCLBStudio.ScriptablePooling;
using Gameplay.Stratagem.Core;
using UnityEngine;

namespace Gameplay.Stratagem.AirStrike
{
    [CreateAssetMenu(fileName = "AirstrikeStratagemData", menuName = "Gameplay/Stratagem/Airstrike Data")]
    public class AirstrikeStratagemData : StratagemData
    {
        public ScriptablePool decalPool;
        public float spawnDistance = 400f;
        public float timeToReachTarget = 4f;
        
        public ScriptablePool explosionPool;
        public int explosions = 3;
        public float explosionDelay = 5.2f;
        public float timeBetweenExplosions = .4f;
        public float distanceBetweenExplosions = 5f;
    }
}
