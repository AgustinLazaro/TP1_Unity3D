using UnityEngine;

public class CivilianIdleState : ICivilianState
{
    private float waitTime;

    public void Enter(CivilianBrain civilian)
    {
        civilian.anim.SetInteger("AnimationState", 0);
        civilian.StopMovement();
        waitTime = Random.Range(0.5f, 1.5f);
    }

    public void Update(CivilianBrain civilian)
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0) civilian.ChangeState(new CivilianWalkState());
    }
}