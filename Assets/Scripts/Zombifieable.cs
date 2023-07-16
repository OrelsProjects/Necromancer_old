using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Zombifieable : MonoBehaviour
{

    [SerializeField]
    private GameObject _zombiePrefab;
    [SerializeField]
    private float _bitesToZombify = 1;
    [SerializeField]
    private float _recoverFromAttackDelay = 2f;

    private Animator _animator;
    private Rigidbody2D _rb;
    private WanderController _wanderController;
    private AnimationHelper _animationHelper;
    private Vector2? _wanderTarget;

    public bool IsZombified { get; private set; }

    // Use this for initialization
    void Start()
    {
        LayerMask.NameToLayer("Zombiefieable");
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _wanderController = GetComponent<WanderController>();
        _animationHelper = new AnimationHelper(_animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (_wanderTarget == null)
        {
            // Wander();
        }
    }

    /// <summary>
    /// Makes the player wander around the map.
    /// </summary>
    private void Wander()
    {
        if (IsZombified)
        {
            _rb.velocity = Vector2.zero;
            return;
        }
        _wanderTarget = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
        _animationHelper.Running();
        // transform.LookAt(_wanderTarget.Value);
        // transform.rotation = Quaternion.LookRotation(Vector3.forward, _wanderTarget.Value);
        _rb.velocity = (_wanderTarget.Value - (Vector2)transform.position).normalized * 2;
        StartCoroutine(StopWandering());
    }

    /// <summary>
    /// Stops the player from wandering after a random amount of time.
    /// </summary>
    private IEnumerator StopWandering()
    {
        yield return new WaitForSeconds(Random.Range(1, 3)); // Wander for a random amount of time.
        if (!IsZombified)
        {
            _animationHelper.Running(false);
            _rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(Random.Range(0, 3)); // Wait for a random amount of time before wandering again.
            _wanderTarget = null;
        }
    }

    /// <summary>
    /// Called when the player is bitten by a zombie.
    /// </summary>
    /// <returns>True if the player is zombified, false otherwise.</returns>
    public bool Zombify(float timeToTurn = 1.5f)
    {
        if (IsZombified) return true;

        _bitesToZombify--;
        if (_bitesToZombify <= 0)
        {
            StartCoroutine(TransformToZombie(timeToTurn));
            return true;
        }
        StartCoroutine(AttackDelay());
        return false;
    }

    private IEnumerator TransformToZombie(float timeToTurn)
    {
        IsZombified = true;
        _rb.velocity = Vector2.zero;
        _animationHelper.Dead();
        yield return new WaitForSeconds(timeToTurn);
        GameObject zombie = Instantiate(_zombiePrefab, transform.position, transform.rotation);
        BattleManager2.Instance.AddZombie(zombie.GetComponent<Zombie>());
        Destroy(gameObject);
    }

    // The zombifieable will be stuck after being attacked for a while
    private IEnumerator AttackDelay()
    {
        _wanderController.DisableMovement();
        yield return new WaitForSeconds(_recoverFromAttackDelay);
        _wanderController.EnableMovement();
    }
}
