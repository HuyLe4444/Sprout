using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Baobab", menuName = "Garden/Plants/Baobab")]
public class BaobabPlant : PlantSO
{
    public BaobabPlant()
    {
        plantName = "Baobab";
        rarity = PlantRarity.Epic;
        plantType = PlantType.Mythical;
        baseResourceValue = 10;
        passiveDescription = "Ancient Wisdom - Adds 3 resource points to each plant in the same row and column";
        affectsRow = true;
        affectsColumn = true;

        OnPassiveTriggered = (source, affectedPlants) =>
        {
            foreach (var plant in affectedPlants)
            {
                plant.ModifyResourceValue(source, 3);
            }
        };
    }
}