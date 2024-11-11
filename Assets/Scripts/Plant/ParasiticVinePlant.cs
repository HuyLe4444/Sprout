using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParasiticVine", menuName = "Garden/Plants/ParasiticVine")]
public class ParasiticVinePlant : PlantSO
{
    public ParasiticVinePlant()
    {
        plantName = "Dây Leo Ký Sinh";
        rarity = PlantRarity.Epic;
        plantType = PlantType.Parasitic;
        baseResourceValue = 0;
        passiveDescription = "Lấy 50% điểm lương thực của các cây trong cùng cột";
        affectsColumn = true;
        
        OnPassiveTriggered = (source, affectedPlants) =>
        {
            Debug.Log($"[Dây Leo Ký Sinh] Bắt đầu ký sinh {affectedPlants.Length} cây trong cột");
            int stolenResources = 0;
            foreach (var plant in affectedPlants)
            {
                int amountToSteal = plant.currentResourceValue / 2;
                Debug.Log($"[Dây Leo Ký Sinh] Ký sinh {amountToSteal} điểm từ {plant.plantData.plantName}");
                plant.ModifyResourceValue(source, -amountToSteal);
                stolenResources += amountToSteal;
            }
            if (stolenResources > 0)
            {
                Debug.Log($"[Dây Leo Ký Sinh] Tổng cộng ký sinh được {stolenResources} điểm");
                source.ModifyResourceValue(source, stolenResources);
            }
        };
    }
}