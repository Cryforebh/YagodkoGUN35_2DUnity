using UnityEngine;

public class ManagerObjects : MonoBehaviour
{
    private MovementPointData[] _allIdlePoint;

    private void Awake()
    {
        _allIdlePoint = GetComponentsInChildren<MovementPointData>();
    }

    public MovementPointData[] GetCollectionIdlePoints => _allIdlePoint;
}
