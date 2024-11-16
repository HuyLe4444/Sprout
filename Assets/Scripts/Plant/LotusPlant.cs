using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lotus", menuName = "Garden/Plants/Lotus")]
public class LotusPlant : PlantSO
{
    public LotusPlant()
    {
        plantName = "Lotus";
        rarity = PlantRarity.Rare;
        plantType = PlantType.Mythical;
        baseResourceValue = 7;
        passiveDescription = "Enlightenment - Adds 1 resource point to each plant within 3 adjacent tiles for every 2 Mythical plants on the map";
        affectsAdjacentTiles = true;

        OnPassiveTriggered = (source, affectedPlants) =>
        {
            // Find all Mythical plants on the map
            MainInventoryManager mainManager = GameObject.FindObjectOfType<MainInventoryManager>();
            if (mainManager == null) return;

            int mythicalCount = 0;
            InventorySlot[] allSlots = mainManager.GetInventorySlots();
            
            // Count Mythical plants
            foreach (var slot in allSlots)
            {
                if (slot.transform.childCount > 0)
                {
                    InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
                    if (item != null && item.plantData.plantType == PlantType.Mythical)
                    {
                        mythicalCount++;
                    }
                }
            }

            // Calculate bonus points (1 point per 2 Mythical plants)
            int bonusPoints = mythicalCount / 2;

            // Apply bonus to each affected plant
            if (bonusPoints > 0)
            {
                foreach (var plant in affectedPlants)
                {
                    plant.ModifyResourceValue(source, bonusPoints);
                }
            }
        };
    }
}