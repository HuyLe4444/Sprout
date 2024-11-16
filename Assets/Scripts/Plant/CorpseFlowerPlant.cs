using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CorpseFlower", menuName = "Garden/Plants/CorpseFlower")]
public class CorpseFlowerPlant : PlantSO
{
    public CorpseFlowerPlant()
    {
        plantName = "Corpse Flower";
        rarity = PlantRarity.Epic;
        plantType = PlantType.Carnivorous;
        baseResourceValue = 9;
        passiveDescription = "Carrion Attraction - Doubles the resource points of all Carnivorous plants in the same column";
        affectsColumn = true;

        OnPassiveTriggered = (source, affectedPlants) =>
        {
            foreach (var plant in affectedPlants)
            {
                if (plant.plantData.plantType == PlantType.Carnivorous)
                {
                    int currentPoints = plant.currentResourceValue;
                    plant.ModifyResourceValue(source, currentPoints);
                }
            }
        };
    }
}