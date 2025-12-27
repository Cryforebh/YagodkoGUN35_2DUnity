using DG.Tweening;
using UnityEngine;

public class BotObstacle : MonoBehaviour
{
    private Vector3 m_startScale;
    private Vector3 m_endScale;
    private float m_durationScale;

    private void Awake()
    {
        m_startScale = transform.localScale;
        m_endScale = new Vector3(transform.localScale.x * 1.3f, transform.localScale.y, transform.localScale.z * 1.3f);
        m_durationScale = Random.Range(0.5f, 0.8f);
    }

    private void Start()
    {
        Sequence animations = DOTween.Sequence();

        animations.Append(transform.DOScale(m_endScale, m_durationScale))
            .Append(transform.DOScale(m_startScale, m_durationScale))
            .SetLoops(-1, LoopType.Yoyo);
    }
}
