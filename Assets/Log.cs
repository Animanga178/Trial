using UnityEngine;

public class Log : MonoBehaviour
{
    public Rigidbody2D rb;

    public float minSpeed = 8f;
    public float maxSpeed = 12f;

    float speed = 1f;

    public float lifeTime = 5f;

    private void Start()
    {
        speed = Random.Range(8f, 12f);
    }
    void FixedUpdate()
    {
        Vector2 forward = new Vector2(transform.right.x, transform.right.y);
        rb.MovePosition(rb.position + forward * Time.fixedDeltaTime * speed);

        Destroy(gameObject, lifeTime);
    }
}