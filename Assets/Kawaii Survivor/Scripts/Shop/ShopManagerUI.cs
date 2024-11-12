using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    [Header("Player Stats Elements")]
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private RectTransform playerStatsClosePanel;
    private Vector2 playerStatsOpenPos;
    private Vector2 playerStatsClosePos;
    
    [Header("Inventory Elements")]
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryClosePanel;
    private Vector2 inventoryOpenPos;
    private Vector2 inventoryClosePos;
    
    [Header("Item Info Elements")]
    [SerializeField] private RectTransform itemInfoSlidePanel;
    private Vector2 itemInfoOpenPos;
    private Vector2 itemInfoClosePos;
    
    
    IEnumerator Start()
    {
        yield return null;

        ConfigurePlayerStatsPanel();
        ConfigureInventoryPanel();
        ConfigureItemInfoPanel();
    }
    
    private void ConfigurePlayerStatsPanel()
    {
        float width = Screen.width / (4 * playerStatsPanel.lossyScale.x);
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);
        
        playerStatsOpenPos = playerStatsPanel.anchoredPosition;
        playerStatsClosePos = playerStatsOpenPos - Vector2.right * width;
        
        playerStatsPanel.anchoredPosition = playerStatsClosePos;
        
        HidePlayerStats();
    }

    public void ShowPlayerStats()
    {
        playerStatsPanel.gameObject.SetActive(true);
        playerStatsClosePanel.gameObject.SetActive(true);
        playerStatsClosePanel.GetComponent<Image>().raycastTarget = true;
        
        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.move(playerStatsPanel, playerStatsOpenPos, 0.5f).setEase(LeanTweenType.easeInCubic);
        
        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.alpha(playerStatsClosePanel, 0.8f, 0.5f).setRecursive(false);
    }

    public void HidePlayerStats()
    {
        //playerStatsClosePanel.SetActive(false);
        //playerStatsPanel.gameObject.SetActive(false);
        
        playerStatsClosePanel.GetComponent<Image>().raycastTarget = false;
        
        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.move(playerStatsPanel, playerStatsClosePos, 0.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => playerStatsPanel.gameObject.SetActive(false));

        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.alpha(playerStatsClosePanel, 0, 0.5f).setRecursive(false)
            .setOnComplete(() => playerStatsClosePanel.gameObject.SetActive(true));
    }
    
    private void ConfigureInventoryPanel()
    {
        float width = Screen.width / (4 * inventoryPanel.lossyScale.x);
        inventoryPanel.offsetMin = inventoryPanel.offsetMin.With(x: -width);
        
        inventoryOpenPos = inventoryPanel.anchoredPosition;
        inventoryClosePos = inventoryOpenPos - Vector2.left * width;
        
        inventoryPanel.anchoredPosition = inventoryClosePos;
        
        HideInventory();
    }

    public void ShowInventory()
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryClosePanel.gameObject.SetActive(true);
        inventoryClosePanel.GetComponent<Image>().raycastTarget = true;
        
        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryOpenPos, 0.5f).setEase(LeanTweenType.easeInCubic);
        
        LeanTween.cancel(inventoryClosePanel);
        LeanTween.alpha(inventoryClosePanel, 0.8f, 0.5f).setRecursive(false);
    }
    
    public void HideInventory()
    {
        inventoryClosePanel.GetComponent<Image>().raycastTarget = false;
        
        LeanTween.cancel(inventoryClosePanel);
        LeanTween.move(inventoryPanel, inventoryClosePos, 0.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => inventoryPanel.gameObject.SetActive(false));

        LeanTween.cancel(inventoryClosePanel);
        LeanTween.alpha(inventoryClosePanel, 0, 0.5f).setRecursive(false)
            .setOnComplete(() => inventoryClosePanel.gameObject.SetActive(true));
    }
    
    private void ConfigureItemInfoPanel()
    {
        float height = Screen.height / (2 * itemInfoSlidePanel.lossyScale.y);
        itemInfoSlidePanel.offsetMax = itemInfoSlidePanel.offsetMax.With(y: height);
        
        itemInfoOpenPos = itemInfoSlidePanel.anchoredPosition;
        itemInfoClosePos = itemInfoOpenPos + Vector2.down * height;
        
        itemInfoSlidePanel.anchoredPosition = itemInfoClosePos;
        
        HideItemInfoPanel();
    }

    [Button]
    public void ShowItemInfoPanel()
    {
        itemInfoSlidePanel.gameObject.SetActive(true);
        
        LeanTween.cancel(itemInfoSlidePanel);
        LeanTween.move(itemInfoSlidePanel, itemInfoOpenPos, 0.3f).setEase(LeanTweenType.easeOutCubic);
    }

    [Button]
    public void HideItemInfoPanel()
    {
        LeanTween.cancel(itemInfoSlidePanel);
        LeanTween.move(itemInfoSlidePanel, itemInfoClosePos, 0.3f).setEase(LeanTweenType.easeInCubic)
            .setOnComplete(() => itemInfoSlidePanel.gameObject.SetActive(false));
    }
}