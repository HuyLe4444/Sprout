using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

// public class MainInventoryManager : MonoBehaviour
// {
//     [SerializeField] private InventorySlot[] inventorySlots;
//     [SerializeField] private Button calculateButton;

//     private void Start()
//     {
//         if (inventorySlots == null || inventorySlots.Length == 0)
//         {
//             inventorySlots = GetComponentsInChildren<InventorySlot>();
//         }

//         if (calculateButton != null)
//         {
//             calculateButton.onClick.AddListener(CalculateTotalResources);
//         }
//         else
//         {
//             Debug.LogWarning("Calculate button not assigned to MainInventoryManager!");
//         }
//     }

//     public InventorySlot[] GetInventorySlots()
//     {
//         return inventorySlots;
//     }

//     public void CalculateTotalResources()
//     {
//         // Đầu tiên reset tất cả các cây về giá trị gốc
//         foreach (InventorySlot slot in inventorySlots)
//         {
//             if (slot.transform.childCount > 0)
//             {
//                 InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
//                 if (item != null)
//                 {
//                     item.ResetToBaseValue();
//                 }
//             }
//         }

//         // Sau đó kích hoạt passive của từng cây
//         foreach (InventorySlot slot in inventorySlots)
//         {
//             if (slot.transform.childCount > 0)
//             {
//                 InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
//                 if (item != null && item.plantData != null)
//                 {
//                     item.ApplyPassiveEffect();
//                     Debug.Log($"Kích hoạt passive của {item.plantData.plantName}");
//                 }
//             }
//         }

//         // Cuối cùng tính tổng
//         int totalResources = 0;
//         foreach (InventorySlot slot in inventorySlots)
//         {
//             if (slot.transform.childCount > 0)
//             {
//                 InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
//                 if (item != null)
//                 {
//                     totalResources += item.GetCurrentResourceValue();
//                     Debug.Log($"{item.plantData.plantName}: {item.GetCurrentResourceValue()} điểm");
//                 }
//             }
//         }

//         Debug.Log($"Tổng điểm: {totalResources}");
//     }
// }

public class MainInventoryManager : MonoBehaviour
{
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private Button calculateButton;

    private void Start()
    {
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            inventorySlots = GetComponentsInChildren<InventorySlot>();
        }

        if (calculateButton != null)
        {
            calculateButton.onClick.AddListener(CalculateTotalResources);
        }
        else
        {
            Debug.LogWarning("Calculate button not assigned to MainInventoryManager!");
        }
    }

    public InventorySlot[] GetInventorySlots()
    {
        return inventorySlots;
    }

    public void CalculateTotalResources()
    {
        // 1. Reset tất cả các cây về giá trị gốc
        Debug.Log("===== BẮT ĐẦU TÍNH TOÁN MỚI =====");
        
        List<InventoryItem> allPlants = new List<InventoryItem>();
        
        // Thu thập tất cả các cây và reset về giá trị gốc
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
                if (item != null)
                {
                    allPlants.Add(item);
                    item.ResetToBaseValue();
                }
            }
        }

        Debug.Log($"Tìm thấy {allPlants.Count} cây trong grid");

        // 2. Tính tổng điểm cơ bản (chưa có passive)
        int baseTotal = 0;
        foreach (var item in allPlants)
        {
            baseTotal += item.GetCurrentResourceValue();
            Debug.Log($"{item.plantData.plantName}: Điểm cơ bản {item.GetCurrentResourceValue()}");
        }
        Debug.Log($"Tổng điểm cơ bản (chưa tính passive): {baseTotal}");

        // 3. Kích hoạt passive của từng cây
        Debug.Log("===== KÍCH HOẠT PASSIVE =====");
        foreach (var item in allPlants)
        {
            if (item.plantData != null)
            {
                Debug.Log($"Đang kích hoạt passive của {item.plantData.plantName}");
                item.ApplyPassiveEffect();
            }
        }

        // 4. Tính tổng điểm cuối cùng
        int finalTotal = 0;
        Debug.Log("===== KẾT QUẢ CUỐI CÙNG =====");
        foreach (var item in allPlants)
        {
            int finalValue = item.GetCurrentResourceValue();
            finalTotal += finalValue;
            Debug.Log($"{item.plantData.plantName}: Điểm cuối cùng {finalValue}");
        }

        Debug.Log($"Tổng điểm cuối cùng (đã tính passive): {finalTotal}");
        Debug.Log("=====================================");
    }
}