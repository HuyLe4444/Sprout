// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;

// public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
// {
//     [Header("UI")]
//     public Image image;
//     [HideInInspector] public Transform parentAfterDrag;

//     [Header("Plant Data")]
//     public PlantSO plantData;
//     private int currentResourceValue;

//     private void Start()
//     {
//         if (plantData != null)
//         {
//             // Cập nhật sprite và resource value từ plantData
//             if (image != null && plantData.plantSprite != null)
//             {
//                 image.sprite = plantData.plantSprite;
//             }
//             currentResourceValue = plantData.baseResourceValue;
//         }
//     }

//     // Các hàm drag-drop giữ nguyên như cũ
//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         image.raycastTarget = false;
//         parentAfterDrag = transform.parent;
//         transform.SetParent(transform.root);
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         transform.position = Input.mousePosition;
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         image.raycastTarget = true;
//         transform.SetParent(parentAfterDrag);
//     }

//     // Thêm các hàm để xử lý nội tại của cây
//     public void ApplyPassiveEffect()
//     {
//         if (plantData == null) return;

//         // Lấy parent slot
//         InventorySlot currentSlot = GetComponentInParent<InventorySlot>();
//         if (currentSlot == null) return;

//         // Lấy tất cả các slot bị ảnh hưởng
//         List<InventorySlot> affectedSlots = GetAffectedSlots(currentSlot);

//         // Áp dụng hiệu ứng lên các slot bị ảnh hưởng
//         foreach (var slot in affectedSlots)
//         {
//             if (slot.transform.childCount > 0)
//             {
//                 InventoryItem affectedItem = slot.GetComponentInChildren<InventoryItem>();
//                 if (affectedItem != null)
//                 {
//                     // Ví dụ về cách áp dụng hiệu ứng
//                     affectedItem.ModifyResourceValue(1);
//                 }
//             }
//         }
//     }

//     private List<InventorySlot> GetAffectedSlots(InventorySlot sourceSlot)
//     {
//         List<InventorySlot> affectedSlots = new List<InventorySlot>();
        
//         // Lấy tất cả các InventorySlot trong scene
//         InventorySlot[] allSlots = FindObjectsOfType<InventorySlot>();
        
//         // Lấy vị trí của source slot trong grid
//         int sourceIndex = System.Array.IndexOf(allSlots, sourceSlot);
//         int gridSize = 6; // Giả sử grid là 6x6
//         int row = sourceIndex / gridSize;
//         int col = sourceIndex % gridSize;

//         foreach (var slot in allSlots)
//         {
//             int targetIndex = System.Array.IndexOf(allSlots, slot);
//             int targetRow = targetIndex / gridSize;
//             int targetCol = targetIndex % gridSize;

//             // Kiểm tra các điều kiện ảnh hưởng
//             if (plantData.affectsRow && targetRow == row)
//                 affectedSlots.Add(slot);
            
//             if (plantData.affectsColumn && targetCol == col)
//                 affectedSlots.Add(slot);
            
//             if (plantData.affectsAdjacentTiles)
//             {
//                 if (Mathf.Abs(targetRow - row) <= 1 && Mathf.Abs(targetCol - col) <= 1)
//                     affectedSlots.Add(slot);
//             }
            
//             if (plantData.affectsDiagonal)
//             {
//                 if (Mathf.Abs(targetRow - row) == 1 && Mathf.Abs(targetCol - col) == 1)
//                     affectedSlots.Add(slot);
//             }
//         }

//         return affectedSlots;
//     }

//     public void ModifyResourceValue(int amount)
//     {
//         currentResourceValue += amount;
//     }

