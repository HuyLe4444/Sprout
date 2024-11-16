using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class MainInventoryManager : MonoBehaviour
{
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private Button calculateButton;
    [SerializeField] private TextMeshProUGUI outputTextBox;
    [Header("Game Settings")]
    public int targetResource = 3000;
    public int maxDays = 30;
    [Header("UI References")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI totalResourceText;



    private int currentDay = 0;
    private int totalResourcesAccumulated = 0;

    private void Start()
    {
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            inventorySlots = GetComponentsInChildren<InventorySlot>();
        }

        if (calculateButton != null)
        {
            calculateButton.onClick.AddListener(CalculateTotalResources);
        }
        else
        {
            LogManager.LogMessage("Calculate button not assigned to MainInventoryManager!");
        }

        LogManager.Initialize(outputTextBox);

        UpdateDayText();
        UpdateTotalResourceText();
    }

    private void Update()
    {
        if(totalResourcesAccumulated >= targetResource && currentDay < maxDays) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } 

        if (currentDay >= maxDays && totalResourcesAccumulated < targetResource) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }

    private void UpdateDayText()
    {
        if (dayText != null)
        {
            dayText.text = $"Day: {currentDay}";
        }
    }

    private void UpdateTotalResourceText()
    {
        if (totalResourceText != null)
        {
            totalResourceText.text = $"Total Resources: {totalResourcesAccumulated}/{targetResource}";
        }
    }

    public InventorySlot[] GetInventorySlots()
    {
        return inventorySlots;
    }

    public void CalculateTotalResources()
    {
        // 1. Reset tất cả các cây về giá trị gốc
        LogManager.ClearLog();
        // LogManager.LogMessage("===== STARTING NEW CALCULATION =====");
        
        List<InventoryItem> allPlants = new List<InventoryItem>();
        
        // Thu thập tất cả các cây và reset về giá trị gốc
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
                if (item != null)
                {
                    allPlants.Add(item);
                    item.ResetToBaseValue();
                }
            }
        }

        // LogManager.LogMessage($"Found {allPlants.Count} plants in the grid");
        // LogManager.LogMessage("");

        // 2. Tính tổng điểm cơ bản (chưa có passive)
        int baseTotal = 0;
        // LogManager.LogMessage("Base Resource Values:");
        foreach (var item in allPlants)
        {
            int baseValue = item.GetCurrentResourceValue();
            baseTotal += item.GetCurrentResourceValue();
            // LogManager.LogMessage($"• {item.plantData.plantName}: {baseValue} points");
        }
        // LogManager.LogMessage($"Base Total (without passives): {baseTotal}");
        // LogManager.LogMessage("");

        // 3. Kích hoạt passive của từng cây
        LogManager.LogMessage("===== ACTIVATING PASSIVE EFFECTS =====");
        foreach (var item in allPlants)
        {
            if (item.plantData != null)
            {
                // LogManager.LogMessage($"Activating {item.plantData.plantName}'s passive effect...");
                item.ApplyPassiveEffect();
            }
        }
        LogManager.LogMessage("");

        // 4. Tính tổng điểm cuối cùng
        int finalTotal = 0;
        LogManager.LogMessage("Final Resource Values:");
        foreach (var item in allPlants)
        {
            int finalValue = item.GetCurrentResourceValue();
            finalTotal += finalValue;
            LogManager.LogMessage($"• {item.plantData.plantName}: +{finalValue} points");
        }

        // Update the accumulated resources
        totalResourcesAccumulated += finalTotal;
        
        // Increment the day counter
        currentDay++;
        
        // Update UI
        UpdateDayText();
        UpdateTotalResourceText();

        LogManager.LogMessage("");
        LogManager.LogMessage($"Final Total (with passives): {finalTotal}");
        LogManager.LogMessage($"Total Passive Bonus: {finalTotal - baseTotal}");
        LogManager.LogMessage("=====================================");
    }
}