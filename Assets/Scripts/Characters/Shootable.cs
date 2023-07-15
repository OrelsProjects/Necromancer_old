using System.Collections;
using UnityEngine;


public enum CollisionType
{
    Disappear,
    Stay,
    StayTillNext
}
// Require collider2d and a sprite
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Shootable : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private CollisionType _collisionType;
    [SerializeField]
    private float _damage;

    private float _destroyIfNotHitAfter = 5f;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfter());
    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(_destroyIfNotHitAfter);
        Destroy(gameObject);
    }

    /// <summary>
    /// Sets a target to hit
    /// </summary>
    /// <param name="target"> Must have a hitbox! </param>
    public void SetTarget(Vector2 target, Vector2 _spawn)
    {
        Vector2 direction = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rigidbody2D.velocity = direction.normalized * _speed;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            collision.gameObject.GetComponent<Damageable>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}