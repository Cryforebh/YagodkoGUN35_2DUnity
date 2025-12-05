using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreText : MonoBehaviour
{
    [SerializeField] private ScoreSkittles _scoreSkittles;

    private TMP_Text _tMP_Text;

    private void Awake()
    {
        _tMP_Text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        UpdateScore();
        _scoreSkittles.NextRoundEvent += _scoreSkittles_NextRoundEvent;
        _scoreSkittles.EndGameEvent += _scoreSkittles_EndGameEvent;
    }

    private void _scoreSkittles_EndGameEvent()
    {
        UpdateScore();
    }

    private void _scoreSkittles_NextRoundEvent()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        _tMP_Text.text = _scoreSkittles.TextRound;
    }

    private void OnDisable()
    {
        _scoreSkittles.NextRoundEvent -= _scoreSkittles_NextRoundEvent;
        _scoreSkittles.EndGameEvent -= _scoreSkittles_EndGameEvent;
    }
}
