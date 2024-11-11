// using UnityEngine;
// using System.Collections.Generic;

// public class PlantController : MonoBehaviour
// {
//     public PlantSO plantData;
//     private InventorySlot currentSlot;
    
//     [Header("Runtime Values")]
//     public int currentResourceValue;
//     private MainInventoryManager inventoryManager;
    
//     private void Start()
//     {
//         if (plantData != null)
//         {
//             currentResourceValue = plantData.baseResourceValue;
//             Debug.Log($"[{plantData.plantName}] Khởi tạo với {currentResourceValue} điểm lương thực");
//         }
//         inventoryManager = FindObjectOfType<MainInventoryManager>();
//     }

//     public void TriggerPassive()
//     {
//         if (plantData == null) return;

//         Debug.Log($"\n[{plantData.plantName}] Kích hoạt nội tại: {plantData.passiveDescription}");

//         // Lấy vị trí hiện tại trong grid
//         InventorySlot[] allSlots = inventoryManager.GetInventorySlots();
//         int currentIndex = GetCurrentIndex(allSlots);
//         if (currentIndex == -1) return;

//         // Tính toán vị trí row/column
//         int gridSize = 6; // hoặc 5 tùy vào setup
//         int row = currentIndex / gridSize;
//         int col = currentIndex % gridSize;
//         Debug.Log($"[{plantData.plantName}] Đang ở vị trí: Hàng {row + 1}, Cột {col + 1}");

//         List<PlantController> affectedPlants = new List<PlantController>();

//         // Xử lý các ô bị ảnh hưởng dựa trên thuộc tính của cây
//         for (int i = 0; i < allSlots.Length; i++)
//         {
//             int targetRow = i / gridSize;
//             int targetCol = i % gridSize;
//             bool shouldAffect = false;

//             // Kiểm tra các điều kiện ảnh hưởng
//             if (plantData.affectsRow && targetRow == row)
//                 shouldAffect = true;
            
//             if (plantData.affectsColumn && targetCol == col)
//                 shouldAffect = true;
            
//             if (plantData.affectsAdjacentTiles)
//             {
//                 if (Mathf.Abs(targetRow - row) <= 1 && Mathf.Abs(targetCol - col) <= 1)
//                     shouldAffect = true;
//             }
            
//             if (plantData.affectsDiagonal)
//             {
//                 if (Mathf.Abs(targetRow - row) == 1 && Mathf.Abs(targetCol - col) == 1)
//                     shouldAffect = true;
//             }

//             if (shouldAffect && allSlots[i].transform.childCount > 0)
//             {
//                 PlantController targetPlant = allSlots[i].GetComponentInChildren<PlantController>();
//                 if (targetPlant != null && targetPlant != this)
//                 {
//                     affectedPlants.Add(targetPlant);
//                     Debug.Log($"[{plantData.plantName}] Tìm thấy {targetPlant.plantData.plantName} ở Hàng {targetRow + 1}, Cột {targetCol + 1} trong vùng ảnh hưởng");
//                 }
//             }
//         }

//         // Kích hoạt passive effect
//         if (plantData.OnPassiveTriggered != null)
//         {
//             Debug.Log($"[{plantData.plantName}] Bắt đầu áp dụng hiệu ứng lên {affectedPlants.Count} cây");
//             plantData.OnPassiveTriggered(this, affectedPlants.ToArray());
//         }
//     }

//     private int GetCurrentIndex(InventorySlot[] allSlots)
//     {
//         InventorySlot mySlot = GetComponentInParent<InventorySlot>();
//         if (mySlot == null) return -1;
//         return System.Array.IndexOf(allSlots, mySlot);
//     }

//     public void ModifyResourceValue(int amount)
//     {
//         int oldValue = currentResourceValue;
//         currentResourceValue += amount;
//         string changeType = amount >= 0 ? "tăng" : "giảm";
//         Debug.Log($"[{plantData.plantName}] Điểm lương thực {changeType} từ {oldValue} thành {currentResourceValue} ({(amount >= 0 ? "+" : "")}{amount})");
//     }
// }

// using UnityEngine;
// using System.Collections.Generic;

// public class PlantController : MonoBehaviour
// {
//     public PlantSO plantData;
//     private InventorySlot currentSlot;
    
