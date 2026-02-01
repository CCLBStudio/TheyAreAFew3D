using Gameplay.Stratagem.Core;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerStratagemLauncher : MonoBehaviour, IPlayerBehaviour
    {
        public PlayerFacade Facade { get; set; }

        public void InitBehaviour()
        {
        }

        public void OnAllBehavioursInitialized()
        {
        }

        public void LaunchStratagem(IRuntimeStratagem stratagem)
        {
            // throw ball
        }
    }
}
