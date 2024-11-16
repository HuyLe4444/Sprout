using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DragonsBreath", menuName = "Garden/Plants/DragonsBreath")]
public class DragonsBreathPlant : PlantSO
{
    public DragonsBreathPlant()
    {
        plantName = "Dragon's Breath";
        rarity = PlantRarity.Legendary;
        plantType = PlantType.Carnivorous;
        baseResourceValue = 15;
        passiveDescription = "Inferno - Destroys all non-Carnivorous plants in a 2x2 area centered on this plant";
        affectsAdjacentTiles = true;

        OnPassiveTriggered = (source, affectedPlants) =>
        {
            foreach (var plant in affectedPlants)
            {
                // Check if plant is in 2x2 area and not Carnivorous
                if (plant.plantData.plantType != PlantType.Carnivorous)
                {
                    // Get the relative position to determine if it's in the 2x2 area
                    Vector3 relativePos = plant.transform.position - source.transform.position;
                    if (Mathf.Abs(relativePos.x) <= 1 && Mathf.Abs(relativePos.y) <= 1)
                    {
                        // Destroy the plant
                        GameObject.Destroy(plant.gameObject);
                    }
                }
            }
        };
    }
}