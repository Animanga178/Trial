using UnityEngine;

public class DeadState : PlayerBaseState
{
    public DeadState(PlayerController player) : base(player) { }
    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}
