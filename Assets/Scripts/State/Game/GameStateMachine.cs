using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEventAggregator;

public class GameStateMachine : MonoBehaviour, IListener<RequestGameStateChange>
{
    GameState m_currentState;

    void Awake()
    {
        EventAggregator.Register<RequestGameStateChange>(this);
    }

    void ChanageState(GameState newState)
    {
        GameState oldState = m_currentState;
        m_currentState = newState;

        EventAggregator.SendMessage(new GameStateChangedEvent()
        {
            NewData = newState,
            OldData = oldState
        });
    }

    public GameState RequestStateChange(GameState state)
    {
        ChanageState(state);

        return state;
    }

    public void Handle(RequestGameStateChange message)
    {
        RequestStateChange(message.NewState);
    }
}
