using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float timeStep = 0.2f;
    private float timeSinceLastStep = 0f;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb;
    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
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
            Debug.Log("New direction: " + newDirection);
        }

        timeSinceLastStep += Time.deltaTime;

        if (timeSinceLastStep >= timeStep && direction != Vector2.zero)
        {
            Debug.Log("Loop initiates");

            timeSinceLastStep = 0f;
            Move(direction);
            Debug.Log("Move done: " + rb.position.ToString());
        }
        else
        {
            Debug.Log("Vector is 0 again");
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void Move(Vector2 direction)
    {
        rb.position += new Vector2(direction.x, direction.y);
    }
}