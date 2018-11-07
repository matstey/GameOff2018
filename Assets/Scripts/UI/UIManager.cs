using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventAggregator;

public class UIManager : MonoBehaviour, IListener<GameStateChangedEvent>
{
    [SerializeField]
    GameObject m_mainMenu;

    [SerializeField]
    GameObject m_deadUI;

    [SerializeField]
    GameObject m_gameUI;

    void Awake()
    {
        EventAggregator.Register<GameStateChangedEvent>(this);
    }

    public void Handle(GameStateChangedEvent message)
    {
        if(message.NewData == GameState.Dead)
        {
            m_deadUI.SetActive(true);
            m_mainMenu.SetActive(false);
            m_gameUI.SetActive(true);
        }
        else if (message.NewData == GameState.Playing)
        {
            m_deadUI.SetActive(false);
            m_mainMenu.SetActive(false);
            m_gameUI.SetActive(true);
        }
        else if (message.NewData == GameState.MainMenu)
        {
            m_deadUI.SetActive(false);
            m_mainMenu.SetActive(true);
            m_gameUI.SetActive(false);
        }
    }
}
