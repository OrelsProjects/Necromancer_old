using DamageNumbersPro;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Damageable : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField]
    private float _maxHealth = 100f;
    [SerializeField]
    private HealthBar _healthBar;
    [SerializeField]
    private DamageNumberMesh _damageNumberMeshPrefab;

    private AnimationHelper _animationHelper;
    private float _currentHealth = 100f;

    private void Awake()
    {
        _animationHelper = new AnimationHelper(GetComponent<Animator>());
        _healthBar.SetMaxHealth((int)_maxHealth);
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (IsDead())
        {
            OnDeath();
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBar.SetHealth((int)_currentHealth);
        ShowDamage(damage);
    }

    public void OnHeal(float heal)
    {
        _currentHealth += heal;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
    public void OnDeath()
    {
        _currentHealth = 0;
        _animationHelper.Dead();
        Destroy(gameObject, 2f);
    }
    public bool IsDead()
    {
        return _currentHealth <= 0;
    }

    public bool IsNotDead()
    {
        return !IsDead();
    }

    private void ShowDamage(float damage)
    {
        _damageNumberMeshPrefab.number = (int)damage;
        _damageNumberMeshPrefab.UpdateText();
        _damageNumberMeshPrefab.Spawn(transform.position + new Vector3(0, 1, 0));
    }
}