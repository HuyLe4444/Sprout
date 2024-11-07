using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// public class ToolbarManager : MonoBehaviour
// {
//     [Header("References")]
//     public Button nextDayButton;
//     public InventorySlot[] choiceSlots = new InventorySlot[3];
//     public GameObject inventoryItemPrefab;
    
//     [Header("Plant Data")]
//     public List<PlantSO> allPlants = new List<PlantSO>();
    
//     [System.Serializable]
//     public class RarityChance
//     {
//         public int dayThreshold;
//         public float commonChance;
//         public float uncommonChance;
//         public float rareChance;
//         public float epicChance;
//         public float legendaryChance;
//     }
    
//     public List<RarityChance> rarityChances;
    
//     private void Start()
//     {
//         if (nextDayButton != null)
//         {
//             nextDayButton.onClick.AddListener(OnNextDay);
//         }
//     }
    
//     public void OnNextDay()
//     {
//         // Clear existing choices
//         foreach (var slot in choiceSlots)
//         {
//             foreach (Transform child in slot.transform)
//             {
//                 Destroy(child.gameObject);
//             }
//         }
        
//         // Generate 3 new choices
//         for (int i = 0; i < 3; i++)
//         {
//             GenerateNewPlant(choiceSlots[i]);
//         }
//     }
    
//     private void GenerateNewPlant(InventorySlot targetSlot)
//     {
//         // Get current day from game manager or other source
//         int currentDay = 1; // Replace with actual current day
        
//         // Determine rarity based on current day
//         PlantRarity selectedRarity = GetRandomRarity(currentDay);
        
//         // Filter plants by selected rarity
//         List<PlantSO> possiblePlants = allPlants.FindAll(p => p.rarity == selectedRarity);
        
//         if (possiblePlants.Count > 0)
//         {
//             // Random select a plant from filtered list
//             PlantSO selectedPlant = possiblePlants[Random.Range(0, possiblePlants.Count)];
            
//             // Create new InventoryItem
//             GameObject newItem = Instantiate(inventoryItemPrefab, targetSlot.transform);
//             InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
            
//             // Set plant data
//             inventoryItem.plantData = selectedPlant;
            
//             // Update UI elements if needed
//             if (inventoryItem.image != null && selectedPlant.plantSprite != null)
//             {
//                 inventoryItem.image.sprite = selectedPlant.plantSprite;
//             }
//         }
//     }
    
//     private PlantRarity GetRandomRarity(int currentDay)
//     {
//         // Find appropriate rarity chances based on current day
//         RarityChance currentChances = rarityChances[0];
//         foreach (var chance in rarityChances)
//         {
//             if (currentDay >= chance.dayThreshold)
//             {
//                 currentChances = chance;
//             }
//         }
        
//         // Generate random number
//         float roll = Random.value;
//         float currentTotal = 0;
        
//         // Check against each rarity threshold
//         currentTotal += currentChances.legendaryChance;
//         if (roll < currentTotal) return PlantRarity.Legendary;
        
//         currentTotal += currentChances.epicChance;
//         if (roll < currentTotal) return PlantRarity.Epic;
        
//         currentTotal += currentChances.rareChance;
//         if (roll < currentTotal) return PlantRarity.Rare;
        
//         currentTotal += currentChances.uncommonChance;
//         if (roll < currentTotal) return PlantRarity.Uncommon;
        
//         return PlantRarity.Common;
//     }
// }

public class ToolbarManager : MonoBehaviour
{
    [Header("References")]
    public Button nextDayButton;
    public InventorySlot[] choiceSlots = new InventorySlot[3];
    public GameObject inventoryItemPrefab;
    
    [Header("Plant Data")]
    public List<PlantSO> allPlants = new List<PlantSO>();

    private void Start()
    {
        if (nextDayButton != null)
        {
            nextDayButton.onClick.AddListener(OnNextDay);
        }
    }
    
    public void OnNextDay()
    {
        // Clear existing choices
        foreach (var slot in choiceSlots)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        // Generate 3 new choices
        for (int i = 0; i < 3; i++)
        {
            GenerateNewPlant(choiceSlots[i]);
        }
    }
    
    private void GenerateNewPlant(InventorySlot targetSlot)
    {
        if (allPlants.Count > 0)
        {
            // Randomly select a plant from all available plants
            PlantSO selectedPlant = allPlants[Random.Range(0, allPlants.Count)];
            
            // Create new InventoryItem as a child of the slot
            GameObject newItem = Instantiate(inventoryItemPrefab);
            InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
            
            // Set up the inventory item
            inventoryItem.plantData = selectedPlant;
            if (inventoryItem.image != null && selectedPlant.plantSprite != null)
            {
                inventoryItem.image.sprite = selectedPlant.plantSprite;
            }

            // Set parent and position
            inventoryItem.transform.SetParent(targetSlot.transform);
            inventoryItem.transform.localPosition = Vector3.zero;
            inventoryItem.transform.localScale = Vector3.one;
            
            // Set the parentAfterDrag reference
            inventoryItem.parentAfterDrag = targetSlot.transform;
        }
    }
}