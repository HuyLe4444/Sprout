using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StrawberryPatch", menuName = "Garden/Plants/StrawberryPatch")]
public class StrawberryPatchPlant : PlantSO 
{
   public StrawberryPatchPlant()
   {
       plantName = "Strawberry Patch";
       rarity = PlantRarity.Common;
       plantType = PlantType.Fruit;
       baseResourceValue = 4;
       passiveDescription = "Proliferation - Adds 1 resource point to each other Fruit plant on the map";
       // Since we want to affect all tiles, we set both row and column to true
       affectsRow = true;
       affectsColumn = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType == PlantType.Fruit && plant != source)
               {
                   plant.ModifyResourceValue(source, 1);
               }
           }
       };
   }
}
