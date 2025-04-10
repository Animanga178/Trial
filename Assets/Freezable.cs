using System.Collections;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 storedVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Freeze()
    {
        storedVelocity = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;
    }

    public void Unfreeze()
    {
        storedVelocity = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;
    }
}