using UnityEngine;

public struct ReflectionSystem
{

    Vector2 LastVelocity;

    public void SetVelocity(Vector2 currentVelocity)
    {
        LastVelocity = currentVelocity;
    }

    public Vector2 ReturnForce(Collision2D col, float Power)
    {
       
        Rigidbody2D colVelocity = col.gameObject.GetComponent<Rigidbody2D>();


        if (colVelocity != null)
        {
            LastVelocity = LastVelocity - col.gameObject.GetComponent<BallConteroler>().LastVelocity();
            
        }

        Vector2 direction = Vector2.Reflect(LastVelocity.normalized, col.contacts[0].normal.normalized);
       

        float speed = LastVelocity.magnitude * (Power / 100);
        return direction * speed;
    }






}
