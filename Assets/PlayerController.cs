using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float timeStep = 0.2f;
    private float timeSinceLastStep = 0f;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb;
    private InputAction moveAction;
    private bool isCollidingWithBarrier = false;
    //private float movementDistance = 1f;
    //private float scaledVelocity;
    //private Vector2 previousPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //scaledVelocity = movementDistance / timeStep;
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 newDirection = moveAction.ReadValue<Vector2>();

        if (moveAction.ReadValue<Vector2>().x != 0 && moveAction.ReadValue<Vector2>().y != 0)
        {
            newDirection.y = 1;
        }
        else if (newDirection != direction)
        {
            direction = newDirection;
        }

        timeSinceLastStep += Time.deltaTime;

        if (timeSinceLastStep >= timeStep && direction != Vector2.zero)
        {
            timeSinceLastStep = 0f;
            Move(direction);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void Move(Vector2 direction)
    {
        if (!isCollidingWithBarrier)
        {
            rb.position += new Vector2(direction.x, direction.y);
            // Is it worth using velocity if rb.position workd
            //direction = direction.normalized;
            //rb.linearVelocity = direction * movementDistance / Time.deltaTime * time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Barrier"))
        {
            isCollidingWithBarrier = true;
        }
        else
        {
            isCollidingWithBarrier = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Barrier"))
        {
            isCollidingWithBarrier = false;

            Vector2 popOutDirection = collision.transform.position.x < 0 ? Vector2.right : Vector2.left;
            rb.position += popOutDirection;
        }
    }
}