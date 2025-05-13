using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // Movement settings
    public float timeStep = 0.2f;
    [SerializeField] private float leapDuration = 0.125f;

    // References
    private Animator animator;
    private Rigidbody2D rb;
    public InputAction moveAction;
    public CameraController cameraController;
    private Rigidbody2D obstacleRigidbody;
    public BlackOutDebuff BlackOutDebuffInstance { get; set; }

    // State variables
    private Vector3 spawnPosition;
    public Vector2 direction = Vector2.zero;
    public float timeSinceLastStep = 0f;
    private float furthestRow;
    public static LayerMask barrierLayer;
    private bool isCollidingWithBarrier = false;
    private bool isCollidingWithPlatform = false;
    private bool isCollidingWithObstacle = false;
    private bool isCollidingWithWater = false;
    private bool isInWater = false;
    private bool isInvincible = false;
    private PlayerBaseState currentState;
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
        SetState(new IdleState(this));
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

    public void Move(Vector2 direction)
    {
        if (currentState is DeadState || currentState is LeapState) return;

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
        SetState(new LeapState(this));
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
            SetState(new IdleState(this));
        }
    }

    private void SetState(PlayerBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    private void Update()
    {
        currentState?.Update();
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
        if (currentState is DeadState || isInvincible) return;

        Debug.Log("Player died from: " + cause);
        GameManager.Instance.UnfreezeObstacles();
        BlackOutDebuffInstance?.LightsOn();
        StopAllCoroutines();
        animator.SetTrigger("Death");
        SetState(newState: new DeadState(this));

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
        SetState(new IdleState(this));
    }
}