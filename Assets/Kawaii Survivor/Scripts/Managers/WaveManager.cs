using System;
using System.Collections.Generic;
using Kawaii_Survivor.Scripts.Enemy;
using UnityEngine;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[RequireComponent(typeof(WaveManager))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Player player;
    [SerializeField] private WaveManagerUI ui;
    
    [Header("Settings")]
    [SerializeField] private float waveDuration;
    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;
    
    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    private List<float> localCounters = new List<float>();

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }

    void Start()
    {
      
    }

    void Update()
    {
        if(!isTimerOn)
            return;

        if (timer < waveDuration)
        {
            ManageCurrentWave();
            string timerString = (Mathf.RoundToInt(waveDuration - timer)).ToString();
            ui.UpdateTimerText(timerString);
        }
        else
            StartWaveTransition();
            
    }

    private void StartWave(int waveIndex)
    {
        Debug.Log("Starting wave " + waveIndex);
        
        ui.UpdateWaveText("Wave " + (currentWaveIndex + 1) + "/" + waves.Length);
        
        localCounters.Clear();
        foreach (WaveSegment waveSegment in waves[waveIndex].segments)
        {
            localCounters.Add(1);
        }
        
        timer = 0;
        isTimerOn = true;
    }
    
    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];
            
            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart && timer > tEnd)
                continue;
            
            float timeSinceSegmentStart = timer - tStart;

            float spawnDelay = 1f / segment.spawnFrequency;

            if (timeSinceSegmentStart / spawnDelay > localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, null);
                localCounters[i]++;
            }
        }
        
        timer += Time.deltaTime;
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;
        
        DefeatAllEnemies();
        
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("waves Completed");
            ui.UpdateTimerText(" ");
            ui.UpdateWaveText("Stage Completed");
            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);
        }
        else
            GameManager.instance.WaveCompletedCallback();
    }

    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }
    
    private void DefeatAllEnemies()
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
        {
            enemy.PassAwayAfterWave();
        }
    }
    
    private Vector3 GetSpawnPosition()
    {
        Vector3 direction = Random.insideUnitSphere;
        direction.y = 0;
        Vector3 offset = direction.normalized * Random.Range(6, 10);
        Vector3 taregtPoition = (Vector3)player.transform.position + offset;
        
        taregtPoition.x = Mathf.Clamp(taregtPoition.x, -18, 18);
        taregtPoition.z = Mathf.Clamp(taregtPoition.z, -8, 8);
        
        return taregtPoition;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;
            
            case GameState.GAMEOVER:
                isTimerOn = false;
                DefeatAllEnemies();
                break;
        }
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0,100)] public Vector2 tStartEnd;
    public float spawnFrequency;
    public GameObject prefab;
}