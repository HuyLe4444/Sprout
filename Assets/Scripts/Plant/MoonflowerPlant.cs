using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Moonflower", menuName = "Garden/Plants/Moonflower")]
public class MoonflowerPlant : PlantSO
{
   public MoonflowerPlant()
   {
       plantName = "Moonflower";
       rarity = PlantRarity.Rare;
       plantType = PlantType.Mythical;
       baseResourceValue = 7;
       passiveDescription = "Lunar Blessing - Adds 2 resource points to each Mythical plant on the map";
       affectsRow = true;
       affectsColumn = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType == PlantType.Mythical)
               {
                   plant.ModifyResourceValue(source, 2);
               }
           }
       };
   }
}