//     public int GetCurrentResourceValue()
//     {
//         return currentResourceValue;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    [Header("UI")]
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;

    [Header("Plant Data")]
    public PlantSO plantData;
    public int currentResourceValue;
    public Dictionary<InventoryItem, int> passiveModifiers = new Dictionary<InventoryItem, int>();

    private bool isInMainInventory = false;

    private void Start()
    {
        InitializePlant();
        CheckLocation();
    }

    private void InitializePlant()
    {
        if (plantData != null)
        {
            if (image != null && plantData.plantSprite != null)
            {
                image.sprite = plantData.plantSprite;
            }
            ResetToBaseValue();
        }
    }

    private void CheckLocation()
    {
        // Kiểm tra xem item có nằm trong MainInventory không
        MainInventoryManager mainInventory = GetComponentInParent<MainInventoryManager>();
        isInMainInventory = mainInventory != null;
    }

    #region Drag and Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        
        // Kiểm tra lại vị trí sau khi kéo thả
        CheckLocation();
    }
    #endregion

    #region Resource and Passive Effects
    public void ResetToBaseValue()
    {
        if (!isInMainInventory)
        {
            // Nếu không trong MainInventory, chỉ giữ giá trị gốc
            currentResourceValue = plantData != null ? plantData.baseResourceValue : 0;
            passiveModifiers.Clear();
            return;
        }

        // Reset về giá trị gốc và xóa tất cả modifier
        currentResourceValue = plantData != null ? plantData.baseResourceValue : 0;
        passiveModifiers.Clear();
        Debug.Log($"{plantData.plantName} reset về {currentResourceValue}");
    }

    public void ModifyResourceValue(InventoryItem source, int amount)
    {
        if (!isInMainInventory) return;

        if (source != null)
        {
            // Lưu modifier từ nguồn cụ thể
            passiveModifiers[source] = amount;
            
            // Tính lại tổng điểm
            RecalculateResourceValue();
            
            Debug.Log($"{plantData.plantName} nhận {amount} điểm từ {source.plantData.plantName}");
        }
    }

    private void RecalculateResourceValue()
    {
        if (!isInMainInventory)
        {
            currentResourceValue = plantData != null ? plantData.baseResourceValue : 0;
            return;
        }

        // Bắt đầu từ giá trị gốc
        currentResourceValue = plantData != null ? plantData.baseResourceValue : 0;
        
        // Cộng tất cả modifier
        foreach (var modifier in passiveModifiers)
        {
            currentResourceValue += modifier.Value;
        }
    }

    public int GetCurrentResourceValue()
    {
        return currentResourceValue;
    }

    public void ApplyPassiveEffect()
    {
        if (!isInMainInventory) return;
        if (plantData == null || plantData.OnPassiveTriggered == null) return;

        InventorySlot currentSlot = GetComponentInParent<InventorySlot>();
        if (currentSlot == null) return;

        List<InventoryItem> affectedItems = new List<InventoryItem>();
        List<InventorySlot> affectedSlots = GetAffectedSlots(currentSlot);

        foreach (var slot in affectedSlots)
        {
            if (slot.transform.childCount > 0)
            {
                InventoryItem affectedItem = slot.GetComponentInChildren<InventoryItem>();
                if (affectedItem != null && affectedItem != this)
                {
                    affectedItems.Add(affectedItem);
                }
            }
        }

        Debug.Log($"[{plantData.plantName}] Đang áp dụng passive lên {affectedItems.Count} cây");
        plantData.OnPassiveTriggered?.Invoke(this, affectedItems.ToArray());
    }

    private List<InventorySlot> GetAffectedSlots(InventorySlot sourceSlot)
    {
        List<InventorySlot> affectedSlots = new List<InventorySlot>();
        
        if (!isInMainInventory) return affectedSlots;

        MainInventoryManager mainManager = FindObjectOfType<MainInventoryManager>();
        if (mainManager == null) return affectedSlots;

        InventorySlot[] allSlots = mainManager.GetInventorySlots();
        
        int sourceIndex = System.Array.IndexOf(allSlots, sourceSlot);
        int gridSize = 6; // Assuming 6x6 grid
        int row = sourceIndex / gridSize;
        int col = sourceIndex % gridSize;

        for (int i = 0; i < allSlots.Length; i++)
        {
            int targetRow = i / gridSize;
            int targetCol = i % gridSize;

            bool shouldAffect = false;

            if (plantData.affectsRow && targetRow == row)
                shouldAffect = true;
            
            if (plantData.affectsColumn && targetCol == col)
                shouldAffect = true;
            
            if (plantData.affectsAdjacentTiles)
            {
                bool isAdjacent = Mathf.Abs(targetRow - row) <= 1 && 
                                 Mathf.Abs(targetCol - col) <= 1 &&
                                 (targetRow != row || targetCol != col);
                if (isAdjacent)
                    shouldAffect = true;
            }
            
            if (plantData.affectsDiagonal)
            {
                bool isDiagonal = Mathf.Abs(targetRow - row) == 1 && 
                                 Mathf.Abs(targetCol - col) == 1;
                if (isDiagonal)
                    shouldAffect = true;
            }

            if (shouldAffect)
            {
                affectedSlots.Add(allSlots[i]);
            }
        }

        return affectedSlots;
    }
    #endregion
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;

// public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
// {
//     [Header("UI")]
//     public Image image;
//     [HideInInspector] public Transform parentAfterDrag;

//     [Header("Plant Data")]
//     public PlantSO plantData;
//     public int currentResourceValue;
//     public Dictionary<InventoryItem, int> passiveModifiers = new Dictionary<InventoryItem, int>();

