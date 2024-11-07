// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections.Generic;

// public class MainInventoryManager : MonoBehaviour
// {
//     [SerializeField] private InventorySlot[] inventorySlots;
//     [SerializeField] private Button nextDayButton;

//     private void Start()
//     {
//         // Nếu slots chưa được assign, tìm chúng trong children
//         if (inventorySlots == null || inventorySlots.Length == 0)
//         {
//             inventorySlots = GetComponentsInChildren<InventorySlot>();
//             Debug.Log($"Tìm thấy {inventorySlots.Length} slots trong inventory");
//         }

//         // Set up next day button listener
//         if (nextDayButton != null)
//         {
//             nextDayButton.onClick.AddListener(ProcessNextDay);
//         }
//     }

//     // Getter cho inventory slots
//     public InventorySlot[] GetInventorySlots()
//     {
//         return inventorySlots;
//     }

//     // Xử lý khi sang ngày mới
//     public void ProcessNextDay()
//     {
//         // 1. Kích hoạt tất cả passive effects trước
//         TriggerAllPassives();
        
//         // 2. Sau đó tính toán và hiển thị tổng tài nguyên
//         CalculateTotalResources();
        
//         Debug.Log("=== Đã kết thúc ngày ===\n");
//     }

//     private void CalculateTotalResources()
//     {
//         int totalResources = 0;
//         List<string> plantDetails = new List<string>();

//         foreach (InventorySlot slot in inventorySlots)
//         {
//             if (slot.transform.childCount > 0)
//             {
//                 PlantController plant = slot.transform.GetComponentInChildren<PlantController>();
//                 // InventoryItem item = slot.transform.GetComponentInChildren<InventoryItem>();
//                 if (plant != null && plant.plantData != null)
//                 {
//                     totalResources += plant.currentResourceValue;
//                     plantDetails.Add($"{plant.plantData.plantName}: {plant.currentResourceValue} điểm");
//                 }
//             }
//         }

//         Debug.Log("\n=== Báo cáo Tài nguyên ===");
//         foreach (string detail in plantDetails)
//         {
//             Debug.Log(detail);
//         }
//         Debug.Log($"Tổng điểm lương thực: {totalResources}");
//         Debug.Log("========================\n");
//     }

//     private void TriggerAllPassives()
//     {
//         Debug.Log("\n=== Kích hoạt tất cả Passive Effects ===");
//         foreach (InventorySlot slot in inventorySlots)
//         {
//             if (slot.transform.childCount > 0)
//             {
//                 PlantController plant = slot.transform.GetComponentInChildren<PlantController>();
//                 if (plant != null && plant.plantData != null)
//                 {
//                     Debug.Log($"\nĐang xử lý passive của {plant.plantData.plantName}:");
//                     plant.TriggerPassive();
//                 }
//             }
//         }
//         Debug.Log("====================================\n");
//     }

//     // Helper method để lấy plant ở một vị trí cụ thể
//     public PlantController GetPlantAtPosition(int row, int col)
//     {
//         int index = row * 6 + col; // Đổi theo kích thước grid của bạn
//         if (index >= 0 && index < inventorySlots.Length)
//         {
//             if (inventorySlots[index].transform.childCount > 0)
//             {
//                 return inventorySlots[index].GetComponentInChildren<PlantController>();
//             }
//         }
//         return null;
//     }

//     // Helper method để kiểm tra xem một vị trí có hợp lệ không
//     public bool IsValidPosition(int row, int col)
//     {
//         int gridSize = 6; // Đổi theo kích thước grid của bạn
//         return row >= 0 && row < gridSize && col >= 0 && col < gridSize;
//     }

//     // Optional: Method để manually assign inventory slots
//     public void SetInventorySlots(InventorySlot[] slots)
//     {
//         inventorySlots = slots;
//     }

//     // Optional: Method để manually assign next day button
//     public void SetNextDayButton(Button button)
//     {
//         nextDayButton = button;
//         nextDayButton.onClick.AddListener(ProcessNextDay);
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainInventoryManager : MonoBehaviour
{
    // Reference to all inventory slots
    [SerializeField] private InventorySlot[] inventorySlots;
    
    // Reference to the calculate button
    [SerializeField] private Button calculateButton;

    private void Start()
    {
        // If slots aren't assigned in inspector, find them in children
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            inventorySlots = GetComponentsInChildren<InventorySlot>();
        }

        // Add listener to the button
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
        int totalResources = 0;

        foreach (InventorySlot slot in inventorySlots)
        {
            // Check if the slot has any children (items)
            if (slot.transform.childCount > 0)
            {
                // Get the InventoryItem component from the child
                InventoryItem item = slot.transform.GetComponentInChildren<InventoryItem>();
                
                if (item != null && item.plantData != null)
                {
                    // Add the current resource value (which may have been modified by passive effects)
                    totalResources += item.GetCurrentResourceValue();
                }
            }
        }

        Debug.Log($"Total Resource Value from all plants: {totalResources}");
    }

    // Optional: Method to manually assign inventory slots if needed
    public void SetInventorySlots(InventorySlot[] slots)
    {
        inventorySlots = slots;
    }

    // Optional: Method to manually assign calculate button if needed
    public void SetCalculateButton(Button button)
    {
        calculateButton = button;
        calculateButton.onClick.AddListener(CalculateTotalResources);
    }
}
