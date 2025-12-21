using System.Collections.Generic;
using UnityEngine;

public class PatchWalkContainer : MonoBehaviour
{
    [SerializeField] private Vector3 m_centerScanner;
    [SerializeField] private float m_maxDistanceScanne;
    [SerializeField] private float m_minDistanceScanne;
    [SerializeField] private LayerMask m_layerMask;

    private List<Collider> m_collidersPatch = new List<Collider>();

    public List<Collider> CollidersPatch => m_collidersPatch;

    private void Awake()
    {
        var colliders = Physics.OverlapSphere(m_centerScanner, m_maxDistanceScanne, m_layerMask);

        foreach (var patchCollider in colliders)
        {
            m_collidersPatch.Add(patchCollider);
        }
    }

    public Vector3 GetScanneNextPatch(Transform botWalkingTransform)
    {
        if (m_collidersPatch.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 nearPatchPosition = Vector3.zero;

        List<Vector3> nearPatchesPosition = new List<Vector3>();

        foreach (var patchCollider in m_collidersPatch)
        {
            var distance = Vector3.Distance(botWalkingTransform.position, patchCollider.transform.position);

            if (distance >= m_minDistanceScanne)
            {
                nearPatchesPosition.Add(patchCollider.transform.position);
            }
        }

        int indexPatch = Random.Range(1, nearPatchesPosition.Count);
        if (indexPatch >= nearPatchesPosition.Count) indexPatch = 0;

        nearPatchPosition = nearPatchesPosition[indexPatch];

        return nearPatchPosition;
    }
}
