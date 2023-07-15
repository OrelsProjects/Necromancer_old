using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Damageable))]
public class Zombie : MonoBehaviour
{
    private enum ZombieState
    {
        IDLE,
        CHASE,
        ATTACK,
        ATTACKING,
        DYING,
        DEAD
    }

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float distanceToStop = 0.2f;
    [SerializeField]
    private float _timeToZombify;

    private ZombieState _state;
    private Zombifieable _target;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Damageable _damageable;
    private AnimationHelper _animationHelper;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _animationHelper = new AnimationHelper(_animator);
        _state = ZombieState.IDLE;
    }

    void Update()
    {
        if (IsTargetDeceased())
        {
            _state = ZombieState.IDLE;
        }
        if (_damageable.IsDead() && _state != ZombieState.DYING)
        {
            _state = ZombieState.DYING;
        }

        switch (_state)
        {
            case ZombieState.IDLE:
                SetTarget();
                break;
            case ZombieState.CHASE:
                ChaseTarget();
                break;
            case ZombieState.ATTACK:
                StartCoroutine(Attack());
                break;
            case ZombieState.DYING:
                Die();
                break;
            case ZombieState.ATTACKING:
                break;
            case ZombieState.DEAD:
                ResetValues();
                break;
        }
    }

    private void SetTarget()
    {
        if (_target != null && !_target.IsZombified)
        {
            _state = ZombieState.CHASE;
        }
        if (IsTargetDeceased())
        {
            _target = BattleManager2.Instance.GetClosestZomifieable(transform);
            _state = ZombieState.CHASE;
        }
        if (_target == null) // The game is over. Zombies won.
        {
            _state = ZombieState.DEAD;
            _animationHelper.Idle();
            ResetValues();
            return;
        }
    }

    private void ChaseTarget()
    {
        Vector3 direction = _target.transform.position - transform.position;
        if (direction.magnitude > distanceToStop)
        {
            _rb.velocity = direction.normalized * 5;
            _animationHelper.Running();
        }
        else
        {
            _rb.velocity = Vector3.zero;
            _animationHelper.Idle();
            _state = ZombieState.ATTACK;
        }
    }

    private IEnumerator Attack()
    {
        _state = ZombieState.ATTACKING;
        _animationHelper.Attacking();
        yield return new WaitForSeconds(_timeToZombify);
        if (GetComponent<Damageable>().IsDead()) yield break;
        _target.Zombify();
        _state = ZombieState.IDLE;
    }

    private void Die()
    {
        ResetValues();
        _state = ZombieState.DEAD;
        _animationHelper.Dead();
        Destroy(gameObject, 2f);
    }

    private bool IsTargetDeceased()
    {
        return _target == null || _target.IsZombified;
    }

    private void ResetValues()
    {
        _rb.velocity = Vector2.zero;
    }
}