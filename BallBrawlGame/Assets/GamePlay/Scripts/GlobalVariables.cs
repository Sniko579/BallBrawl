using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalVariables", menuName = "Scriptable Objects/GlobalVariables")]
public class GlobalVariables : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeed = 4f;
    public float JumpHeight = 4f;
    public float Acceleration = 30f;
    public float AirAcceleration = 30f;
    public float Deceleration = 30f;

    [Header("Dash Setting")]
    public float DashSpeed = 50;
    public float DashTime = 0.11f;
    public float DashCoolDown = 0.6f;
    public float DashAcceleration = 8f;
    public float DashDeceleration = 4f;

    [Header("Bounceness Setting")]
    [Range(0f, 100f)] public float ReflectionPower = 50f;

    [Header("Gravity Setting")]
    public float NoramlGravity = 1f;
    public float FallGravity = 1f;
    public float DashGravity = 1f;


    public List<IGlobalData> GlobalDatas = new List<IGlobalData>();
    private void OnValidate()
    {
        Change();
    }

    void Change()
    {
        foreach (var data in GlobalDatas)
        {
            data.UpdateVaribles();
        }
    }

}

public interface IGlobalData
{
    void UpdateVaribles();
}
