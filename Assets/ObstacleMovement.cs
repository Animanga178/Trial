using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public float speed = 10f;
    public float lifeTime = 50f;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.position = rb.position;

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}
