using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player;
    
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private NavMeshAgent agent;

    public NavMeshAgent Agent => agent;
    
    private void OnEnable()
    {
        
    }

    void Update()
    {
        /*if(player != null) 
            FollowPlayer();*/
    }

    public void StorePlayer(Player player, Vector3 spawnPosition)
    {
        this.player = player;
        
        agent.enabled = true;
        
        agent.Warp(spawnPosition);
    }

    public void ClearPlayer()
    {
        this.player = null;
    }

    public void FollowPlayer()
    {
        if(player == null || !player.gameObject.activeInHierarchy) return;

        agent.destination = player.transform.position;
    }
}
