using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VenusFlytrap", menuName = "Garden/Plants/VenusFlytrap")]
public class VenusFlyTrapPlant : PlantSO
{
   public VenusFlyTrapPlant()
   {
       plantName = "Venus Flytrap";
       rarity = PlantRarity.Rare;
       plantType = PlantType.Carnivorous;
       baseResourceValue = 8;
       passiveDescription = "Predator - Adds 2 resource points to all other Carnivorous plants within 2 adjacent tiles";
       affectsAdjacentTiles = true;

       OnPassiveTriggered = (source, affectedPlants) =>
       {
           foreach (var plant in affectedPlants)
           {
               if (plant.plantData.plantType == PlantType.Carnivorous && plant != source)
               {
                   plant.ModifyResourceValue(source, 2);
               }
           }
       };
   }
}