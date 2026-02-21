public class Idel : IState
{
    public void OnEnter(StateMachine state)
    {
        state.PlayAnimation("IdelAnim", 0.2f);
    }


    public void OnUpdate(StateMachine state)
    {

        if (Player.RB.linearVelocityY > 0.1f)
        {
            state.ChangeState(new Jump());
        }
        else if (Player.RB.linearVelocityY < -0.1f)
        {
            state.ChangeState(new Fall());
        }

    }
    public void OnExit(StateMachine state)
    {

    }
}
