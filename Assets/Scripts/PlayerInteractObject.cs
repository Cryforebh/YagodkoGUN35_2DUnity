using System;
using TMPro;
using UnityEngine;

public class PlayerInteractObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private string _textActivated = "Зажмите ЛКМ - чтобы взять.";
    [SerializeField] private TMP_Text _tMPText;
    [SerializeField] private float _attractionSpeed = 3f;
    [SerializeField] private float _throwForce = 100f;
    [Header("Настройка положения динамического обьека в руках:")]
    [SerializeField] private bool _isCenterActioanInCenterCamera = false;
    [SerializeField] private float _distanceMaxActive = 2.5f;
    [SerializeField] private float _addHeightCenterActioan = 0.5f;

    private InputControl _playerInput;
    private GameObject _targetObject;
    private Rigidbody _targetRigidbody;
    private bool _isObjectInCenterCursor;
    private bool _isObjectGrabbed;
    private bool _isButton;
    private float _distanceToObject;

    private Vector3 _pointEndAction;

    public event Action ButtonClickEvent;

    private void Awake()
    {
        _tMPText.text = _textActivated;
        _tMPText.enabled = false;
        _playerInput = new InputControl();
    }

    private void OnEnable()
    {
        _playerInput.PlayerActive.Enable();

        _playerInput.PlayerActive.MouseRightButton.started += MouseRightButton_started;
        _playerInput.PlayerActive.MouseLeftButton.started += MouseLeft_started;
        _playerInput.PlayerActive.MouseLeftButton.performed += MouseLeft_performed;
        _playerInput.PlayerActive.MouseLeftButton.canceled += MouseLeft_canceled;
        _playerInput.PlayerActive.MouseScroll.started += MouseScroll_started;
    }

    private void MouseRightButton_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) => ToThrow();

    private void MouseScroll_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) => ToMove(obj.ReadValue<float>() * 0.001f);

    private void MouseLeft_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) => ToTaking();

    private void MouseLeft_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) => ToLower();

    private void MouseLeft_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) { }

    private void ToThrow()
    {
        if (_isObjectGrabbed && !_isButton)
        {
            _isObjectGrabbed = false;
            _targetRigidbody.AddForce(_camera.transform.forward * _throwForce, ForceMode.Impulse);
            print("Кинуто");
        }
    }

    private void ToMove(float scroll)
    {
        if (_isObjectGrabbed && !_isButton)
        {
            _distanceToObject += scroll;
            _distanceToObject = Mathf.Clamp(_distanceToObject, 1f, _distanceMaxActive);
            print("Дальность изменена");
        }
    }

    private void ToTaking()
    {
        if (_isObjectInCenterCursor && !_isButton)
        {
            _distanceToObject = Vector3.Distance(_camera.transform.position, _targetObject.transform.position);
            _targetRigidbody = _targetObject.GetComponent<Rigidbody>();
            _isObjectGrabbed = true;
            _tMPText.enabled = false;
            print("Взято");
        }
        if (_isButton)
        {
            ButtonClickEvent?.Invoke();
            print("Нажато");
        }
    }

    private void ToLower()
    {
        if (_isObjectGrabbed && !_isButton)
        {
            _isObjectGrabbed = false;
            print("Отпущено");
        }
    }

    private void LateUpdate()
    {
        ProcessRaycastObjectInteraction();
    }

    private void FixedUpdate()
    {
        ProcessAttractionObjectToPointInteraction();
    }

    private void ProcessAttractionObjectToPointInteraction()
    {
        if (_isObjectGrabbed && !_isButton)
        {

            if (_isCenterActioanInCenterCamera)
            {
                _pointEndAction = _camera.transform.position + _camera.transform.forward * _distanceToObject;
            }
            else
            {
                _pointEndAction = (transform.position + Vector3.up * _addHeightCenterActioan) + _camera.transform.forward * _distanceToObject;
            }

            _targetRigidbody.velocity = (_pointEndAction - _targetObject.transform.position) * _attractionSpeed * Time.fixedDeltaTime;
            _targetRigidbody.angularVelocity = Vector3.zero;

            Debug.DrawLine(_camera.transform.position, _pointEndAction, Color.green);
        }
    }

    private void ProcessRaycastObjectInteraction()
    {
        if (!_isObjectGrabbed)
        {
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var hitTarget = hit.collider.gameObject;

                if (hitTarget.tag == "Ball")
                {
                    _targetObject = hitTarget;
                    var distanceToObject = Vector3.Distance(transform.position, _targetObject.transform.position);

                    if (distanceToObject <= _distanceMaxActive)
                    {
                        _isButton = false;
                        _isObjectInCenterCursor = true;
                        _tMPText.enabled = true;
                    }
                    else
                    {
                        _isObjectInCenterCursor = false;
                        _tMPText.enabled = false;
                    }
                }
                else
                {
                    _isObjectInCenterCursor = false;
                    _tMPText.enabled = false;
                }
                if (hitTarget.tag == "Button")
                {
                    _targetObject = hitTarget;
                    var distanceToObject = Vector3.Distance(transform.position, _targetObject.transform.position);

                    if (distanceToObject <= _distanceMaxActive)
                    {
                        _isObjectInCenterCursor = false;
                        _tMPText.enabled = false;
                        _isButton = true;
                    }
                    else
                    {
                        _isButton = false;
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        _playerInput.PlayerActive.Disable();
        _playerInput.PlayerActive.MouseRightButton.started -= MouseRightButton_started;
        _playerInput.PlayerActive.MouseLeftButton.started -= MouseLeft_started;
        _playerInput.PlayerActive.MouseLeftButton.performed -= MouseLeft_performed;
        _playerInput.PlayerActive.MouseLeftButton.canceled -= MouseLeft_canceled;
        _playerInput.PlayerActive.MouseScroll.started -= MouseScroll_started;
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(_camera.transform.position, _targetObject.transform.position);
        Gizmos.DrawLine(_camera.transform.position, (transform.position + Vector3.up * _addHeightCenterActioan) + _camera.transform.forward * _distanceMaxActive);
    }
}
