using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateBase
{
    public override void Enter()
    {
        animator.SetInteger("AnimationState", 0);
        manager.agent.isStopped = false;
        manager.agent.speed = manager.walkSpeed;
        SetRandomDestination();
    }

    public override void Update()
    {
        if (player != null && Vector3.Distance(manager.transform.position, player.position) <= manager.visionRange)
        {
            manager.ChangeState(manager.chaseState);
            return;
        }

        if (!manager.agent.pathPending && manager.agent.remainingDistance < 0.5f)
            SetRandomDestination();
    }


    private void SetRandomDestination()
    {
        
        Vector3 randomPoint = manager.transform.position + Random.insideUnitSphere * manager.walkRadius;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, manager.walkRadius, 1))
        {
            manager.agent.SetDestination(hit.position);
        }
    }

    public override void Exit() { if (manager.agent.gameObject.activeSelf) manager.agent.ResetPath(); }
}