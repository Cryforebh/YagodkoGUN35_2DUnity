using Netologia.Quest.Characters.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public abstract class LaserElementBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MeshRenderer _circle;
    [SerializeField] private float _radiusActive = 3.5f;

    private Transform _circleTrans;
    private InterfaceManager _interfaceManager; // Inject
    private Slider _slider; // Inject
    private Controls _input; // Inject
    private PlayerController _playerController; // Inject

    private bool _mouseOnObject = false;
    private bool _isActive = false;

    public Vector3 CirclePosition => _circleTrans.position;

    [Inject]
    protected void Construct(InterfaceManager interfaceManager, Controls input, PlayerController player)
    {
        _interfaceManager = interfaceManager;
        _input = input;
        _playerController = player;
    }

    protected virtual void Start()
    {
        _circleTrans = _circle.transform;
        _circle.enabled = false;
        _slider = _interfaceManager.GetSlider();
    }

    protected virtual void Update()
    {
        ProcessClickMouse();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _circle.enabled = true;
        _mouseOnObject = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _mouseOnObject = false;
        _circle.enabled = false;
    }

    protected void ProcessClickMouse()
    {
        if (_input.Mouse.Click.ReadValue<float>() > 0.5f)
        {
            var laserPanel = _interfaceManager.GetLaserPanel();

            if (!_mouseOnObject)
            {
                if (_isActive)
                {
                    laserPanel.SetActive(false);
                    Exit();
                    Debug.Log($"Exit: {name}");
                }
                return;
            }

            if (Vector3.Distance(transform.position, _playerController.transform.position) > _radiusActive
                /*_playerController.StoppingDistanceForInteract + 1.5f*/) return;

            _isActive = true;
            laserPanel.SetActive(true);
            //_slider = _interfaceManager.GetSlider();

            if (_slider != null)
            {
                // Подписываемся на изменение значения
                _slider.onValueChanged.RemoveAllListeners(); // очищаем предыдущие слушатели

                // Устанавливаем начальное значение = текущий угол объекта
                float currentAngle = transform.localEulerAngles.y; // ось Y (вертикальный поворот)
                _slider.value = currentAngle;

                _slider.onValueChanged.AddListener(OnSliderValueChanged);
            }
        }
    }

    private void OnSliderValueChanged(float value)
    {
        Vector3 euler = transform.localEulerAngles;
        euler.y = value; // меняем угол по оси Y
        transform.localEulerAngles = euler;
    }

    protected virtual void OnDisable()
    {
        Exit();
    }

    protected void Exit()
    {
        if (_slider != null)
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            _isActive = false;
        }
    }
}
