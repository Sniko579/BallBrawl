using UnityEngine;

public struct ReflectionBody
{
    Vector2 LastVelocity;

    public void SetVelocity(Vector2 currentVelocity)
    {
        LastVelocity = currentVelocity;
    }

    public Vector2 ReturnForce(Collision2D col, float Power)
    {
        Vector2 direction;
        var enemy = col.gameObject.GetComponent<IPVP>();
        if (enemy != null)
            return col.otherRigidbody.linearVelocity;

        direction = Vector2.Reflect(LastVelocity.normalized, col.contacts[0].normal).normalized;

        float speed = LastVelocity.magnitude * (Power / 100);

        return direction * speed;
    }



}


public interface IPVP { }







