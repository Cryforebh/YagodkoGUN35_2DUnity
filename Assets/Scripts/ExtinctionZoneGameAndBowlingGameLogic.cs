using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinctionZoneGameAndBowlingGameLogic : MonoBehaviour
{
    [SerializeField] private ScoreSkittles _scoreSkittles;
    [SerializeField] private Transform _objectTransform;
    [SerializeField] private GameObject _skittles;
    [SerializeField] private float _thresholdSkittles = 30f; // Допустимое отклонение от эталонных углов (в градусах)

    private Collider _colliderBall;
    private Vector3 _startPosition;
    private Skittle[] _allSkittles;
    private Dictionary<Skittle, Vector3> _allSkittlesStartPosition;
    private Dictionary<Skittle, Quaternion> _allSkittlesStartRotation;
    private Dictionary<Skittle, Rigidbody> _allSkittlesRigidbodyCache;

    private int _numberOfAttempts = 0;
    private bool _isStrike = false;
    private bool _isSpare = false;
    private int _countRemainingSkittles = 0;
    private int _shotDownLastTimeSkittles;
    private Skittle _currentSkittle;


    public event Action OnCollisionEnter;
    public event Action ReloadeEvent;
    public event Action ReloadeEarlyEvent;

    public event Action StrikeEvent;
    public event Action SpareEvent;
    public event Action WinEvent;

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
        _scoreSkittles.EndGameEvent += _scoreSkittles_EndGameEvent;
    }

    private void _scoreSkittles_EndGameEvent()
    {
        WinEvent?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_scoreSkittles.IsEndGame)
        {
            _scoreSkittles.ScoreValue = 0;
            _scoreSkittles.IsEndGame = false;
        }

        if (other.gameObject.tag == "Ball")
        {
            _colliderBall = other;
            StartCoroutine(ProcessReloade());
            OnCollisionEnter?.Invoke();
        }
        if (other.gameObject.tag == "Skittle")
        {
            _currentSkittle = other.gameObject.GetComponent<Skittle>();
            _currentSkittle.SkittleOut = true;
        }
    }

    private void ThrowResultInSkittles()
    {
        _numberOfAttempts += 1;
        Debug.LogWarning($"Был {_numberOfAttempts} бросок!");

        foreach (var skittle in _allSkittles)
        {
            if ((!IsStandingQuaternionSkittle(skittle) || skittle.SkittleOut == true) && skittle.gameObject.activeSelf == true)
            {
                skittle.gameObject.SetActive(false);
                _scoreSkittles.ScoreValue += 1;
            }
            SetInitialPropertySkittle(skittle);
        }

        // Кегли еще остались
        if (IsInStockSkittles())
        {
            // Первый бросок
            if (_numberOfAttempts == 1)
            {
                _shotDownLastTimeSkittles = _allSkittles.Length - _countRemainingSkittles;
                if (_isStrike || _isSpare)
                {
                    _isSpare = false;
                    _scoreSkittles.ScoreValue += _shotDownLastTimeSkittles;
                    Debug.Log($"Добавленно {_shotDownLastTimeSkittles} бонусных очков!");
                }
            }
            // Второй бросок
            if (_numberOfAttempts >= 2)
            {
                _shotDownLastTimeSkittles = _allSkittles.Length - (_shotDownLastTimeSkittles + _countRemainingSkittles);
                if (_isStrike)
                {
                    _isStrike = false;
                    _scoreSkittles.ScoreValue += _shotDownLastTimeSkittles;
                    Debug.Log($"Добавленно {_shotDownLastTimeSkittles} бонусных очков!");
                }
                RespawnSkittles();
            }
        }
        // Выбиты все кегли
        else
        {
            // Страйк
            if (_numberOfAttempts == 1)
            {
                _shotDownLastTimeSkittles = _allSkittles.Length - _countRemainingSkittles;
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
                _shotDownLastTimeSkittles = _allSkittles.Length - (_shotDownLastTimeSkittles + _countRemainingSkittles);
                // Если в прошлый раз был Страйк
                if (_isStrike)
                {
                    _isStrike = false;
                    _scoreSkittles.ScoreValue += _shotDownLastTimeSkittles;
                    Debug.LogWarning($"Добавленно {_shotDownLastTimeSkittles} бонусных очков!");
                }

                _isSpare = true;
                SpareEvent?.Invoke();
                print($"Спэр!");
            }
            RespawnSkittles();
        }
    }

    private IEnumerator ProcessReloade()
    {
        yield return new WaitForSeconds(1.8f);
        _colliderBall.gameObject.SetActive(false);
        ReloadeEarlyEvent?.Invoke();

        yield return new WaitForSeconds(1.2f);
        _colliderBall.gameObject.transform.position = _startPosition;
        _colliderBall.gameObject.SetActive(true);
        ResetPhysicMoveToObject(_colliderBall.gameObject);
        ThrowResultInSkittles();
        ReloadeEvent?.Invoke();
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
        _numberOfAttempts = 0;
        foreach (var skittle in _allSkittles)
        {
            skittle.SkittleOut = false;
            skittle.gameObject.SetActive(true);
            SetInitialPropertySkittle(skittle);
        }
        _scoreSkittles.NextRound();
        Debug.Log("Респавн - следующий этап включен");
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

    private void ResetPhysicMoveToObject(GameObject gObject)
    {
        // Останавливаем физику
        Rigidbody rb = gObject.GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }
}
