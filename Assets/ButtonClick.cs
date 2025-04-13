using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AudioClip compressClip, uncompressClip;
    [SerializeField] private AudioSource source;

    public void OnPointerDown(PointerEventData eventData)
    {
        source.PlayOneShot(compressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        source.PlayOneShot(uncompressClip);
    }
}
