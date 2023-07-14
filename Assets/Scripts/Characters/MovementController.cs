using Assets.Scripts;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Character))]
public class MovementController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float distanceToStop = 1f;

    private Character _character;
    private Rigidbody2D _rb;
    private Attackable _attackable;
    private Animator _animator;
    private AnimationHelper _animationHelper;
    private Damageable _damageable;

    public Damageable Target { get; private set; }
    private bool _canAttack = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _attackable = GetComponent<Attackable>();
        _character = GetComponent<Character>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _animationHelper = new AnimationHelper(_animator);
    }

    void Update()
    {
        if (BattleManager.Instance.IsBattleOver())
        {
            ResetValues();
            return;
        }
        if (Target != null && Target.IsNotDead() && _damageable.IsNotDead()) // There's a target and its not dead and we're not dead.
        {
            MoveTowardsTarget();
        }
        else
        {
            if (_character.CharacterType == CharacterType.Hero)
            {
                Target = BattleManager.Instance.GetClosestEnemy(transform);
            }
            else
            {
                Target = BattleManager.Instance.GetClosestHero(transform);
            }
        }
    }
    private void MoveTowardsTarget()
    {
        try
        {
            _canAttack = false;
            if (Target == null) return;

            var direction = Target.transform.position - transform.position;
            if (direction.magnitude > distanceToStop)
            {
                _rb.velocity = direction.normalized * 5;
                _animationHelper.Running();
            }
            else
            {
                _rb.velocity = Vector3.zero;
                _animationHelper.Idle();
                _canAttack = true;
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Error");
        }
    }

    private void ResetValues()
    {
        _canAttack = false;
        _rb.velocity = Vector3.zero;
        if (_damageable.IsNotDead())
        {
            _animationHelper.Idle();
        }
    }

    public bool CanAttack()
    {
        return _canAttack && Target != null;
    }
}
