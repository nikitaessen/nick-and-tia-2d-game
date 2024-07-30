using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public event Action DamageTaken;
    public event Action PillCollected;

    private Rigidbody2D _rigidbody;
    private bool _isStopped;

    public void Initialize()
    {
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var inputVector = context.ReadValue<Vector2>();
        _rigidbody.velocity = inputVector * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            DamageTaken?.Invoke();
        }

        if (other.CompareTag("Pill"))
        {
            PillCollected?.Invoke();
        }
    }
}