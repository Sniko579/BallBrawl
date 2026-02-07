public class Fall : IState
{
    public void OnEnter(StateMachine state)
    {
        state.PlayAnimation("FallAnim", 0.2f);
    }


    public void OnUpdate(StateMachine state)
    {
        if (Movement.IsGrounded)
        {
            state.ChangeState(new Idel());
        }
        else if (Movement.RB.linearVelocityY > 0.1f)
        {
            state.ChangeState(new Jump());
        }

    }
    public void OnExit(StateMachine state)
    {

    }
}
