using UnityEngine;

public struct PhysicForce
{

    Vector2 CurrentVelocity;


    public void SetCurrentVelocity(Vector2 Velocity)
    {
        CurrentVelocity = Velocity;
    }
    public void SetCurrentVelocityX(float X)
    {
        CurrentVelocity.x = X;
    }
    public void SetCurrentVelocityY(float Y)
    {
        CurrentVelocity.y = Y;
    }




    public Vector2 GetForceByT(Vector2 Force, float T)
    {
        return Vector2.Lerp(CurrentVelocity, Force, T * Time.fixedDeltaTime);
    }
   

    public Vector2 GetForce(Vector2 Force)
    {
        return CurrentVelocity = Force;
    }
    

}
