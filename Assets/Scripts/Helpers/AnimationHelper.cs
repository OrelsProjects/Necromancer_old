using UnityEditor;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Running,
    Attacking,
    Dead
}

public class AnimationHelper
{

    private Animator _animator;

    public AnimationHelper(Animator animator)
    {
        _animator = animator;
        Idle();
    }

    public void Idle(bool idle = true)
    {
        _animator.SetBool("Idle", idle);
        _animator.SetBool("Running", false);
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Dead", false);
    }

    public void Attacking(bool attacking = true)
    {
        if (attacking)
        {
            Running(false);
            _animator.SetBool("Idle", true);
            _animator.SetTrigger("Attack");
        }
    }

    public void Running(bool running = true)
    {
        if (_animator.GetBool("Running") != running)
        {
            _animator.SetBool("Running", running);
        }
        if (_animator.GetBool("Idle") != !running)
        {
            _animator.SetBool("Idle", !running);
        }
    }

    public void Dead(bool dead = true)
    {
        Idle(false);
        _animator.SetBool("Dead", dead);
    }
}