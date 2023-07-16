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
        WANDER,
        CHASE,
        ATTACKING,
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
        if (_damageable.IsDead() || (_state == ZombieState.IDLE && BattleManager2.Instance.IsBattleOver()))
        {
            ResetValues(ZombieState.IDLE);
            return;
        }

        if (_target == null)
        {
            _state = ZombieState.WANDER;
        }

        switch (_state)
        {
            case ZombieState.WANDER:
                SetTarget();
                break;
            case ZombieState.CHASE:
                ChaseTarget();
                break;
            case ZombieState.ATTACKING:
                AttackTarget();
                break;
            case ZombieState.IDLE: // If we reached here, it means the game is not over yet.
                ResetValues(ZombieState.WANDER);
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
            ResetValues(ZombieState.IDLE);
            _animationHelper.Idle();
            return;
        }
    }

    private void ChaseTarget()
    {
        Vector3 direction = _target.transform.position - transform.position;
        if (IsTargetReached()) // Target reached
        {
            if (_target.IsZombified)
            {
                _state = ZombieState.WANDER;
                return;
            }
            else
            {
                ResetValues();
                _state = ZombieState.ATTACKING;
            }
        }
        else
        {
            _rb.velocity = direction.normalized * 5;
            _animationHelper.Running();
        }
    }

    private void AttackTarget()
    {
        if (IsTargetReached())
        {
            StartCoroutine(Attack());
        }
        else
        {
            ResetValues(ZombieState.CHASE);
            _animationHelper.Idle();
        }
    }

    private IEnumerator Attack()
    {
        _state = ZombieState.ATTACKING;
        _animationHelper.Attacking();
        bool isTargetZombified = _target.Zombify();
        yield return new WaitForSeconds(_timeToZombify);
        if (isTargetZombified)
        {
            _state = ZombieState.WANDER;
        }
    }

    private bool IsTargetReached()
    {
        Vector3 direction = _target.transform.position - transform.position;
        return direction.magnitude < distanceToStop;
    }

    private bool IsTargetDeceased()
    {
        return _target == null || _target.IsZombified;
    }

    private void ResetValues(ZombieState? state = null)
    {
        if (state != null) _state = state.Value;
        _rb.velocity = Vector2.zero;
    }
}