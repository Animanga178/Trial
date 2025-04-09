using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

//public enum PlayerState { Idle, Leaping, Dead }

public class PlayerController : MonoBehaviour
{
    // Movement settings
    [SerializeField] private float timeStep = 0.2f;
    [SerializeField] private float leapDuration = 0.125f;

    // References
    private Animator animator;
    private Rigidbody2D rb;
    private InputAction moveAction;
    private CameraController cameraController;
    private Rigidbody2D obstacleRigidbody;


    // State variables
    private Vector3 spawnPosition;
    private Vector2 direction = Vector2.zero;
    private Vector2 lastPosition;
    private float timeSinceLastStep = 0f;
    private float furthestRow;
    public static LayerMask barrierLayer;
    private bool isCollidingWithBarrier = false;
    private bool isCollidingWithPlatform = false;
    private bool isCollidingWithObstacle = false;
    private bool hasDiedFromBounds = true;
    private bool isInWater = false;
    //private PlayerState currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        barrierLayer = LayerMask.GetMask("Barrier");
        spawnPosition = transform.position;
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        cameraController = GameObject.FindFirstObjectByType<CameraController>();
    }
    private void FixedUpdate()
    {
        if (isCollidingWithPlatform && obstacleRigidbody != null)
        {
            rb.linearVelocity = new Vector2(obstacleRigidbody.linearVelocity.x, rb.linearVelocity.y);
        }
        else if (!isCollidingWithPlatform)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void Update()
    {
        Vector2 newDirection = moveAction.ReadValue<Vector2>();

        // Normalize diagonal movement
        if (newDirection.x != 0 && newDirection.y != 0)
        {
            newDirection.y = 1;
        }
        direction = newDirection;

        // Update direction
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Update movement
        timeSinceLastStep += Time.deltaTime;
        float buffer = 0.1f;

        if (timeSinceLastStep >= timeStep && direction != Vector2.zero)
        {
            timeSinceLastStep = 0f;

            if (direction.y < 0 && transform.position.y < cameraController.BottomEdge + 1f)
            {
                return;
            }

            Move(direction);
        }
         
        if (!hasDiedFromBounds)
        {
            if (transform.position.x < cameraController.LeftEdge - buffer || transform.position.x > cameraController.RightEdge + buffer)
            {
                hasDiedFromBounds = true;
                Death();
            }
        }
    }

    public void Move(Vector2 direction)
    {
        if (isCollidingWithBarrier)
        {
            PopOutDirection();
        }
        else
        {
            if (rb.position.y > furthestRow)
            {
                furthestRow = rb.position.y;
                GameManager.Instance.MovedUp();
            }

            StartCoroutine(Leap(direction));
        }
    }

    public void PopOutDirection()
    {
        if (direction.y > 0 && IsCollidingWithGridBackground())
        {
            rb.position += Vector2.down;
        }
        else
        {
            Vector2 popOutDirection = rb.position.x < 0 ? Vector2.right : Vector2.left;
            rb.position += popOutDirection;
        }
    }

    private bool IsCollidingWithGridBackground()
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(rb.position);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.name == "Stay Still")
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator Leap(Vector2 direction)
    {
        // Trigger leap animation
        animator.SetTrigger("Leap");

        Vector3 destination = rb.position + new Vector2(direction.x, direction.y);
        Vector3 startPosition = rb.position;
        float elapsed = 0f;

        while (elapsed < leapDuration)
        {
            float t = elapsed / leapDuration;
            rb.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.position = destination;

        if (isInWater && rb.linearVelocity.x == 0)
        {
            Debug.Log("Player landed in water after leap");
            Death();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            obstacleRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            isCollidingWithPlatform = true;
            rb.linearVelocity = new Vector2(obstacleRigidbody.linearVelocity.x, rb.linearVelocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollidingWithBarrier = (collision.gameObject.layer == LayerMask.NameToLayer("Barrier"));
        isCollidingWithPlatform = (collision.gameObject.layer == LayerMask.NameToLayer("Platform"));
        isCollidingWithObstacle = (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"));

        if (isCollidingWithPlatform)
        {
            Debug.Log("Player is on top of Platform");
            obstacleRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        }

        Debug.Log("Player collided with: " + collision.gameObject.name);
        // TODO: set players position to old position

        if (isCollidingWithObstacle)
        {
            Death();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isInWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            obstacleRigidbody = null;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isInWater = false;
        }

        isCollidingWithBarrier = false;
        isCollidingWithPlatform = false;
        isCollidingWithObstacle = false;
    }

    public void Death()
    {
        StopAllCoroutines();
        animator.SetTrigger("Death");

        GameManager.Instance.PlayerDeath();
    }

    public void Respawn()
    {
        StopAllCoroutines();
        transform.position = spawnPosition;
        furthestRow = spawnPosition.y;
        gameObject.SetActive(true);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Player is colliding with: " + collision.collider.name);
    //}
}