public class Fall : IState
{
    public void OnEnter(StateMachine state)
    {
        state.PlayAnimation("FallAnim", 0.2f);
    }


    public void OnUpdate(StateMachine state)
    {
        if (Player.IsGrounded)
        {
            state.ChangeState(new Idel());
        }
        else if (Player.RB.linearVelocityY > 0.1f)
        {
            state.ChangeState(new Jump());
        }

    }
    public void OnExit(StateMachine state)
    {

    }
}
