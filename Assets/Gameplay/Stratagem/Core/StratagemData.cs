using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Stratagem.Core
{
    public abstract class StratagemData : ScriptableObject
    {
        public string StratagemName => stratagemName;
        public List<StratagemInputDirection> InputPath => inputPath;
        
        [SerializeField] private string stratagemName;
        [SerializeField] private GameObject stratagemPrefab;
        [SerializeField] private List<StratagemInputDirection> inputPath;

        public bool Equip(IStratagemHolder holder, out IRuntimeStratagem generatedStratagem)
        {
            generatedStratagem = null;
            
            if (!stratagemPrefab)
            {
                Debug.LogError($"No stratagem prefab found for {stratagemName}!");
                return false;
            }

            generatedStratagem = Instantiate(stratagemPrefab).GetComponent<IRuntimeStratagem>();
            if (generatedStratagem == null)
            {
                Debug.LogError($"No {nameof(IRuntimeStratagem)} object found on prefab {stratagemName}!");
                return false;
            }
            
            generatedStratagem = generatedStratagem.Initialize(this, holder);
            return generatedStratagem != null;
        }
    }
}
