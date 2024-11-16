using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldenAppleTree", menuName = "Garden/Plants/GoldenAppleTree")]
public class GoldenAppleTreePlant : PlantSO
{
   public GoldenAppleTreePlant()
   {
       plantName = "Golden Apple Tree";
       rarity = PlantRarity.Rare;
       plantType = PlantType.Fruit;
       baseResourceValue = 6;
       passiveDescription = "Autumn Harvest - Adds 2 resource points to all other Fruit plants in the same row";
       affectsRow = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType == PlantType.Fruit && plant != source)
               {
                   plant.ModifyResourceValue(source, 2);
               }
           }
       };
   }
}
