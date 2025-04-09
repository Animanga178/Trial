using System.Threading;
using UnityEngine;

// Controls the movement of the obstacle
public class ObstacleMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed = 10f;
    private new BoxCollider2D collider;

    private void Start()
    {
        rb = rb ?? GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (rb == null || collider == null)
        {
            return;
        }
        //Vector2 targetPosition = (Vector2)transform.position + direction * speed * Time.deltaTime;
        //rb.MovePosition(targetPosition);
        rb.linearVelocity = direction * speed;

        CheckForBarrierCollision(collider);
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
                Debug.Log("Destroying obstacle");
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

    private void OnDrawGizmos()
    {
        Vector2 lastEdge = GetLastEdge(collider);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lastEdge, 0.1f);
    }
}
