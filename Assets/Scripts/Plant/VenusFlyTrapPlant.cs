using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VenusFlytrap", menuName = "Garden/Plants/VenusFlytrap")]
public class VenusFlyTrapPlant : PlantSO
{
    public VenusFlyTrapPlant()
    {
        plantName = "Nắp Ấm Venus";
        rarity = PlantRarity.Rare;
        plantType = PlantType.Carnivorous;
        baseResourceValue = -1;
        passiveDescription = "Hút 2 điểm lương thực từ các cây xung quanh và tăng gấp đôi điểm lương thực của chính nó";
        affectsAdjacentTiles = true;
        
        OnPassiveTriggered = (source, affectedPlants) =>
        {
            Debug.Log($"[Nắp Ấm Venus] Bắt đầu hút điểm từ {affectedPlants.Length} cây xung quanh");
            foreach (var plant in affectedPlants)
            {
                Debug.Log($"[Nắp Ấm Venus] Đang hút điểm từ {plant.plantData.plantName}");
                plant.ModifyResourceValue(-2);
                source.ModifyResourceValue(2);
            }
        };
    }
}