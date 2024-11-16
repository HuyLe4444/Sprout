using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PitcherPlant", menuName = "Garden/Plants/PitcherPlant")]
public class PitcherPlant : PlantSO
{
   public PitcherPlant()
   {
       plantName = "Pitcher Plant";
       rarity = PlantRarity.Uncommon;
       plantType = PlantType.Carnivorous;
       baseResourceValue = 5;
       passiveDescription = "Digestive Fluid - Reduces resource points of non-Carnivorous plants within 2 adjacent tiles by 2";
       affectsAdjacentTiles = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType != PlantType.Carnivorous)
               {
                   plant.ModifyResourceValue(source, -2);
               }
           }
       };
   }
}