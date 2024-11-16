using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rafflesia", menuName = "Garden/Plants/Rafflesia")]
public class RafflesiaPlant : PlantSO
{
   public RafflesiaPlant()
   {
       plantName = "Rafflesia";
       rarity = PlantRarity.Legendary;
       plantType = PlantType.Parasitic;
       baseResourceValue = 15;
       passiveDescription = "Siphon - Steals 30% of the resource points from the plant with the highest points within 3 adjacent tiles";
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

           // Calculate points to steal (30%)
           int pointsToSteal = Mathf.RoundToInt(highestValuePlant.currentResourceValue * 0.3f);
           
           // Apply the effect
           highestValuePlant.ModifyResourceValue(source, -pointsToSteal);
           source.ModifyResourceValue(source, pointsToSteal);
       };
   }
}