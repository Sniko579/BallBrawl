using UnityEngine;

public struct ReflectionBody
{
    Vector2 LastVelocity;
    public void SetVelocity(Vector2 currentVelocity)
    {
        LastVelocity = currentVelocity;
    }


    public Vector2 ReturnForce(Collision2D col , float Power)
    {
        var speed = LastVelocity.magnitude * Power;
        
        Vector2 direction = Vector2.Reflect(LastVelocity.normalized,col.contacts[0].normal).normalized;
       
        return direction * speed;
    }
    public float ReturnXForce(Collision2D col , float Power)
    {
        var speed = LastVelocity.magnitude * Power;
        
        Vector2 direction = Vector2.Reflect(LastVelocity.normalized,col.contacts[0].normal).normalized;
       
        return direction.x * speed;
    }
    public float ReturnYForce(Collision2D col , float Power)
    {
        var speed = LastVelocity.magnitude * Power;

        
        Vector2 direction = Vector2.Reflect(LastVelocity.normalized,col.contacts[0].normal).normalized;
       
        return direction.y * speed;
    }
    
}
