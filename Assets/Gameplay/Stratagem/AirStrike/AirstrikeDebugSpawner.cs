using Gameplay.Stratagem.Core;
using UnityEngine;

namespace Gameplay.Stratagem.AirStrike
{
    public class AirstrikeDebugSpawner : MonoBehaviour, IStratagemHolder
    {
        [SerializeField] private StratagemData defaultStratagem;
        [SerializeField] private Camera cam;
        [SerializeField] private Transform player;
        
        private IRuntimeStratagem _airstrike;

        private void Start()
        {
            if (defaultStratagem)
            {
                EquipStratagem(defaultStratagem);
            }
        }
        
        public void EquipStratagem(StratagemData stratagem)
        {
            if (!stratagem) return;
            
            if (_airstrike != null)
            {
                Debug.Log($"Stratagem {stratagem.name} is already equipped.");
                return;
            }

            if (!stratagem.Equip(this, out IRuntimeStratagem equippedStratagem))
            {
                Debug.LogError($"Failed to equip stratagem {stratagem.name}.");
                return;
            }
            
            Debug.Log($"Successfully equipped {stratagem.StratagemName}!");
            _airstrike = equippedStratagem;
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var ctx = new DefaultStratagemLaunchContext(player.position, hit.point);
                    _airstrike?.Launch(ctx);
                }
            }
        }
    }
}
