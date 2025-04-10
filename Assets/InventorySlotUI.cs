using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshPro countText;
    [SerializeField] private CanvasGroup canvasGroup;

    private void UpdateSlot(Sprite sprite, int count)
    {
        icon.sprite = sprite;
        countText.text = count > 1 ? count.ToString() : "";
        canvasGroup.alpha = count == 0 ? 0.5f : 1f;
    }
}
