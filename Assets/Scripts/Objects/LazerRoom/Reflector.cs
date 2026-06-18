using UnityEngine;

public class Reflector : LaserElementBase
{
    private Vector3 _normal;

    protected override void Start()
    {
        base.Start();
    }

    public void SetNormal(Vector3 normal) => _normal = normal;

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
