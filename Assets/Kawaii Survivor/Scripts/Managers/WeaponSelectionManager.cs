using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [FormerlySerializedAs("conrainerParent")]
    [Header("Elements")] 
    [SerializeField] private Transform containerParent;
    [SerializeField] private WeaponSelectionContainer weaponSelectionPrefab;
    [SerializeField] private PlayerWeapons playerWeapons;
    
    [Header("Data")]
    [SerializeField] private WeaponDataSO[] starterWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;
    
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                if(selectedWeapon == null) 
                    return;
                
                playerWeapons.AddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;
            
            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    [Button]
    private void Configure()
    {
        containerParent.Clear();

        for (int i = 0; i < 3; i++)
        {
            GenerateWeaponContainer();
        }
    }

    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer containerInstance = Instantiate(weaponSelectionPrefab, containerParent);
        
        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];
        
        int level = Random.Range(0, 4);
        
        containerInstance.Configure(weaponData.Sprite, weaponData.Name, level);
        
        containerInstance.Button.onClick.RemoveAllListeners();
        containerInstance.Button.onClick.AddListener(()=> WeaponSelectedCallback(containerInstance, weaponData, level));
    }

    private void WeaponSelectedCallback(WeaponSelectionContainer containerInstance, WeaponDataSO weaponData, int level)
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = level;
        
        foreach (WeaponSelectionContainer container in containerParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == containerInstance)
            {
                container.Select();
            }
            else
            {
                container.Deselct();
            }
        }
    }
}
