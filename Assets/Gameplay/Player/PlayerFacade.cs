using System.Collections.Generic;
using CCLBStudio.Facade;
using CCLBStudio.ScriptableBehaviour;
using CCLBStudio.ScriptableVariable.Scripts;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerFacade : MonoFacade<IPlayerBehaviour>
    {
        public PlayerStratagemHolder StratagemHolder => GetBehaviour<PlayerStratagemHolder>();
        
        [SerializeField] private PlayerFacadeListVariable playerFacadeList;
        [SerializeField] private List<ScriptableAction> onBeforeBehavioursInit;

        protected override void Awake()
        {
            base.Awake();
            playerFacadeList.Add(this);
            
            var behaviours = Behaviours;
            foreach (var b in behaviours)
            {
                b.Facade = this;
                b.InitBehaviour();
            }

            foreach (var b in behaviours)
            {
                b.OnAllBehavioursInitialized();
            }
        }
    }
}
