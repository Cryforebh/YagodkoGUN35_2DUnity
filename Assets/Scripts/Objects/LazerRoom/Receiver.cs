using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField] private Color _expectedColor = Color.green;
    [SerializeField] private GameObject _projection;
    public bool IsActive = false;

    private Color _defaulColor;

    private void Start()
    {
        ProjectionVision(false);
    }

    public void Activate(ref LineRenderer lineRender, Color laserColor)
    {
        if (!IsActive)
        {
            IsActive = true;
            lineRender.startColor = _expectedColor;
            lineRender.endColor = _expectedColor;
            _defaulColor = laserColor;
            ProjectionVision(true);
        }
    }

    public void Deactivate(ref LineRenderer lineRender, Color laserColor)
    {
        if (IsActive)
        {
            IsActive = false;
            lineRender.startColor = _defaulColor;
            lineRender.endColor = _defaulColor;
            ProjectionVision(false);
        }
    }

    private void ProjectionVision(bool vision)
    {
        if (_projection == null) return;
        _projection.SetActive(vision);
    }
}
