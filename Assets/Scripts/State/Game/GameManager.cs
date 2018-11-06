using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventAggregator;

public class GameManager : MonoBehaviour, IListener<GameStateChangedEvent>, IListener<PlayerDamagedEvent>, IListener<LevelChangedEvent>
{
    [SerializeField]
    PlayerController m_playerPrefab;

    [SerializeField]
    FollowCamera m_camera;

    LevelData m_currentLevel;
    PlayerController m_player;
    float m_playerHealth = 0.0f;

    void Awake()
    {
        EventAggregator.Register<GameStateChangedEvent>(this);
        EventAggregator.Register<PlayerDamagedEvent>(this);
        EventAggregator.Register<LevelChangedEvent>(this);
    }

    void Start()
    {
        OpenMainMenu();
    }

    public void Handle(GameStateChangedEvent message)
    {
        if(message.NewData == GameState.Playing && m_currentLevel != null)
        {
            m_playerHealth = m_currentLevel.StartHealth;

            if(m_player)
            {
                Destroy(m_player.gameObject);
            }
            m_player = Instantiate(m_playerPrefab, m_currentLevel.PlayerStartPosition, Quaternion.identity) as PlayerController;
            m_camera.SetTarget(m_player.transform);
        }
        else if(message.NewData == GameState.MainMenu)
        {
            // TODO: Unload level??
        }
    }

    public void Handle(PlayerDamagedEvent message)
    {
        m_playerHealth -= message.Damage;

        if(m_playerHealth <= 0)
        {
            if (m_player)
            {
                Destroy(m_player.gameObject);
            }

            Debug.Log("You died...");

            EventAggregator.SendMessage(new RequestGameStateChange() { NewState = GameState.Dead });
        }
    }

    public void Handle(LevelChangedEvent message)
    {
        m_currentLevel = message.NewData;

        EventAggregator.SendMessage(new RequestGameStateChange() { NewState = GameState.Playing });
    }

    public void RestartLevel()
    {
        EventAggregator.SendMessage(new LoadLevelEvent() { LevelIndex = 0 });
    }

    public void OpenMainMenu()
    {
        EventAggregator.SendMessage(new RequestGameStateChange() { NewState = GameState.MainMenu });
    }
}
