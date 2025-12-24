using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private Transform m_approachingObjectTransform;
    [SerializeField] private AnimationCurve m_colorCurve;
    [SerializeField] private float m_minDistance = 4f;

    private Renderer m_renderer;
    private Color m_startColor;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        m_startColor = m_renderer.material.color;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, m_approachingObjectTransform.position);


        if (distance < m_minDistance)
        {
            float redColor = m_colorCurve.Evaluate(1 - distance / m_minDistance);
            m_renderer.material.color = new Color(m_startColor.r, m_startColor.g - redColor, m_startColor.b - redColor);
        }
        else
        {
            m_renderer.material.color = m_startColor;
        }
    }
}
