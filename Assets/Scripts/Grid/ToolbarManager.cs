using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarManager : MonoBehaviour
{
    [Header("References")]
    public Button nextDayButton;
    public InventorySlot[] choiceSlots = new InventorySlot[3];
    public GameObject inventoryItemPrefab;
    
    [Header("Plant Data")]
    public List<PlantSO> allPlants = new List<PlantSO>();
    int itemCount = 0;

    private void Start()
    {
        if (nextDayButton != null)
        {
            nextDayButton.onClick.AddListener(OnNextDay);
        }
    }

    private void Update()
    {
        itemCount = 0;
        List<GameObject> existingItems = new List<GameObject>();
        foreach (var slot in choiceSlots) {
            if (slot.transform.childCount > 0)
            {
                itemCount++;
                existingItems.Add(slot.transform.GetChild(0).gameObject);
            }
        }

        if (itemCount == 2)
        {
            foreach (var item in existingItems)
            {
                Destroy(item);
            }
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