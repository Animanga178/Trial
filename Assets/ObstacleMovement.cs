using UnityEngine;

// Controls the movement of the obstacle
public class ObstacleMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed = 10f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.Log("Rigidbody2D component is missing from " + gameObject.name);
            }
        }
        rb.position = transform.position;

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }
}
