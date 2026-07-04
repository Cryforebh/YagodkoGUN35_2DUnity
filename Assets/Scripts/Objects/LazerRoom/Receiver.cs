using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField] private Color _expectedColor = Color.red;
    [SerializeField] private Color _compliteColor = Color.green;
    [SerializeField] private GameObject _projection;

    private bool _isActive = false;
    private Color _defaultColor;

    public Color DefaultColor => _defaultColor;
    public bool IsActive => _isActive;

    private void Start()
    {
        ProjectionVision(false);
        _defaultColor = _expectedColor;
    }

    public void Activate(ref LineRenderer lineRender, Color laserColor)
    {
        if (!_isActive)
        {
            _isActive = true;
            lineRender.startColor = _compliteColor;
            lineRender.endColor = _compliteColor;
            _defaultColor = laserColor;
            ProjectionVision(true);
        }
    }

    public void Deactivate(ref LineRenderer lineRender, Color laserColor)
    {
        if (_isActive)
        {
            _isActive = false;
            lineRender.startColor = _defaultColor;
            lineRender.endColor = _defaultColor;
            ProjectionVision(false);
        }
    }

    private void ProjectionVision(bool vision)
    {
        if (_projection == null) return;
        _projection.SetActive(vision);
    }
}
