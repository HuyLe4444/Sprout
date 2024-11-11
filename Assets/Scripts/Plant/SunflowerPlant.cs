using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sunflower", menuName = "Garden/Plants/Sunflower")]
public class SunflowerPlant : PlantSO
{
    public SunflowerPlant()
    {
        plantName = "Hướng Dương";
        rarity = PlantRarity.Common;
        plantType = PlantType.Fruit;
        baseResourceValue = 3;
        passiveDescription = "Tăng 1 điểm lương thực cho tất cả cây trong cùng hàng";
        affectsRow = true;
        
        OnPassiveTriggered = (source, affectedPlants) =>
        {
            Debug.Log($"[Hướng Dương] Bắt đầu tăng điểm cho {affectedPlants.Length} cây trong hàng");
            foreach (var plant in affectedPlants)
            {
                Debug.Log($"[Hướng Dương] Đang buff cho {plant.plantData.plantName}");
                plant.ModifyResourceValue(source, 1);
            }
        };
    }
}