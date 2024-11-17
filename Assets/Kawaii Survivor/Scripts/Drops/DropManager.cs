using System;
using Kawaii_Survivor.Scripts.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Pool;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Cash cashPrefab;
    [SerializeField] private Chest chestPrefab;

    [Header("Settings")]
    [Range(0, 100)]
    [SerializeField] private int cashDropChance;
    [Range(0, 100)]
    [SerializeField] private int chestDropChance;
    
    [Header("Pooling")]
    private ObjectPool<Candy> candyPool;
    private ObjectPool<Cash> cashPool;
    
    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
        Candy.onCollected += ReleaseCandy;
        Cash.onCollected += ReleaseCash;
    }

    private void Start()
    {
        candyPool = new ObjectPool<Candy>(CandyCreateFunction, CandyActionOnGet, CandyActionOnRelease, CandyActionOnDestroy);
        cashPool = new ObjectPool<Cash>(CashCreateFunction, CashActionOnGet, CashActionOnRelease, CashActionOnDestroy);
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
        Candy.onCollected -= ReleaseCandy;
        Cash.onCollected -= ReleaseCash;
    }

    
    private Candy CandyCreateFunction()
    {
        Candy candyInstance = Instantiate(candyPrefab, transform);
        
        return candyInstance;
    }

    private void CandyActionOnGet(Candy candy)
    {
        candy.gameObject.SetActive(true);
    }

    private void CandyActionOnRelease(Candy candy)
    {
        candy.gameObject.SetActive(false);
    }

    private void CandyActionOnDestroy(Candy candy)
    {
        Destroy(candy.gameObject);
    }

    private Cash CashCreateFunction()
    {
        Cash cashInstance = Instantiate(cashPrefab, transform);
        
        return cashInstance;
    }

    private void CashActionOnGet(Cash cash)
    {
        cash.gameObject.SetActive(true);
    }

    private void CashActionOnRelease(Cash cash)
    {
        cash.gameObject.SetActive(false);
    }

    private void CashActionOnDestroy(Cash cash)
    {
        Destroy(cash.gameObject);
    }
   
    private void EnemyPassedAwayCallback(Vector3 enemyPosition)
    {
        bool shouldDropCash = Random.Range(0, 101) <= cashDropChance;
        
        DroppableCurrency droppable = shouldDropCash ? cashPool.Get() : candyPool.Get();
        droppable.transform.position = enemyPosition;

        TryDropChest(enemyPosition);
    }

    private void TryDropChest(Vector2 enemyPosition)
    {
        bool shouldDropChest = Random.Range(0, 101) <= chestDropChance;
        
        if(!shouldDropChest)
            return;
        
        Instantiate(chestPrefab, enemyPosition, Quaternion.identity, transform);
    }

    private void ReleaseCandy(Candy candy)
    {
        candyPool.Release(candy);
    }
    
    private void ReleaseCash(Cash cash)
    {
        cashPool.Release(cash);
    }
}
