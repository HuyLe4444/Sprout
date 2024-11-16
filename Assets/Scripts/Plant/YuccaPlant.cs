using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Yucca", menuName = "Garden/Plants/Yucca")]
public class YuccaPlant : PlantSO
{
    public YuccaPlant()
    {
        plantName = "Yucca";
        rarity = PlantRarity.Rare;
        plantType = PlantType.Medicinal;
        baseResourceValue = 6;
        passiveDescription = "Desert Strength - Adds 1 resource point for each Medicinal plant within 2 adjacent tiles. If no Medicinal plants are nearby, gains 3 resource points instead";
        affectsAdjacentTiles = true;

        OnPassiveTriggered = (source, affectedPlants) =>
        {
            int medicinalCount = 0;
            
            foreach (var plant in affectedPlants)
            {
                if (plant.plantData.plantType == PlantType.Medicinal)
                {
                    medicinalCount++;
                }
            }

            // If no Medicinal plants nearby, gain 3 points
            if (medicinalCount == 0)
            {
                source.ModifyResourceValue(source, 3);
            }
            // Otherwise gain 1 point per Medicinal plant
            else
            {
                source.ModifyResourceValue(source, medicinalCount);
            }
        };
    }
}