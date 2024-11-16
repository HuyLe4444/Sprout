using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JadeVine", menuName = "Garden/Plants/JadeVine")]
public class JadeVinePlant : PlantSO
{
    public JadeVinePlant()
    {
        plantName = "Jade Vine";
        rarity = PlantRarity.Uncommon;
        plantType = PlantType.Parasitic;
        baseResourceValue = 4;
        passiveDescription = "Stranglehold - Reduces resource points of the plant with the highest points within 2 adjacent tiles by 25%";
        affectsAdjacentTiles = true;

        OnPassiveTriggered = (source, affectedPlants) =>
        {
            if (affectedPlants.Length == 0) return;

            // Find plant with highest resource points
            InventoryItem highestValuePlant = affectedPlants[0];
            foreach (var plant in affectedPlants)
            {
                if (plant.currentResourceValue > highestValuePlant.currentResourceValue)
                {
                    highestValuePlant = plant;
                }
            }

            // Calculate points to reduce (25%)
            int pointsToReduce = Mathf.RoundToInt(highestValuePlant.currentResourceValue * 0.25f);
            
            // Apply the effect
            highestValuePlant.ModifyResourceValue(source, -pointsToReduce);
        };
    }
}