using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ZombieBomb : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private float _range = 0.4f;

    [SerializeField]
    [Range(0, 0.1f)]
    private float _explodeSpeed = 0.05f;

    public bool IsReleased { get; private set; }
    private Vector2 _positionToRelease;
    private CircleCollider2D _circleCollider;

    void Awake()
    {
        gameObject.SetActive(false);
        IsReleased = false;
        transform.localScale = Vector3.zero;
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReleased)
        {
            StartCoroutine(Explode());
        }
    }

    public void ReleaseBomb(Vector2 position)
    {
        if (!IsReleased)
        {
            _positionToRelease = position;
            IsReleased = true;
            _circleCollider.radius = 1;
            gameObject.SetActive(true);
        }
    }

    private IEnumerator Explode()
    {
        if (_circleCollider.radius >= _range)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 currentScale = transform.localScale;
            transform.localScale = new(currentScale.x + _explodeSpeed, currentScale.y + _explodeSpeed, currentScale.z);
            transform.position = _positionToRelease;
            _circleCollider.radius += (1 / 2 * Mathf.PI) + _explodeSpeed;
            yield return new WaitForSeconds(0.07F);
            StartCoroutine(Explode());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Civilian"))
        {
            var civilian = collision.gameObject.GetComponent<Character>();
            civilian.Zombify();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Zombifieable"))
        {
            var civilian = collision.gameObject.GetComponent<Zombifieable>();
            civilian.Zombify();
        }
    }

}