using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject _laserPanel;

    private Slider _sliderLaserPanel;

    private void Awake()
    {
        _sliderLaserPanel = _laserPanel.GetComponentInChildren<Slider>();
    }

    public GameObject GetLaserPanel() => _laserPanel;
    public Slider GetSlider() => _sliderLaserPanel;
}
