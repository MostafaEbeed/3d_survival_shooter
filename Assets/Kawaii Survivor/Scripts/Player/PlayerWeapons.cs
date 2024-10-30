using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private WeaponPosition[] weaponPositions; 
    
    public void AddWeapon(WeaponDataSO selectedWeapon, int weaponLevel)
    {
        weaponPositions[Random.Range(0, weaponPositions.Length)].AssignWeapon(selectedWeapon.Prefab, weaponLevel);
    }
}