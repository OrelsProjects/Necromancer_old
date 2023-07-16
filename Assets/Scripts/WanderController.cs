using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class WanderController : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Animator _animator;

    private Vector2? _wanderTarget;
    private bool _canMove = true;


    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_wanderTarget == null)
        {
            Wander();
        }
    }

    public void DisableMovement()
    {
        _canMove = false;
    }

    public void EnableMovement()
    {
        _canMove = true;
    }

    /// <summary>
    /// Makes the player wander around the map.
    /// </summary>
    private void Wander()
    {
        if (!_canMove)
        {
            return;
        }
        _wanderTarget = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
        _animator.SetBool("Running", true);
        _rb.velocity = (_wanderTarget.Value - (Vector2)transform.position).normalized * 2;
        StartCoroutine(StopWandering());
    }

    /// <summary>
    /// Stops the player from wandering after a random amount of time.
    /// </summary>
    private IEnumerator StopWandering()
    {
        yield return new WaitForSeconds(Random.Range(2, 7));
        _animator.SetBool("Running", false);
        _rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(Random.Range(1, 3));
        _wanderTarget = null;
    }
}