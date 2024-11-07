using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject highlight;
    private bool isOccupied = false;

    public void Init(bool isOffset) {
        _renderer.color = isOffset ? offsetColor : baseColor;
    }

    void OnMouseEnter() {
        if (!isOccupied) {
            highlight.SetActive(true);
        }
    }

    void OnMouseExit() {
        highlight.SetActive(false);
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
    }
}
