using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private WeaponPosition[] weaponPositions; 
    
    public bool TryAddWeapon(WeaponDataSO weapon, int level)
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if(weaponPositions[i].Weapon != null)
                continue;
            
            weaponPositions[i].AssignWeapon(weapon.Prefab, level);
            return true;
        }
        
        return false;
    }
}