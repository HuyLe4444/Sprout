using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTrack : MonoBehaviour
{
    public static GameTrack Instance { get; private set; }
    
    [Header("UI References")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI totalResourceText;
    
    [Header("Game Settings")]
    public int targetResource = 1000;
    public int maxDays = 30;
    
    private int currentDay = 0;
    private int totalResourceValue = 0;
    private MainInventoryManager mainInventory;
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        mainInventory = FindObjectOfType<MainInventoryManager>();
        UpdateUI();
    }
    
    public void AdvanceDay()
    {
        // Calculate total resource value from all plants
        CalculateTotalResource();
        
        // Check win/loss condition
        if (totalResourceValue >= targetResource)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        currentDay++;
        if (currentDay > maxDays)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        
        UpdateUI();
    }
    
    private void CalculateTotalResource()
    {
        totalResourceValue = 0;
        InventorySlot[] slots = mainInventory.GetInventorySlots();
        
        foreach (InventorySlot slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
                if (item != null)
                {
                    totalResourceValue += item.GetCurrentResourceValue();
                }
            }
        }
        
        LogManager.LogMessage($"Day {currentDay}: Total Resource = {totalResourceValue}");
    }
    
    private void UpdateUI()
    {
        if (dayText != null)
        {
            dayText.text = $"Day: {currentDay}/{maxDays}";
        }
        
        if (totalResourceText != null)
        {
            totalResourceText.text = $"Total Resource: {totalResourceValue}/{targetResource}";
        }
    }
    
    // Helper methods to access game state
    public int GetCurrentDay() => currentDay;
    public int GetTotalResource() => totalResourceValue;
    public bool IsGameOver() => currentDay > maxDays || totalResourceValue >= targetResource;
}