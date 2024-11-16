using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mistletoe", menuName = "Garden/Plants/Mistletoe")]
public class MistletoePlant : PlantSO
{
   public MistletoePlant()
   {
       plantName = "Mistletoe";
       rarity = PlantRarity.Uncommon;
       plantType = PlantType.Parasitic;
       baseResourceValue = 3;
       passiveDescription = "Parasitism - Steals 20% of the resource points from the plant with the lowest points within 1 adjacent tile";
       affectsAdjacentTiles = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           if (affectedPlants.Length == 0) return;

           // Find plant with lowest resource points
           InventoryItem lowestValuePlant = affectedPlants[0];
           foreach (var plant in affectedPlants)
           {
               if (plant.currentResourceValue < lowestValuePlant.currentResourceValue)
               {
                   lowestValuePlant = plant;
               }
           }

           // Calculate points to steal (20%)
           int pointsToSteal = Mathf.RoundToInt(lowestValuePlant.currentResourceValue * 0.2f);
           
           // Apply the effect
           lowestValuePlant.ModifyResourceValue(source, -pointsToSteal);
           source.ModifyResourceValue(source, pointsToSteal);
       };
   }
}