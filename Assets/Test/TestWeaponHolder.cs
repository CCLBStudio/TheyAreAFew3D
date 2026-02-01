using CCLBStudio.Inputs;
using Gameplay.Weapon;
using UnityEngine;

public class TestWeaponHolder : MonoBehaviour, IWeaponHolder
{
    [SerializeField] private WeaponData weapon;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform weaponPivot;

    private RuntimeWeapon _runtimeWeapon;
    void Start()
    {
        inputReader.AttackEvent += OnShootInputPressed;
        _runtimeWeapon = weapon.Equip(this);
    }

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

    public Transform GetWeaponPivot()
    {
        return weaponPivot;
    }
}
