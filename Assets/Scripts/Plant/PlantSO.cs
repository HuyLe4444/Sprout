using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Định nghĩa các enum
public enum PlantRarity
{
    Common,     // Phổ biến
    Uncommon,   // Không phổ biến
    Rare,       // Hiếm
    Epic,       // Cực hiếm
    Legendary   // Huyền thoại
}

public enum PlantType
{
    Carnivorous,    // Cây ăn thịt
    Fruit,          // Cây ăn quả
    Medicinal,      // Cây thuốc
    Parasitic,      // Cây ký sinh
    Mythical        // Cây thần thoại
}

[CreateAssetMenu(fileName = "New Plant", menuName = "Garden/Plant")]
public class PlantSO : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string plantName;
    public Sprite plantSprite;
    public PlantRarity rarity;
    public PlantType plantType;
    
    [Header("Thuộc tính")]
    public int baseResourceValue;    // Giá trị lương thực cơ bản
    public string passiveDescription; // Mô tả nội tại
    
    [Header("Vị trí ảnh hưởng")]
    public bool affectsAdjacentTiles = false;
    public bool affectsRow = false;
    public bool affectsColumn = false;
    public bool affectsDiagonal = false;
    
    // Delegate để xử lý các hiệu ứng nội tại
    public delegate void PassiveEffect(PlantController source, PlantController[] affectedPlants);
    public PassiveEffect OnPassiveTriggered;
}
