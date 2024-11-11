using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Text;

public static class LogManager
{
    private static StringBuilder logBuilder = new StringBuilder();
    private static TextMeshProUGUI outputTextBox;

    public static void Initialize(TextMeshProUGUI textBox)
    {
        outputTextBox = textBox;
    }

    public static void LogMessage(string message)
    {
        Debug.Log(message);
        logBuilder.AppendLine(message);
        if (outputTextBox != null)
        {
            outputTextBox.text = logBuilder.ToString();
        }
    }

    public static void ClearLog()
    {
        logBuilder.Clear();
        if (outputTextBox != null)
        {
            outputTextBox.text = string.Empty;
        }
    }
}

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;

    [Header("Hover UI")]
    public GameObject hoverImagePrefab;
    private GameObject currentHoverInstance;

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

    private void Update()
    {
        // Update hover image position if it exists
        if (currentHoverInstance != null)
        {
            // UpdateHoverPosition();  
        }
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

    private void OnDestroy()
    {
        // Clean up hover instance if it exists when item is destroyed
        if (currentHoverInstance != null)
        {
            Destroy(currentHoverInstance);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImagePrefab != null && currentHoverInstance == null)
        {
            // Find the Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                // Instantiate the hover image as a child of the canvas
                currentHoverInstance = Instantiate(hoverImagePrefab, canvas.transform);
                
                Image hoverImage = currentHoverInstance.GetComponentInChildren<Image>();
                // TextMeshProUGUI plantNameText = currentHoverInstance.GetComponentInChildren<TextMeshProUGUI>();
                TextMeshProUGUI[] texts = currentHoverInstance.GetComponentsInChildren<TextMeshProUGUI>();
                TextMeshProUGUI nameText = null;
                TextMeshProUGUI passiveText = null;

                foreach (TextMeshProUGUI text in texts)
                {
                    if (text.gameObject.name == "Name")
                    {
                        nameText = text;
                    }
                    else if (text.gameObject.name == "Passive")
                    {
                        passiveText = text;
                    }
                }

                if (hoverImage != null)
                {
                    hoverImage.sprite = plantData.plantSprite;
                    // Preserve aspect ratio
                    // hoverImage.preserveAspectRatio = true;
                }

                // Set the plant name
                if (nameText != null)
                {
                    nameText.text = plantData.plantName;
                }
                
                // Set the passive description
                if (passiveText != null)
                {
                    passiveText.text = plantData.passiveDescription;
                }
                
                // if (plantNameText != null)
                // {
                //     plantNameText.text = plantData.plantName;
                // }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentHoverInstance != null)
        {
            Destroy(currentHoverInstance);
            currentHoverInstance = null;
        }
    }

    private void UpdateHoverPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        currentHoverInstance.transform.position = mousePos + new Vector2(50f, 50f); // Offset to not cover the item
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
        // currentResourceValue = plantData != null ? plantData.baseResourceValue : 0;
        // passiveModifiers.Clear();
        // Debug.Log($"{plantData.plantName} reset về {currentResourceValue}");
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
            
            LogManager.LogMessage($"{plantData.plantName} received {amount} points from {source.plantData.plantName}");
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

        LogManager.LogMessage($"[{plantData.plantName}] Applying passive effect to {affectedItems.Count} plants");
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
