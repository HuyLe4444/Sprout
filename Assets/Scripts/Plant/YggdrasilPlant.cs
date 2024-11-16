using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Yggdrasil", menuName = "Garden/Plants/Yggdrasil")]
public class YggdrasilPlant : PlantSO
{
   public YggdrasilPlant()
   {
       plantName = "Yggdrasil";
       rarity = PlantRarity.Epic;
       plantType = PlantType.Mythical;
       baseResourceValue = 12;
       passiveDescription = "World Tree - Doubles the resource points of all plants in the same row and column";
       affectsRow = true;
       affectsColumn = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               int currentPoints = plant.currentResourceValue;
               plant.ModifyResourceValue(source, currentPoints);
           }
       };
   }
}