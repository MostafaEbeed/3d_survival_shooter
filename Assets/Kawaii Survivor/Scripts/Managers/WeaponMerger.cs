using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
     public static WeaponMerger instance;

     [Header("Elements")]
     [SerializeField] private PlayerWeapons playerWeapons;
     
     private List<Weapon> weaponsToMerge = new List<Weapon>();

     [Header("Actions")] 
     public static Action<Weapon> onMerge;
     
     private void Awake()
     {
          if (instance == null)
               instance = this;
          else
               Destroy(gameObject);
     }

     public bool CanMerge(Weapon weapon)
     {
         if(weapon.Level >= 3)
              return false;
         
         weaponsToMerge.Clear();
         weaponsToMerge.Add(weapon);
         
         Weapon[] weapons = playerWeapons.GetWeapons();

         foreach (Weapon playerWeapon in weapons)
         {
              if(playerWeapon == null)
                   continue;
              
              if(playerWeapon == weapon)
                   continue;
              
              if(playerWeapon.weaponData.Name != weapon.weaponData.Name)
                   continue;
              
              if(playerWeapon.Level != weapon.Level)
                   continue;

              weaponsToMerge.Add(playerWeapon);
              return true;
         }
         
         return false;
     }

     public void Merge()
     {
          if (weaponsToMerge.Count < 2)
          {
               Debug.Log("No enough weapons to merge");
               return;
          }
          
          DestroyImmediate(weaponsToMerge[1].gameObject);

          weaponsToMerge[0].Upgrade();

          Weapon weapon = weaponsToMerge[0];
          weaponsToMerge.Clear();
          
          onMerge?.Invoke(weapon);
     }
}