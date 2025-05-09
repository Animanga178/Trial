using UnityEngine;

public abstract class PowerUpCommand
{
    public abstract void Execute(PlayerController player);
}

public enum PowerUpType
{
    Freeze,
    Invincibility,
    LuckyDip,
    BlackOut
}