//     [Header("Runtime Values")]
//     public int currentResourceValue;
//     private MainInventoryManager inventoryManager;
    
//     private void Start()
//     {
//         if (plantData != null)
//         {
//             ResetToBaseValue();
//             Debug.Log($"[{plantData.plantName}] Khởi tạo với {currentResourceValue} điểm lương thực");
//         }
//         inventoryManager = FindObjectOfType<MainInventoryManager>();
//     }

//     // Thêm method để reset về giá trị cơ bản
//     public void ResetToBaseValue()
//     {
//         if (plantData != null)
//         {
//             currentResourceValue = plantData.baseResourceValue;
//             Debug.Log($"[{plantData.plantName}] Reset về giá trị cơ bản: {currentResourceValue}");
//         }
//     }

//     public void TriggerPassive()
//     {
//         if (plantData == null) return;

//         Debug.Log($"\n[{plantData.plantName}] Kích hoạt nội tại: {plantData.passiveDescription}");

//         // Lấy vị trí hiện tại trong grid
//         InventorySlot[] allSlots = inventoryManager.GetInventorySlots();
//         int currentIndex = GetCurrentIndex(allSlots);
//         if (currentIndex == -1) return;

//         // Tính toán vị trí row/column
//         int gridSize = 6; // hoặc 5 tùy vào setup
//         int row = currentIndex / gridSize;
//         int col = currentIndex % gridSize;
//         Debug.Log($"[{plantData.plantName}] Đang ở vị trí: Hàng {row + 1}, Cột {col + 1}");

//         List<PlantController> affectedPlants = new List<PlantController>();

//         // Xử lý các ô bị ảnh hưởng dựa trên thuộc tính của cây
//         for (int i = 0; i < allSlots.Length; i++)
//         {
//             int targetRow = i / gridSize;
//             int targetCol = i % gridSize;
//             bool shouldAffect = false;

//             // Kiểm tra các điều kiện ảnh hưởng
//             if (plantData.affectsRow && targetRow == row)
//                 shouldAffect = true;
            
//             if (plantData.affectsColumn && targetCol == col)
//                 shouldAffect = true;
            
//             if (plantData.affectsAdjacentTiles)
//             {
//                 if (Mathf.Abs(targetRow - row) <= 1 && Mathf.Abs(targetCol - col) <= 1)
//                     shouldAffect = true;
//             }
            
//             if (plantData.affectsDiagonal)
//             {
//                 if (Mathf.Abs(targetRow - row) == 1 && Mathf.Abs(targetCol - col) == 1)
//                     shouldAffect = true;
//             }

//             if (shouldAffect && allSlots[i].transform.childCount > 0)
//             {
//                 PlantController targetPlant = allSlots[i].GetComponentInChildren<PlantController>();
//                 if (targetPlant != null && targetPlant != this)
//                 {
//                     affectedPlants.Add(targetPlant);
//                     Debug.Log($"[{plantData.plantName}] Tìm thấy {targetPlant.plantData.plantName} ở Hàng {targetRow + 1}, Cột {targetCol + 1} trong vùng ảnh hưởng");
//                 }
//             }
//         }

//         // Kích hoạt passive effect
//         if (plantData.OnPassiveTriggered != null)
//         {
//             Debug.Log($"[{plantData.plantName}] Bắt đầu áp dụng hiệu ứng lên {affectedPlants.Count} cây");
//             plantData.OnPassiveTriggered(this, affectedPlants.ToArray());
//         }
//     }

//     private int GetCurrentIndex(InventorySlot[] allSlots)
//     {
//         InventorySlot mySlot = GetComponentInParent<InventorySlot>();
//         if (mySlot == null) return -1;
//         return System.Array.IndexOf(allSlots, mySlot);
//     }

//     public void ModifyResourceValue(int amount)
//     {
//         int oldValue = currentResourceValue;
//         currentResourceValue += amount;
//         string changeType = amount >= 0 ? "tăng" : "giảm";
//         Debug.Log($"[{plantData.plantName}] Điểm lương thực {changeType} từ {oldValue} thành {currentResourceValue} ({(amount >= 0 ? "+" : "")}{amount})");
//     }
// }