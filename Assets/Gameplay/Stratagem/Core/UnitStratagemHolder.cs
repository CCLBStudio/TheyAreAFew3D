using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Stratagem.Core
{
    public class UnitStratagemHolder : MonoBehaviour, IStratagemHolder
    {
        [SerializeField] private StratagemData defaultStratagem;
        
        private readonly Dictionary<StratagemData, IRuntimeStratagem> _equipedStratagems = new();
        protected List<StratagemData> EquipedStratagemData => _equipedStratagems.Keys.ToList();

        protected void Start()
        {
            if (defaultStratagem)
            {
                EquipStratagem(defaultStratagem);
            }
            
            InitStratagemHolder();
        }

        protected virtual void InitStratagemHolder(){}

        public void EquipStratagem(StratagemData stratagem)
        {
            if (!stratagem) return;
            
            if (_equipedStratagems.ContainsKey(stratagem))
            {
                Debug.Log($"Stratagem {stratagem.name} is already equipped.");
                return;
            }

            if (!stratagem.Equip(this, out IRuntimeStratagem equippedStratagem))
            {
                return;
            }
            
            Debug.Log($"Successfully equipped {stratagem.StratagemName}!");
            _equipedStratagems.Add(stratagem, equippedStratagem);
        }

        protected IRuntimeStratagem GetRuntimeStratagem(StratagemData stratagem)
        {
            return _equipedStratagems.GetValueOrDefault(stratagem);
        }
    }
}
