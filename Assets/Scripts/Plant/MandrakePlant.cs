using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mandrake", menuName = "Garden/Plants/Mandrake")]
public class MandrakePlant : PlantSO
{
   public MandrakePlant()
   {
       plantName = "Mandrake";
       rarity = PlantRarity.Epic;
       plantType = PlantType.Medicinal;
       baseResourceValue = 10;
       passiveDescription = "Scream - Reduces resource points of all non-Medicinal plants in the same column by 3";
       affectsColumn = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType != PlantType.Medicinal)
               {
                   plant.ModifyResourceValue(source, -3);
               }
           }
       };
   }
}