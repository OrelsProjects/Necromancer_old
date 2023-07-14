using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(MovementController))]
public class Ranger : MonoBehaviour
{

    [SerializeField]
    private Shootable _shootable;
    [SerializeField]
    private Transform _spawnPoint;

    private Attackable _attackable;
    private MovementController _movementController;
    private Animator _animator;
    private AnimationHelper _animationHelper;
    private bool _canShoot = true;

    private void Start()
    {
        _attackable = GetComponent<Attackable>();
        _movementController = GetComponent<MovementController>();
        _animator = GetComponent<Animator>();
        _animationHelper = new AnimationHelper(_animator);
    }

    private void Update()
    {
        if (_movementController.CanAttack())
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!_canShoot)
        {
            return;
        }
        _animationHelper.Attacking();
        Shootable arrow = Instantiate(_shootable, _spawnPoint.position, Quaternion.identity);
        arrow.SetTarget(_movementController.Target.gameObject.transform.position, _spawnPoint.position);
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(1 / _attackable._attackSpeed);
        _canShoot = true;
    }
}