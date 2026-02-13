using UnityEngine;

public class ReflectionBody
{
    Vector2 LastVelocity;
    public void SetVelocity(Vector2 currentVelocity,string type)
    {
        LastVelocity = currentVelocity;
        Debug.Log(type);
    }

    
    public Vector2 ReturnForce(Collision2D col, float Power)
    {
        var enemy = col.gameObject.GetComponent<IPVP>();
        if (enemy != null) Shoot(col);


        var speed = LastVelocity.magnitude * (Power / 100);

        Vector2 direction = Vector2.Reflect(LastVelocity.normalized, col.contacts[0].normal).normalized;

        return direction * speed;
    }
    public float ReturnXForce(Collision2D col, float Power)
    {
        var enemy = col.gameObject.GetComponent<IPVP>();
        if (enemy != null) Shoot(col);


        var speed = LastVelocity.magnitude * (Power / 100);

        Vector2 direction = Vector2.Reflect(LastVelocity.normalized, col.contacts[0].normal).normalized;

        return direction.x * speed;
    }
    public float ReturnYForce(Collision2D col, float Power)
    {
        var enemy = col.gameObject.GetComponent<IPVP>();
        if (enemy != null) Shoot(col);


        var speed = LastVelocity.magnitude * (Power / 100);

        Vector2 direction = Vector2.Reflect(LastVelocity.normalized, col.contacts[0].normal).normalized;

        return direction.y * speed;
    }


    public void Shoot(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            return;
        col.gameObject.GetComponent<IPVP>().GetReflectionBody().SetVelocity(LastVelocity,"Hit");

    }




}


public interface IPVP
{
    public ReflectionBody GetReflectionBody();   
   
}



