using UnityEngine;

public class AttackState : StateBase
{
    public override void Enter()
    {
        manager.agent.isStopped = true;
        animator.SetInteger("AnimationState", 2);
    }

    public override void Update()
    {
        if (player == null) return;

        manager.RotateTowardsTarget(player.position);
        manager.FireWeapon();

        if (Vector3.Distance(manager.transform.position, player.position) > manager.attackRange)
        {
            manager.ChangeState(manager.chaseState);
        }
    }

    public override void Exit() => manager.agent.isStopped = false;
}