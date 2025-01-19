using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float countdownTime = 3f;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float speed;
    [SerializeField] private float followDistance = 0.95f;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private Facing facing = Facing.Left;


    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private bool _playerDetected;
    private int _currentTarget;
    private Transform _currentTargetTransform;
    private GameObject _followTarget;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentTargetTransform = targets[_currentTarget];
    }

    private void Update()
    {
        if (_playerDetected)
        {
            if (Vector3.Distance(_followTarget.transform.position, transform.position) <= followDistance)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }

            SetNextTarget(_followTarget.transform);
        }

        var remainingDistance = FindDirectionAndMove();

        if (Math.Abs(remainingDistance.x) < 0.3f && Math.Abs(remainingDistance.y) < 0.3f)
        {
            if (_currentTarget == targets.Count - 1)
            {
                _currentTarget = 0;
            }
            else
            {
                _currentTarget++;
            }

            SetNextTarget(targets[_currentTarget]);
        }

        Debug.DrawRay(transform.position, remainingDistance, Color.magenta);
    }

    private Vector3 FindDirectionAndMove()
    {
        var direction = _currentTargetTransform.position - transform.position;
        _rigidbody.velocity = new Vector2(direction.normalized.x * speed, direction.normalized.y * speed);
        
        switch (_rigidbody.velocity.x)
        {
            case > 0 when facing == Facing.Left:
            case < 0 when facing == Facing.Right:
                Flip();
                break;
        }

        return direction;
    }
    
    private void Flip()
    {
        facing = facing == Facing.Right ? Facing.Left : Facing.Right;

        var transformSnapshot = transform;
        var flippedScale = transformSnapshot.localScale;

        flippedScale.x *= -1;
        transformSnapshot.localScale = flippedScale;
    }

    private void SetNextTarget(Transform nextTarget)
    {
        _currentTargetTransform = nextTarget;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerDetected = true;
        _followTarget = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerDetected = false;

        if (isActiveAndEnabled)
            StartCoroutine(CountdownAndDisappear());
    }

    private IEnumerator CountdownAndDisappear()
    {
        yield return new WaitForSeconds(countdownTime);

        if (isActiveAndEnabled)
            StartCoroutine(DissolveAndDeactivate());
    }

    private IEnumerator DissolveAndDeactivate()
    {
        var time = 0f;

        while (time < fadeDuration)
        {
            var alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            var newColor = _spriteRenderer.color;
            newColor.a = alpha;
            _spriteRenderer.color = newColor;

            time += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        _collider = GetComponent<CircleCollider2D>();
        var color = _playerDetected ? Color.red : Color.green;

        Gizmos.color = color;
        var currentTransform = transform;
        var position = currentTransform.position + (Vector3)_collider.offset;
        Vector3 scale = currentTransform.lossyScale;
        var radius = _collider.radius * Mathf.Max(scale.x, scale.y);
        Gizmos.DrawWireSphere(position, radius);
    }
}