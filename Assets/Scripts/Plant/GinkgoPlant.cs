using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ginkgo", menuName = "Garden/Plants/Ginkgo")]
public class GinkgoPlant : PlantSO
{
   public GinkgoPlant()
   {
       plantName = "Ginkgo";
       rarity = PlantRarity.Rare;
       plantType = PlantType.Medicinal;
       baseResourceValue = 6;
       passiveDescription = "Memory Boost - Adds 2 resource points to all other Medicinal plants within 2 adjacent tiles";
       affectsColumn = true;
       affectsRow = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType == PlantType.Medicinal && plant != source)
               {
                   plant.ModifyResourceValue(source, 2);
               }
           }
       };
   }
}