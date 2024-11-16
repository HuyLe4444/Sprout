using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DurianTree", menuName = "Garden/Plants/DurianTree")]
public class DurianTreePlant : PlantSO
{
    public DurianTreePlant()
    {
        plantName = "Durian Tree";
        rarity = PlantRarity.Uncommon;
        plantType = PlantType.Fruit;
        baseResourceValue = 5;
        passiveDescription = "Pungent Aroma - Reduces resource points of non-Fruit plants within 3 adjacent tiles by 1";
        affectsAdjacentTiles = true;

        OnPassiveTriggered = (source, affectedPlants) =>
        {
            foreach (var plant in affectedPlants)
            {
                if (plant.plantData.plantType != PlantType.Fruit)
                {
                    plant.ModifyResourceValue(source, -1);
                }
            }
        };
    }
}