//     private void Start()
//     {
//         InitializePlant();
//     }

//     private void InitializePlant()
//     {
//         if (plantData != null)
//         {
//             if (image != null && plantData.plantSprite != null)
//             {
//                 image.sprite = plantData.plantSprite;
//             }
//             ResetToBaseValue();
//         }
//     }

//     #region Drag and Drop
//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         image.raycastTarget = false;
//         parentAfterDrag = transform.parent;
//         transform.SetParent(transform.root);
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         transform.position = Input.mousePosition;
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         image.raycastTarget = true;
//         transform.SetParent(parentAfterDrag);
//     }
//     #endregion

//     #region Resource and Passive Effects
//     public void ResetToBaseValue()
//     {
//         // Reset về giá trị gốc và xóa tất cả modifier
//         currentResourceValue = plantData != null ? plantData.baseResourceValue : 0;
//         passiveModifiers.Clear();
//         Debug.Log($"{plantData.plantName} reset về {currentResourceValue}");
//     }

//     public void ModifyResourceValue(InventoryItem source, int amount)
//     {
//         if (source != null)
//         {
//             // Lưu modifier từ nguồn cụ thể
//             passiveModifiers[source] = amount;
            
//             // Tính lại tổng điểm
//             RecalculateResourceValue();
            
//             Debug.Log($"{plantData.plantName} nhận {amount} điểm từ {source.plantData.plantName}");
//         }
//     }

//     private void RecalculateResourceValue()
//     {
//         // Bắt đầu từ giá trị gốc
//         currentResourceValue = plantData != null ? plantData.baseResourceValue : 0;
        
//         // Cộng tất cả modifier
//         foreach (var modifier in passiveModifiers)
//         {
//             currentResourceValue += modifier.Value;
//         }
//     }

//     public int GetCurrentResourceValue()
//     {
//         return currentResourceValue;
//     }

//     public void ApplyPassiveEffect()
//     {
//         if (plantData == null || plantData.OnPassiveTriggered == null) return;

//         InventorySlot currentSlot = GetComponentInParent<InventorySlot>();
//         if (currentSlot == null) return;

//         List<InventoryItem> affectedItems = new List<InventoryItem>();
//         List<InventorySlot> affectedSlots = GetAffectedSlots(currentSlot);

//         foreach (var slot in affectedSlots)
//         {
//             if (slot.transform.childCount > 0)
//             {
//                 InventoryItem affectedItem = slot.GetComponentInChildren<InventoryItem>();
//                 if (affectedItem != null && affectedItem != this)
//                 {
//                     affectedItems.Add(affectedItem);
//                 }
//             }
//         }

//         Debug.Log($"[{plantData.plantName}] Đang áp dụng passive lên {affectedItems.Count} cây");
//         plantData.OnPassiveTriggered?.Invoke(this, affectedItems.ToArray());
//     }

//     private List<InventorySlot> GetAffectedSlots(InventorySlot sourceSlot)
//     {
//         List<InventorySlot> affectedSlots = new List<InventorySlot>();
        
//         MainInventoryManager mainManager = FindObjectOfType<MainInventoryManager>();
//         if (mainManager == null) return affectedSlots;

//         InventorySlot[] allSlots = mainManager.GetInventorySlots();
        
//         int sourceIndex = System.Array.IndexOf(allSlots, sourceSlot);
//         int gridSize = 6; // Assuming 6x6 grid
//         int row = sourceIndex / gridSize;
//         int col = sourceIndex % gridSize;

//         for (int i = 0; i < allSlots.Length; i++)
//         {
//             int targetRow = i / gridSize;
//             int targetCol = i % gridSize;

//             bool shouldAffect = false;

//             if (plantData.affectsRow && targetRow == row)
//                 shouldAffect = true;
            
//             if (plantData.affectsColumn && targetCol == col)
//                 shouldAffect = true;
            
//             if (plantData.affectsAdjacentTiles)
//             {
//                 bool isAdjacent = Mathf.Abs(targetRow - row) <= 1 && 
//                                  Mathf.Abs(targetCol - col) <= 1 &&
//                                  (targetRow != row || targetCol != col);
//                 if (isAdjacent)
//                     shouldAffect = true;
//             }
            
//             if (plantData.affectsDiagonal)
//             {
//                 bool isDiagonal = Mathf.Abs(targetRow - row) == 1 && 
//                                  Mathf.Abs(targetCol - col) == 1;
//                 if (isDiagonal)
//                     shouldAffect = true;
//             }

//             if (shouldAffect)
//             {
//                 affectedSlots.Add(allSlots[i]);
//             }
//         }

//         return affectedSlots;
//     }
//     #endregion
// }