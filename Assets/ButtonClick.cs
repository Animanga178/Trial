using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
