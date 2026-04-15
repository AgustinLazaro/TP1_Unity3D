using UnityEngine;

public abstract class StateBase
{
    protected EnemyBrain manager;
    protected Animator animator;
    protected Transform player;

    public virtual void Initialize(EnemyBrain _manager, Animator _animator, Transform _player)
    {
        manager = _manager;
        animator = _animator;
        player = _player;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}