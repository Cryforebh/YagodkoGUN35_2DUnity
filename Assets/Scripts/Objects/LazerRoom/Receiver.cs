using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField] private Color _expectedColor = Color.green;
    public bool IsActive = false;

    private Color _defaulColor;

    public void Activate(ref LineRenderer lineRender, Color laserColor)
    {
        if (!IsActive)
        {
            IsActive = true;
            lineRender.startColor = _expectedColor;
            lineRender.endColor = _expectedColor;
            _defaulColor = laserColor;
        }
    }

    public void Deactivate(ref LineRenderer lineRender, Color laserColor)
    {
        if (IsActive)
        {
            IsActive = false;
            lineRender.startColor = _defaulColor;
            lineRender.endColor = _defaulColor;
        }
    }
}
