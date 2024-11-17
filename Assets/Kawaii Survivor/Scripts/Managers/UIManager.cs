using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameStateListener
{
    public static UIManager instance;
    
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject weaponSelectionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject restartConfirmationPanel;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI xpText;
    
    private List<GameObject> panels = new List<GameObject>();

    public Slider HealthBar => healthBar;
    public TextMeshProUGUI HealthText => healthText;
    public Slider XPBar => xpBar;
    public TextMeshProUGUI XPText => xpText;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        panels.AddRange(new GameObject[]
        {
            menuPanel,
            weaponSelectionPanel,
            gamePanel,
            gameOverPanel,
            stageCompletePanel,
            waveTransitionPanel,
            shopPanel
        });

        GameManager.onGamePaused += GamePausedCallback;
        GameManager.onGameResumed += GameResumedCallback;
        
        pausePanel.SetActive(false);
        HideRestartConfirmationPanel();
    }

    private void OnDestroy()
    {
        GameManager.onGamePaused -= GamePausedCallback;
        GameManager.onGameResumed -= GameResumedCallback;
    }
    
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;
            
            case GameState.WEAPONSELECTION:
                ShowPanel(weaponSelectionPanel);
                break;
            
            case GameState.GAME:
                ShowPanel(gamePanel);
                break;
            
            case GameState.GAMEOVER:
                ShowPanel(gameOverPanel);
                break;

            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;

            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;
            
            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;
        }
    }

    private void ShowPanel(GameObject panel)
    {
        foreach (GameObject p in panels)
            p.SetActive(p == panel);
    }
    
    private void GamePausedCallback()
    {
        pausePanel.SetActive(true);
    }

    private void GameResumedCallback()
    {
        pausePanel.SetActive(false);
    }

    public void ShowRestartConfirmationPanel()
    {
        restartConfirmationPanel.SetActive(true);
    }
    
    public void HideRestartConfirmationPanel()
    {
        restartConfirmationPanel.SetActive(false);
    }
}
