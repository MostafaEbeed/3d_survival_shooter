using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Data", menuName = "Scriptable Objects/New Object", order = 0)]
public class ObjectDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite icon { get; private set; }
    
    [field: SerializeField] public int Price { get; private set; }
    
    [field: SerializeField] public int RecyclePrice { get; private set; }
    
    [field: Range(0, 3)]
    [field: SerializeField] public int Rarity { get; private set; }
    
    [SerializeField] private StatData[] statDatas;
   

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            Dictionary<Stat, float> stats = new Dictionary<Stat, float>();

            foreach (StatData data in statDatas)
            {
                stats.Add(data.Stat, data.Value);
            }
            
            return stats;
        }
        
        private set { }
    }
}

[Serializable]
public struct StatData
{
    public Stat Stat;
    public float Value;
}