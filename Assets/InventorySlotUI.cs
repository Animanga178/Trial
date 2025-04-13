using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private CanvasGroup canvasGroup;

    public void UpdateSlot(int count)
    {
        countText.text = count > 1 ? "" : count.ToString();
        canvasGroup.alpha = count == 0 ? 0.5f : 1f;
    }

    public Sprite GetIconSprite()
    {
        return icon.sprite;
    }
}
