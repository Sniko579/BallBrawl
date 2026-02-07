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
    public float GetXForceByT(float XForce, float T)
    {
        return Mathf.Lerp(CurrentVelocity.x, XForce, T * Time.fixedDeltaTime);
    }
    public float GetYForceByT(float YForce, float T)
    {
        return Mathf.Lerp(CurrentVelocity.y, YForce, T * Time.fixedDeltaTime);
    }


    public Vector2 GetForce(Vector2 Force)
    {
        return CurrentVelocity = Force;
    }
    public float GetXForce(float XForce)
    {
        return CurrentVelocity.x = XForce;
    }
    public float GetYForce(float YForce)
    {
        return CurrentVelocity.y = YForce;
    }

}
