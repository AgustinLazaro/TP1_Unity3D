using UnityEngine;

public class ChaseState : StateBase
{
    public override void Enter()
    {
        animator.SetInteger("AnimationState", 1);
        manager.agent.isStopped = false;
        manager.agent.speed = manager.chaseSpeed;
    }

    public override void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(manager.transform.position, player.position);

        if (distance <= manager.attackRange)
        {
            manager.ChangeState(manager.attackState);
            return;
        }

        if (distance > manager.visionRange)
        {
            manager.ChangeState(manager.patrolState);
            return;
        }

        manager.agent.SetDestination(player.position);
    }

    public override void Exit() { if (manager.agent.gameObject.activeSelf) manager.agent.ResetPath(); }
}