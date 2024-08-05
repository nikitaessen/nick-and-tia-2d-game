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

    [SerializeField]
    private Facing facing = Facing.Left;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    public void Initialize(SoundPlayer soundPlayer)
    {
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var inputVector = context.ReadValue<Vector2>();
        var velocity = inputVector * speed;
        
        switch (velocity.x)
        {
            case > 0 when facing == Facing.Left:
            case < 0 when facing == Facing.Right:
                Flip();
                break;
        }
        
        _rigidbody.velocity = velocity;
        var isWalking = Math.Abs(velocity.x) > 0 || Math.Abs(velocity.y) > 0;
        //TODO playSound() if walking
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