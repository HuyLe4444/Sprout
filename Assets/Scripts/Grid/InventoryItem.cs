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
    private int currentResourceValue;

    private void Start()
    {
        if (plantData != null)
        {
            // Cập nhật sprite và resource value từ plantData
            if (image != null && plantData.plantSprite != null)
            {
                image.sprite = plantData.plantSprite;
            }
            currentResourceValue = plantData.baseResourceValue;
        }
    }

    // Các hàm drag-drop giữ nguyên như cũ
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
    }

    // Thêm các hàm để xử lý nội tại của cây
    public void ApplyPassiveEffect()
    {
        if (plantData == null) return;

        // Lấy parent slot
        InventorySlot currentSlot = GetComponentInParent<InventorySlot>();
        if (currentSlot == null) return;

        // Lấy tất cả các slot bị ảnh hưởng
        List<InventorySlot> affectedSlots = GetAffectedSlots(currentSlot);

        // Áp dụng hiệu ứng lên các slot bị ảnh hưởng
        foreach (var slot in affectedSlots)
        {
            if (slot.transform.childCount > 0)
            {
                InventoryItem affectedItem = slot.GetComponentInChildren<InventoryItem>();
                if (affectedItem != null)
                {
                    // Ví dụ về cách áp dụng hiệu ứng
                    affectedItem.ModifyResourceValue(1);
                }
            }
        }
    }

    private List<InventorySlot> GetAffectedSlots(InventorySlot sourceSlot)
    {
        List<InventorySlot> affectedSlots = new List<InventorySlot>();
        
        // Lấy tất cả các InventorySlot trong scene
        InventorySlot[] allSlots = FindObjectsOfType<InventorySlot>();
        
        // Lấy vị trí của source slot trong grid
        int sourceIndex = System.Array.IndexOf(allSlots, sourceSlot);
        int gridSize = 6; // Giả sử grid là 6x6
        int row = sourceIndex / gridSize;
        int col = sourceIndex % gridSize;

        foreach (var slot in allSlots)
        {
            int targetIndex = System.Array.IndexOf(allSlots, slot);
            int targetRow = targetIndex / gridSize;
            int targetCol = targetIndex % gridSize;

            // Kiểm tra các điều kiện ảnh hưởng
            if (plantData.affectsRow && targetRow == row)
                affectedSlots.Add(slot);
            
            if (plantData.affectsColumn && targetCol == col)
                affectedSlots.Add(slot);
            
            if (plantData.affectsAdjacentTiles)
            {
                if (Mathf.Abs(targetRow - row) <= 1 && Mathf.Abs(targetCol - col) <= 1)
                    affectedSlots.Add(slot);
            }
            
            if (plantData.affectsDiagonal)
            {
                if (Mathf.Abs(targetRow - row) == 1 && Mathf.Abs(targetCol - col) == 1)
                    affectedSlots.Add(slot);
            }
        }

        return affectedSlots;
    }

    public void ModifyResourceValue(int amount)
    {
        currentResourceValue += amount;
    }

    public int GetCurrentResourceValue()
    {
        return currentResourceValue;
    }
}