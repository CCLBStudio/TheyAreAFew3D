using System.Collections.Generic;
using CCLBStudio.Inputs;
using Gameplay.Weapon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerWeaponHolder : SerializedMonoBehaviour, IWeaponHolder, IPlayerBehaviour
    {
        public PlayerFacade Facade { get; set; }

        [SerializeField] private WeaponData weapon;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Dictionary<WeaponData, Transform> pivots;

        private RuntimeWeapon _runtimeWeapon;

        #region Initialization

        public void InitBehaviour()
        {
            inputReader.AttackEvent += OnShootInputPressed;
            _runtimeWeapon = weapon.Equip(this);
        }

        public void OnAllBehavioursInitialized()
        {
        }

        #endregion

        private void OnShootInputPressed(bool pressed)
        {
            if (pressed)
            {
                _runtimeWeapon.StartShooting();
            }
            else
            {
                _runtimeWeapon.StopShooting();
            }
        }

        public Transform GetWeaponPivot(WeaponData forWeapon)
        {
            if (pivots.TryGetValue(forWeapon, out Transform t))
            {
                return t;
            }
            
            Debug.LogError($"Unable to find a pivot for weapon {forWeapon.name}. Returning {name} as pivot, which will probably fuck everything up.");
            return transform;
        }
    }
}
