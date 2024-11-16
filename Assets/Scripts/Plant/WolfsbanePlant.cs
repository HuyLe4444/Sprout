using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wolfsbane", menuName = "Garden/Plants/Wolfsbane")]
public class WolfsbanePlant : PlantSO
{
   public WolfsbanePlant()
   {
       plantName = "Wolfsbane";
       rarity = PlantRarity.Uncommon;
       plantType = PlantType.Medicinal;
       baseResourceValue = 5;
       passiveDescription = "Lethal - Destroys all non-Medicinal plants within 2 adjacent tiles";
       affectsAdjacentTiles = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType != PlantType.Medicinal)
               {
                   // Destroy the plant instead of just reducing its resource value
                   Destroy(plant.gameObject);
               }
           }
       };
   }
}