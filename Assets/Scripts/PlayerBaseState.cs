using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerController player;

    public PlayerBaseState(PlayerController player)
    {
        this.player = player;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}