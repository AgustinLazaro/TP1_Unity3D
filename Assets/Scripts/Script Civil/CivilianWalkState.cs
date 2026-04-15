using UnityEngine;

public class CivilianWalkState : ICivilianState
{
    public void Enter(CivilianBrain civilian)
    {
        civilian.anim.SetInteger("AnimationState", 1);
        civilian.GoToRandomPoint();
    }

    public void Update(CivilianBrain civilian)
    {
        if (civilian.HasReachedDestination()) civilian.ChangeState(new CivilianIdleState());
        if (civilian.IsPlayerNear()) civilian.ChangeState(new CivilianScaredState());
    }
}