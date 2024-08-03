using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private List<Transform> targets;

    private Rigidbody2D _rigidbody;

    private int _currentTarget;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var direction = targets[_currentTarget].position - transform.position;
        _rigidbody.velocity = new Vector2(direction.normalized.x * speed, direction.normalized.y * speed);

        if (Math.Abs(direction.x) < 0.3f && Math.Abs(direction.y) < 0.3f)
        {
            if (_currentTarget == targets.Count - 1)
            {
                _currentTarget = 0;
            }
            else
            {
                _currentTarget++;
            }
        }

        Debug.DrawRay(transform.position, direction, Color.magenta);
    }
}