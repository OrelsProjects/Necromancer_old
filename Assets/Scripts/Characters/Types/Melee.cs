using UnityEngine;

[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(MovementController))]
public class Melee : Character
{

    [SerializeField]
    private Shootable _shootable;

    private MovementController _movementController;
    private Attackable _attackable;

    private void Start()
    {
        _movementController = GetComponent<MovementController>();
        _attackable = GetComponent<Attackable>();
    }

    private void Update()
    {
        if (_movementController.CanAttack())
        {
            Attack();
        }
    }

    public void Attack()
    {
        _attackable.Attack(_movementController.Target);
    }
}