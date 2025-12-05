using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerInteractObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private string _textActivated = "Зажмите ЛКМ - чтобы взять.";
    [SerializeField] private TMP_Text _tMPText;
    [SerializeField] private Rigidbody _targetObject;
    [SerializeField] private float _distanceActive = 2.5f;
    [SerializeField] private float _attractionSpeed = 3f;
    [SerializeField] private float _throwForce = 100f;

    private InputControl _playerInput;
    private bool _isObjectInCenterCursor;
    private bool _isObjectGrabbed;
    private float _distanceToObject;

    private Vector3 _pointEndAction;

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

        StartCoroutine(ProcessRaycastObject());
        StartCoroutine(ProcessAttractionObject());
    }

    private void MouseRightButton_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) => ToThrow();

    private void MouseScroll_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) => ToMove(obj.ReadValue<float>() * 0.001f);

    private void MouseLeft_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) => ToTaking();

    private void MouseLeft_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) => ToLower();

    private void MouseLeft_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) { }

    private void ToThrow()
    {
        if (_isObjectGrabbed)
        {
            _isObjectGrabbed = false;
            _targetObject.AddForce(_camera.transform.forward * _throwForce, ForceMode.Impulse);
            print("Кинуто");
        }
    }

    private void ToMove(float scroll)
    {
        if (_isObjectGrabbed)
        {
            _distanceToObject += scroll;
            _distanceToObject = Mathf.Clamp(_distanceToObject, 1f, _distanceActive);
            print("Дальность изменена");
        }
    }

    private void ToTaking()
    {
        if (_isObjectInCenterCursor)
        {
            _distanceToObject = Vector3.Distance(_camera.transform.position, _targetObject.transform.position);
            _isObjectGrabbed = true;
            _tMPText.enabled = false;
            print("Взято");
        }
    }

    private void ToLower()
    {
        if (_isObjectGrabbed)
        {
            _isObjectGrabbed = false;
            print("Отпущено");
        }
    }

    private IEnumerator ProcessAttractionObject()
    {
        while (true)
        {
            if (_isObjectGrabbed)
            {
                //_pointEndAction = _camera.transform.position + _camera.transform.forward * _distanceToObject; - Старый вариант,
                //больше похоже на игру REPO, обьект перед глазами, неудобно целиться.

                _pointEndAction = transform.position  + _camera.transform.forward * _distanceToObject;
                _targetObject.velocity = (_pointEndAction - _targetObject.transform.position) * _attractionSpeed * Time.deltaTime;
                _targetObject.angularVelocity = Vector3.zero;

                Debug.DrawLine(_camera.transform.position, _pointEndAction);
            }
            yield return null;
        }
    }

    private IEnumerator ProcessRaycastObject()
    {
        while (true)
        {
            if (!_isObjectGrabbed)
            {
                Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var distanceToObject = Vector3.Distance(transform.position, _targetObject.transform.position);

                    if (hit.collider.gameObject == _targetObject.gameObject && distanceToObject <= _distanceActive)
                    {
                        _isObjectInCenterCursor = true;
                        _tMPText.enabled = true;
                    }
                    else
                    {
                        _isObjectInCenterCursor = false;
                        _tMPText.enabled = false;
                    }
                }
            }
            yield return null;
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
        Gizmos.DrawLine(_camera.transform.position, _targetObject.transform.position);
        Gizmos.DrawLine(_camera.transform.position, transform.position + _camera.transform.forward * _distanceActive);
    }
}
