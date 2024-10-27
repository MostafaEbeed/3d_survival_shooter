using System;
using System.Collections;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectable
{
    private bool isCollceted;

    private void OnEnable()
    {
        isCollceted = false;
    }

    public void Collect(Player player)
    {
        if(isCollceted)
            return;
        
        isCollceted = true;
        
        StartCoroutine(MoveTowardsPlayer(player));
    }

    private IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0.0f;
        Vector2 initialPosition = transform.position;
       
        while (timer < 1.0f)
        {
            Vector2 targetPosition = player.GetCenter();
            
            transform.position = Vector2.Lerp(initialPosition, targetPosition, timer * 2f);
            timer += Time.deltaTime;
            
            yield return null;
        }

        Collceted();
    }

    protected abstract void Collceted();
}
