using System.Collections.Generic;
using System.Linq;
using CCLBStudio.Inputs;
using Gameplay.Stratagem.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Player
{
    public sealed class PlayerStratagemHolder : UnitStratagemHolder, IPlayerBehaviour
    {
        public PlayerFacade Facade { get; set; }
        
        [SerializeField] private InputReader inputReader;
        [SerializeField] private UnityEvent<IRuntimeStratagem> onStratagemRequested;
        
        private List<StratagemData> _availableStratagems = new();
        private readonly List<StratagemInputDirection> _currentInputPath = new();
        private StratagemData _selectedStratagem;

        #region Initialization

        protected override void InitStratagemHolder()
        {
            inputReader.OnStratagemInputBegin += OnStratagemInputBegin;
            inputReader.OnStratagemInputCancelled += OnStratagemInputCancelled;
        }


        public void InitBehaviour()
        {
        }

        public void OnAllBehavioursInitialized()
        {
        }

        #endregion

        #region Stratagem Input

        private void OnStratagemInputBegin()
        {
            _availableStratagems = EquipedStratagemData;
            _currentInputPath.Clear();
            _selectedStratagem = null;
            
            inputReader.OnStratagemInputUp += OnInputUp;
            inputReader.OnStratagemInputDown += OnInputDown;
            inputReader.OnStratagemInputLeft += OnInputLeft;
            inputReader.OnStratagemInputRight += OnInputRight;
        }
        
        private void OnStratagemInputCancelled()
        {
            inputReader.OnStratagemInputUp -= OnInputUp;
            inputReader.OnStratagemInputDown -= OnInputDown;
            inputReader.OnStratagemInputLeft -= OnInputLeft;
            inputReader.OnStratagemInputRight -= OnInputRight;

            if (_selectedStratagem)
            {
                RequestStratagem();
            }
        }

        private void OnInputUp()
        {
            CheckValidStratagem(StratagemInputDirection.Up);
        }

        private void OnInputDown()
        {
            CheckValidStratagem(StratagemInputDirection.Down);
        }

        private void OnInputLeft()
        {
            CheckValidStratagem(StratagemInputDirection.Left);
        }

        private void OnInputRight()
        {  
            CheckValidStratagem(StratagemInputDirection.Right);
        }

        #endregion

        #region Stratagem Invoke

        private void RequestStratagem()
        {
            Debug.Log($"Requested stratagem : {_selectedStratagem.name}");
            onStratagemRequested?.Invoke(GetRuntimeStratagem(_selectedStratagem));
        }

        #endregion

        #region Stratagem Checking
        
        private void CheckValidStratagem(StratagemInputDirection inputDirection)
        {
            if (_availableStratagems.Count == 0)
            {
                Debug.Log("No available stratagems");
                return;
            }
            
            _currentInputPath.Add(inputDirection);
            FilterAvailableStratagems();
            
            _selectedStratagem = CheckForMatch();
            if (_selectedStratagem)
            {
                OnStratagemSelected();
            }
        }
        
        private void OnStratagemSelected()
        {
            inputReader.OnStratagemInputUp -= OnInputUp;
            inputReader.OnStratagemInputDown -= OnInputDown;
            inputReader.OnStratagemInputLeft -= OnInputLeft;
            inputReader.OnStratagemInputRight -= OnInputRight;
            
            Debug.Log($"Stratagem selected: {_selectedStratagem.StratagemName} !");
        }

        private void FilterAvailableStratagems()
        {
            _availableStratagems = _availableStratagems
                .Where(s => s.InputPath.Count >= _currentInputPath.Count &&
                            !_currentInputPath.Where((value, i) => s.InputPath[i] != value)
                                .Any())
                .ToList();
        }

        private StratagemData CheckForMatch()
        {
            if (_availableStratagems.Count == 0)
            {
                Debug.Log("No available stratagems");
                return null;
            }

            foreach (var stratagem in _availableStratagems)
            {
                if (stratagem.InputPath.Count != _currentInputPath.Count)
                {
                    continue;
                }

                bool match = false;
                for (int i = 0; i < stratagem.InputPath.Count; i++)
                {
                    if (stratagem.InputPath[i] != _currentInputPath[i])
                    {
                        match = false;
                        break;
                    }
                    
                    match = true;
                }

                if (match)
                {
                    return stratagem;
                }
            }

            return null;
        }

        #endregion
    }
}
