using CCLBStudio.ScriptablePooling;
using Gameplay.Stratagem.Core;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerStratagemLauncher : MonoBehaviour, IPlayerBehaviour
    {
        [SerializeField] private ScriptablePool stratagemCallers;
        [SerializeField] private Transform throwOrigin;
        public PlayerFacade Facade { get; set; }

        public void InitBehaviour()
        {
        }

        public void OnAllBehavioursInitialized()
        {
        }

        public void LaunchStratagem(IRuntimeStratagem stratagem)
        {
            stratagemCallers.RequestObjectAs<PlayerStratagemCaller>()
                .At(throwOrigin.position)
                .AlignWith(transform)
                .Throw(transform.position, stratagem);
        }
    }
}
