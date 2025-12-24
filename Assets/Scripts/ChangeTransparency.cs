using UnityEngine;

public class ChangeTransparency : MonoBehaviour
{
    [SerializeField] private float m_fadeDuration = 1f;

    private MeshRenderer m_renderer;
    private float m_startTime;
    private bool m_isFadingOut;

    private void Awake()
    {
        m_renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        // –аботаем с Time.realtimeSinceStartup дл€ синхранизации с глобавльным временем, где не будет зависимости от частоты кадров.
        // »наче скорость мерцани€ будет зависить от кадров на устройстве

        m_startTime = Time.realtimeSinceStartup;
        m_isFadingOut = true;
    }

    private void Update()
    {
        UpdateTransparency();
    }

    private void UpdateTransparency()
    {
        float elapsedTime = Time.realtimeSinceStartup - m_startTime;
        float progress = Mathf.Clamp01(elapsedTime / m_fadeDuration);

        float alpha = m_isFadingOut ? Mathf.Lerp(1, 0, progress) : Mathf.Lerp(0, 1, progress);

        Color currentColor = m_renderer.material.color;
        m_renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

        if (progress >= 1)
        {
            m_isFadingOut = !m_isFadingOut;
            m_startTime = Time.realtimeSinceStartup;
        }
    }
}
