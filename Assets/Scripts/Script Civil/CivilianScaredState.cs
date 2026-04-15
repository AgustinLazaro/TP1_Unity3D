using UnityEngine;

public class CivilianScaredState : ICivilianState
{
    public void Enter(CivilianBrain civilian)
    {
        civilian.anim.SetInteger("AnimationState", 2);
        civilian.StopMovement();
        civilian.PanicScream();
    }

    public void Update(CivilianBrain civilian)
    {
        civilian.LookAt(civilian.target.position);
        if (!civilian.IsPlayerNear()) civilian.ChangeState(new CivilianIdleState());
    }
}