using DamageNumbersPro;
using System.Collections;
using UnityEngine;

public class Attackable : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField]
    private float _strength = 30f; // The strength of the attack
    [SerializeField]
    public float _attackSpeed = 1; // attacks per second

    private bool _isAttacking = false;
    private bool _canAttack = true;

    private void Update()
    {
        if (GetComponent<Zombifieable>().IsZombified)
        {
            _canAttack = false;
        }
    }

    public bool Attack(Damageable target)
    {
        if (_isAttacking || target.IsDead())
        {
            return false;
        }
        _isAttacking = true;
        target.TakeDamage(_strength);
        StartCoroutine(Cooldown());
        return true;
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1 / _attackSpeed);
        _isAttacking = false;
    }

    public bool CanAttack()
    {
        return !_isAttacking && _canAttack;
    }
}