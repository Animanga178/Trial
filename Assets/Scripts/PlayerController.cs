using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum PlayerState { Idle, Leap, Dead }

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
    public BlackOutDebuff BlackOutDebuffInstance { get; set; }

    // State variables
    private Vector3 spawnPosition;
    private Vector2 direction = Vector2.zero;
    private float timeSinceLastStep = 0f;
    private float furthestRow;
    public static LayerMask barrierLayer;
    private bool isCollidingWithBarrier = false;
    private bool isCollidingWithPlatform = false;
    private bool isCollidingWithObstacle = false;
    private bool isCollidingWithWater = false;
    private bool isInWater = false;
    private bool isInvincible = false;
    private PlayerState currentState;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        barrierLayer = LayerMask.GetMask("Barrier");
        spawnPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        cameraController = GameObject.FindFirstObjectByType<CameraController>();
        Transition(PlayerState.Idle);
    }

    public void SetInvincibility(bool value)
    {
        isInvincible = value;
    }

    private void FixedUpdate()
    {
        if (isCollidingWithPlatform && obstacleRigidbody != null)
        {
            var movement = obstacleRigidbody.GetComponent<ObstacleMovement>();
            bool isFrozen = movement != null && movement.GetIsFrozen();

            rb.linearVelocity = isFrozen
                ? new Vector2(0, rb.linearVelocity.y)
                : new Vector2(obstacleRigidbody.linearVelocity.x, rb.linearVelocity.y);

            if (isFrozen && isInWater)
            {
                return;
            }
        }

        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            //if (isInWater && !isCollidingWithPlatform && !isInvincible)
            //{
            //    Death("Water");
            //}
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                HandleIdleState();
                CheckOutOfBounds();
                break;
        }   
    }

    private void HandleIdleState()
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

        if (timeSinceLastStep >= timeStep && direction != Vector2.zero)
        {
            timeSinceLastStep = 0f;

            if (direction.y < 0 && transform.position.y < cameraController.BottomEdge + 1f)
            {
                return;
            }

            Move(direction);
        }
    }

    private void CheckOutOfBounds()
    {
        float buffer = 0.1f;

        if (transform.position.x < cameraController.LeftEdge - buffer ||
            transform.position.x > cameraController.RightEdge + buffer)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.carHitSFX);
            Death("Out of bounds");
        }
    }

    public void Move(Vector2 direction)
    {
        if (currentState == PlayerState.Dead || currentState == PlayerState.Leap) return;

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
        Transition(PlayerState.Leap);
        animator.SetTrigger("Leap");
        AudioManager.Instance.PlaySFX(AudioManager.Instance.leapSound);

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

        yield return new WaitForEndOfFrame();

        if (isInWater && !isCollidingWithPlatform)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.waterDeathSFX);
            Death("Water");
        }
        else
        {
            Transition(PlayerState.Idle);
        }
    }

    private void Transition(PlayerState newState)
    {
        currentState = newState;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            obstacleRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            isCollidingWithPlatform = true;
            ObstacleMovement movement = obstacleRigidbody.GetComponent<ObstacleMovement>();
            if (movement != null && movement.GetIsFrozen())
            {
                isInWater = false;
            }
        }
    }

    private bool IsLayer(Collider2D collider, string layerName)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer(layerName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollidingWithBarrier = IsLayer(collision, "Barrier");
        isCollidingWithPlatform = IsLayer(collision, "Platform");
        isCollidingWithObstacle = IsLayer(collision, "Obstacle");
        isCollidingWithWater = IsLayer(collision, "Water");

        if (isCollidingWithPlatform)
        {
            obstacleRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        }

        if (isCollidingWithObstacle)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.carHitSFX);
            Death("Obstacle");
        }

        if (isCollidingWithWater)
        {
            isInWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsLayer(collision, "Platform"))
        {
            obstacleRigidbody = null;
        }

        if (IsLayer(collision, "Water"))
        {
            isInWater = false;
        }

        isCollidingWithPlatform = false;
        isCollidingWithBarrier = false;
        isCollidingWithObstacle = false;
        isCollidingWithWater = false;
    }

    public void Death(string cause)
    {
        if (currentState == PlayerState.Dead || isInvincible) return;

        Debug.Log("Player died from: " + cause);
        GameManager.Instance.UnfreezeObstacles();
        BlackOutDebuffInstance?.LightsOn();
        StopAllCoroutines();
        animator.SetTrigger("Death");
        Transition(PlayerState.Dead);

        GameManager.Instance.PlayerDeath();
    }

    public void Respawn()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.respawnSFX);
        GameManager.Instance.UnfreezeObstacles();
        BlackOutDebuffInstance?.LightsOn();
        StopAllCoroutines();
        transform.position = spawnPosition;
        furthestRow = spawnPosition.y;
        gameObject.SetActive(true);
        Transition(PlayerState.Idle);
    }
}