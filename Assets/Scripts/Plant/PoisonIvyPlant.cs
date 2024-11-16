using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonIvy", menuName = "Garden/Plants/PoisonIvy")]
public class PoisonIvyPlant : PlantSO
{
   public PoisonIvyPlant()
   {
       plantName = "Poison Ivy";
       rarity = PlantRarity.Common;
       plantType = PlantType.Medicinal;
       baseResourceValue = 3;
       passiveDescription = "Toxin - Reduces resource points of non-Medicinal plants within 1 adjacent tile by 1";
       affectsAdjacentTiles = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType != PlantType.Medicinal)
               {
                   plant.ModifyResourceValue(source, -1);
               }
           }
       };
   }
}