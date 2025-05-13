using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerController player) : base(player) {}
    public override void Enter() { }
    public override void Update() 
    {
        CheckOutOfBounds();

        Vector2 newDirection = player.moveAction.ReadValue<Vector2>();

        // Normalize diagonal movement
        if (newDirection.x != 0 && newDirection.y != 0)
        {
            newDirection.y = 1;
        }
        player.direction = newDirection;

        // Update direction
        if (player.direction.x < 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.direction.x > 0)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }

        // Update movement
        player.timeSinceLastStep += Time.deltaTime;

        if (player.timeSinceLastStep >= player.timeStep && player.direction != Vector2.zero)
        {
            player.timeSinceLastStep = 0f;

            if (player.direction.y < 0 && player.transform.position.y < player.cameraController.BottomEdge + 1f)
            {
                return;
            }

            player.Move(player.direction);
        }
    }

    private void CheckOutOfBounds()
    {
        float buffer = 0.1f;
        if (player.transform.position.x < player.cameraController.LeftEdge - buffer ||
            player.transform.position.x > player.cameraController.RightEdge + buffer)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.carHitSFX);
            player.Death("Out of bounds");
        }
    }

    public override void Exit() { }
}
