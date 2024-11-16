using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pineapple", menuName = "Garden/Plants/Pineapple")]
public class PineapplePlant : PlantSO
{
   public PineapplePlant()
   {
       plantName = "Pineapple";
       rarity = PlantRarity.Common;
       plantType = PlantType.Fruit;
       baseResourceValue = 4;
       passiveDescription = "Sweet Aroma - Adds 1 resource point to each Fruit plant in the same column";
       affectsColumn = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType == PlantType.Fruit)
               {
                   plant.ModifyResourceValue(source, 1);
               }
           }
       };
   }
}