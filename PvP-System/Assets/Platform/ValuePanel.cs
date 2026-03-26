using UnityEngine;

[CreateAssetMenu(fileName = "ValuePanel", menuName = "Scriptable Objects/ValuePanel")]
public class ValuePanel : ScriptableObject
{
    [Range(0.1f, 50f)] public float Speed;
    [Range(0.1f, 50f)] public float Accelration;
    [Range(0.1f, 50f)] public float Decelration;
    [Space]
    [Range(0, 100)] public int Bounceness_Power;
}
