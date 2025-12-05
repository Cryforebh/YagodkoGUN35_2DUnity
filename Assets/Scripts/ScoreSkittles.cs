using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSkittles", menuName = "ScriptableObjects/ScoreSkittles")]
public class ScoreSkittles : ScriptableObject
{
    private string _textRound = "";
    private string _textAllScoreName = "Общий счет: ";
    private int _scoreValue = 0;
    private int _allScoreValue = 0;
    private int _roundCount = 10;
    private int _currentRound = 1;
    private bool _isEndGame = false;

    public event Action ScoreUpdateEvent;
    public event Action NextRoundEvent;
    public event Action EndGameEvent;

    public string TextRound => _textRound;
    public string TextAllScoreName => _textAllScoreName;
    public string TextAllScoreValue => GetTextValue(_allScoreValue);

    public bool IsEndGame
    {
        get => _isEndGame;
        set
        {
            _isEndGame = value;
            if (_isEndGame == false)
            {
                _textRound = "";
                _scoreValue = 0;
                _allScoreValue = 0;
                _roundCount = 10;
                _currentRound = 1;
            }
        }
    }

    public int ScoreValue
    {
        get => _scoreValue;
        set
        {
            _scoreValue = value;
            ScoreUpdateEvent?.Invoke();
        }
    }

    public void NextRound() 
    {
        _roundCount -= 1;
        if (_roundCount != 0)
        {
            // Первый раунд
            if (_currentRound == 1)
            {
                _textRound = $"{_currentRound}:{GetTextValue(_scoreValue)}";
            }
            else
            {
                _textRound = $"{_textRound}, {_currentRound}:{GetTextValue(_scoreValue)}";
            }
            _allScoreValue += _scoreValue;
            _currentRound += 1;
            _scoreValue = 0;

            NextRoundEvent?.Invoke();
        }
        else if (_roundCount == 0)
        {
            _isEndGame = true;
            _textRound = $"{_textAllScoreName}{_allScoreValue}";
            EndGameEvent?.Invoke();
        }
    }

    private string GetTextValue(int score)
    {
        int charCount = $"{score}".Length;
        switch (charCount)
        {
            case 1:
                return $"0{score}";
            default:
                return $"{score}";
        }
    }
}
