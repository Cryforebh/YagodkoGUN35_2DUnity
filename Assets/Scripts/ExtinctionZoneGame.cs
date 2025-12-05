using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinctionZoneGame : MonoBehaviour
{
    [SerializeField] private ScoreSkittles _scoreSkittles;
    [SerializeField] private Transform _objectTransform;
    [SerializeField] private GameObject _skittles;
    [SerializeField] private float _thresholdSkittles = 30f; // Допустимое отклонение от эталонных углов (в градусах)

    private Collider _collider;
    private Vector3 _startPosition;
    private Skittle[] _allSkittles;
    private Dictionary<Skittle, Vector3> _allSkittlesStartPosition;
    private Dictionary<Skittle, Quaternion> _allSkittlesStartRotation;
    private Dictionary<Skittle, Rigidbody> _allSkittlesRigidbodyCache;

    private int _numberOfAttempts = 0;
    private bool _isStrike = false;
    private bool _isSpare = false;
    private int _countRemainingSkittles = 0;
    private int _countSkittles;

    public event Action OnCollisionEnter;
    public event Action OnCollisionExit;
    public event Action OnCollisionExitEarly;

    public event Action StrikeEvent;
    public event Action SpareEvent;

    private void Awake()
    {
        Construct();
    }

    private void Construct()
    {
        _allSkittles = _skittles.GetComponentsInChildren<Skittle>();

        _allSkittlesStartPosition = new Dictionary<Skittle, Vector3>();
        _allSkittlesStartRotation = new Dictionary<Skittle, Quaternion>();
        _allSkittlesRigidbodyCache = new Dictionary<Skittle, Rigidbody>();

        foreach (var skittle in _allSkittles)
        {
            _allSkittlesStartPosition.Add(skittle, skittle.transform.position);
            _allSkittlesStartRotation.Add(skittle, skittle.transform.rotation);
            _allSkittlesRigidbodyCache.Add(skittle, skittle.GetComponent<Rigidbody>());
        }

        _scoreSkittles.ScoreValue = 0;
        _scoreSkittles.IsEndGame = false;
    }

    private void Start()
    {
        _startPosition = _objectTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            _collider = other;
            other.gameObject.SetActive(false);
            StartCoroutine(ProcessReloade());
            OnCollisionEnter?.Invoke();
        }
        if (other.gameObject.tag == "Skittle")
        {
            other.gameObject.SetActive(false);
            _scoreSkittles.ScoreValue += 1;
            //print("Счет увеличен");
        }
    }

    private void ThrowResultInSkittles()
    {
        if (_scoreSkittles.IsEndGame) return;

        _numberOfAttempts += 1;
        Debug.LogWarning($"Был {_numberOfAttempts} бросок!");

        foreach (var skittle in _allSkittles)
        {
            if (!IsStandingQuaternionSkittle(skittle) && skittle.gameObject.activeSelf == true)
            {
                skittle.gameObject.SetActive(false);
                _scoreSkittles.ScoreValue += 1;
                //print("Счет увеличен");
            }
            SetInitialPropertySkittle(skittle);
        }

        // Выбиты все кегли
        if (!IsInStockSkittles())
        {
            // Страйк
            if (_numberOfAttempts == 1)
            {
                // Если в прошлый раз был Страйк или Спэр
                if (_isStrike || _isSpare)
                {
                    _scoreSkittles.ScoreValue += _allSkittles.Length;
                    Debug.LogWarning($"Добавленно {_allSkittles.Length} бонусных очков!");
                }

                _isStrike = true;
                StrikeEvent?.Invoke();
                print($"Страйк!!!!");
            }

            // Спэр
            if (_numberOfAttempts == 2)
            {
                // Если в прошлый раз был Страйк
                if (_isStrike)
                {
                    _scoreSkittles.ScoreValue += _countSkittles;
                    Debug.LogWarning($"Добавленно {_countSkittles} бонусных очков!");
                }

                _isSpare = true;
                SpareEvent?.Invoke();
                print($"Спэр!");
            }

            _numberOfAttempts = 0;
            RespawnSkittles();
            _scoreSkittles.NextRound();
        }
        // Кегли еще остались
        else
        {
            // Первый бросок
            if ( _numberOfAttempts == 1)
            {
                if (_isStrike || _isSpare)
                {
                    _isSpare = false;
                    _countSkittles = _countRemainingSkittles;
                    _scoreSkittles.ScoreValue += _allSkittles.Length - _countSkittles;
                    //print($"Начислены доплнительные балы за прошлый бросок!");
                    Debug.LogWarning($"Добавленно {_allSkittles.Length - _countSkittles} бонусных очков!");
                }
            }
            // Второй бросок
            if (_numberOfAttempts >= 2)
            {
                if(_isStrike) 
                {
                    _isStrike = false;
                    _scoreSkittles.ScoreValue += _countSkittles;
                    //print($"Начислены доплнительные балы за прошлый Страйк!");
                    Debug.LogWarning($"Добавленно {_countSkittles} бонусных очков!");
                }

                _numberOfAttempts = 0;
                RespawnSkittles();
                _scoreSkittles.NextRound();
            }
        }
    }

    private IEnumerator ProcessReloade()
    {
        yield return new WaitForSeconds(1.8f);
        OnCollisionExitEarly?.Invoke();

        yield return new WaitForSeconds(1.2f);
        _collider.gameObject.transform.position = _startPosition;
        _collider.gameObject.SetActive(true);
        ThrowResultInSkittles();
        OnCollisionExit?.Invoke();
    }

    private bool IsStandingQuaternionSkittle(Skittle skittle)
    {
        Vector3 upDirection = skittle.transform.rotation * Vector3.up;
        return Vector3.Angle(upDirection, Vector3.up) < _thresholdSkittles;
    }

    private bool IsInStockSkittles()
    {
        _countRemainingSkittles = 0;
        foreach (var skittle in _allSkittles)
        {
            if (skittle.gameObject.activeSelf == true) _countRemainingSkittles += 1;
        }

        if (_countRemainingSkittles == 0) return false;
        return true;
    }

    private void RespawnSkittles()
    {
        foreach (var skittle in _allSkittles)
        {
            skittle.gameObject.SetActive(true);
            SetInitialPropertySkittle(skittle);
        }
        print("Респавн");
    }

    private void SetInitialPropertySkittle(Skittle skittle)
    {
        // Останавливаем физику
        Rigidbody rb = _allSkittlesRigidbodyCache[skittle];
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        // Задаем изначальную позицию
        skittle.transform.rotation = _allSkittlesStartRotation[skittle];
        skittle.transform.position = _allSkittlesStartPosition[skittle];
    }
}
