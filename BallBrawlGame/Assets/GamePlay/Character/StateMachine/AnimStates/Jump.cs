public class Jump : IState
{
    
    public void OnEnter(StateMachine state)
    {
        state.PlayAnimation("JumpAnim", 0.2f);
        
    }

    public void OnUpdate(StateMachine state)
    {

        if (Player.RB.linearVelocityY < -0.1f)
        {

            state.ChangeState(new Fall());
        }
        else if (Player.IsGrounded)
        {
            state.ChangeState(new Idel());
        }

    }
    public void OnExit(StateMachine state)
    {

    }


}
