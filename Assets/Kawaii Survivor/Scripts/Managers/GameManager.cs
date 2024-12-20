using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Actions")] 
    public static Action onGamePaused;
    public static Action onGameResumed;

    [Header("Refs")] 
    [SerializeField] private Player player;
    
    public Player Player => player;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        SetGameState(GameState.MENU);
    }

    public void StartGame() => SetGameState(GameState.GAME);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECTION);
    public void StartShop() => SetGameState(GameState.SHOP);
    
    public void SetGameState(GameState gameState)
    {
        IEnumerable<IGameStateListener> gameStateListeners =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListener>();

        foreach (IGameStateListener gameStateListener in gameStateListeners)
        {
            gameStateListener.GameStateChangedCallback(gameState);
        }
    }

    public void ManageGameover()
    {
        SceneManager.LoadScene(0);
    }

    public void WaveCompletedCallback()
    {
        if (Player.instance.HasLeveledUp() || WaveTransitionManager.instance.HasCollectedChest())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
        {
            SetGameState(GameState.SHOP);
        }
    }

    public void PauseButtonCallback()
    {
       Time.timeScale = 0; 
       onGamePaused?.Invoke();
    }
    
    public void ResumeButtonCallback()
    {
        Time.timeScale = 1; 
        onGameResumed?.Invoke();
    }

    public void RestartFromPause()
    {
        Time.timeScale = 1;
        ManageGameover();
    }
}


public interface IGameStateListener
{
    void GameStateChangedCallback(GameState gameState);
}