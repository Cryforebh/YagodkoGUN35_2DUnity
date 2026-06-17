using Netologia.Quest.Characters;
using UnityEngine;


public class MovementPointData : MonoBehaviour
{
    [SerializeField, Header("Basic parameters (Required!):")] private Transform _actionPointPosition;
    [SerializeField] private float _timeDelay = 5.0f;
    [SerializeField] private bool _isRotateTowardsTheObject = true;
    [SerializeField] private float _circleRadius = 0.5f;
    [SerializeField, Header("Belongs to the character (Optional):")] private BaseMovement _belongsTo;

    public float TimeDelay => _timeDelay;
    public Vector3 ActionPointPosition => _actionPointPosition.position;
    public float Radius => _circleRadius;

    public bool CharacterCanUse(BaseMovement bot) => !_belongsTo || _belongsTo == bot;

    public Vector3 GetDirection(Vector3 botPosition)
    {
        if (_isRotateTowardsTheObject)
        {
            Vector3 targetDirection = transform.position - botPosition;
            targetDirection.y = 0;
            return targetDirection;
        }
        else
            return _actionPointPosition.forward.normalized;
    }

    public void SetCharacterUse(BaseMovement bot) => _belongsTo = bot;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_actionPointPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_actionPointPosition.position, _circleRadius);
            Gizmos.DrawSphere(_actionPointPosition.position, _circleRadius / 4);
        }
    }
#endif
}
