using UnityEngine;
using UnityEngine.Serialization;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [FormerlySerializedAs("conrainerParent")]
    [Header("Elements")] 
    [SerializeField] private Transform containerParent;
    [SerializeField] private WeaponSelectionContainer weaponSelectionPrefab;
    
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

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
    }
}
