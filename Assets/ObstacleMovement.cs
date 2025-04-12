using UnityEngine;

// Controls the movement of the obstacle
public class ObstacleMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed = 10f;
    private new BoxCollider2D collider;
    private bool isFrozen = false;
    public static bool GlobalFreeze = false;
    private float originalSpeed;
    private const float frozenSpeed = 0.001f;

    private void Start()
    {
        rb = rb ?? GetComponent<Rigidbody2D>();
        collider = collider ?? GetComponent<BoxCollider2D>();
        originalSpeed = speed;
    }

    private void FixedUpdate()
    {
        if (!isFrozen)
        {
            rb.linearVelocity = direction * speed;
            CheckForBarrierCollision(collider);
        }
    }

    public void Freeze()
    {
        if (!isFrozen)
        {
            isFrozen = true;
            originalSpeed = speed;
            speed = frozenSpeed;
            rb.linearVelocity = direction * speed;
            Debug.Log($"{gameObject.name} frozen");
        }
    }

    public void Unfreeze()
    {
        if (isFrozen)
        {
            isFrozen = false;
            speed = originalSpeed;
            rb.linearVelocity = direction * speed;
            Debug.Log($"{gameObject.name} unfrozen");
        }
    }

    private void CheckForBarrierCollision(BoxCollider2D collider)
    {
        Vector2 lastEdge = GetLastEdge(collider);

        // Get the barrier's position based on the direction
        GameObject barrier = direction.x > 0 ? GameObject.Find("RightBarrier") : GameObject.Find("LeftBarrier");

        if (barrier != null)
        {
            Vector2 barrierPosition = barrier.transform.position;

            if ((direction.x > 0 && lastEdge.x >= barrierPosition.x) ||
                (direction.x < 0 && lastEdge.x <= barrierPosition.x))
            {
                Destroy(gameObject);
            }
        }
    }

    private Vector2 GetLastEdge(BoxCollider2D collider)
    {
        Vector2 bottom_left = collider.bounds.min;
        Vector2 top_right = collider.bounds.max;

        return direction.x > 0 ? bottom_left : top_right;
    }

    public bool IsOnWater()
    {
        return CompareTag("OnWater");
    }

    public bool GetIsFrozen()
    {
        return isFrozen;
    }
}
