using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public event Action DamageTaken;
    public event Action PillCollected;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _isStopped;
    private bool _needsFlip;
    private bool _isTakingDamage;

    private Coroutine _damageCooldownCoroutine;
    private Vector2 _moveInputVector;

    private Vector2 CalculatedVelocity => _moveInputVector * speed;

    [SerializeField] private Facing facing = Facing.Left;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    public void Initialize(SoundPlayer soundPlayer)
    {
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = CalculatedVelocity;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("hello");
        _moveInputVector = context.ReadValue<Vector2>().normalized;

        switch (CalculatedVelocity.x)
        {
            case > 0 when facing == Facing.Left:
            case < 0 when facing == Facing.Right:
                Flip();
                break;
        }

        var isWalking = Math.Abs(CalculatedVelocity.x) > 0 || Math.Abs(CalculatedVelocity.y) > 0;
        _animator.SetBool(IsWalking, isWalking);
    }

    private void Flip()
    {
        facing = facing == Facing.Right ? Facing.Left : Facing.Right;

        var transformSnapshot = transform;
        var flippedScale = transformSnapshot.localScale;

        flippedScale.x *= -1;
        transformSnapshot.localScale = flippedScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _isTakingDamage = true;
            _damageCooldownCoroutine = StartCoroutine(TakeDamageAndWait());
        }

        if (other.CompareTag("Pill"))
        {
            PillCollected?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _isTakingDamage = false;
            StopCoroutine(_damageCooldownCoroutine);
        }
    }

    private IEnumerator TakeDamageAndWait()
    {
        while (_isTakingDamage)
        {
            DamageTaken?.Invoke();
            yield return new WaitForSeconds(1f);
        }
    }
}

public enum Facing
{
    Right,
    Left
}