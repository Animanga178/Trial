using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject homePlayer;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        homePlayer.SetActive(true);
    }

    private void OnDisable()
    {
        homePlayer.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled && collision.gameObject.CompareTag("Player"))
        {
            enabled = true;
            GameManager.Instance.HomeOccupied();

        }
    }
}
