using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private PlayerBase m_player;
    private Dictionary<IDPlayerState,IPlayerState> m_allStates;
    private IDPlayerState m_currentState;

    public PlayerStateMachine(PlayerBase player)
    {
        m_player = player;
        m_allStates = new Dictionary<IDPlayerState,IPlayerState>();
    }

    public IDPlayerState GetCurrentState() => m_currentState;
    public PlayerBase GetPlayer() => m_player;
    public Dictionary<IDPlayerState, IPlayerState> GetAllStates() => m_allStates;

    public void AddState(IPlayerState state) => m_allStates.Add(state.GetID(), state);

    public IPlayerState GetState(IDPlayerState iDPlayerState) => m_allStates.GetValueOrDefault(iDPlayerState);

    public void Update() => GetState(m_currentState)?.Update(m_player);

    public void ChangeState(IDPlayerState nextState)
    {
        var state = GetState(nextState);
        if (state == null)
        {
            Debug.LogError("Зафиксированно незарегистрированное состояние!");
        }

        if (nextState != m_currentState)
        {
            GetState(m_currentState)?.Exit(m_player);
            m_currentState = nextState;
            GetState(m_currentState)?.Enter(m_player);
        }
    }
}
