using System.Collections;
using System;
using UnityEngine;

public class Cash : DroppableCurrency, ICollectable
{
    [Header("Actions")]
    public static Action<Cash> onCollected;
    
    protected override void Collceted()
    {
        onCollected?.Invoke(this);
    }
}
