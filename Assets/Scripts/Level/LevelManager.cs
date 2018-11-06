using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEventAggregator;

public class LevelManager : MonoBehaviour, IListener<LoadLevelEvent>
{
    [SerializeField]
    LevelData[] m_levels;

    LevelData m_currentLevel = null;

    LevelMap m_currentMap = null;

    void Awake()
    {
        EventAggregator.Register<LoadLevelEvent>(this);
    }

    public void Handle(LoadLevelEvent message)
    {
        LoadLevel(message.LevelIndex);
    }

    public void LoadLevel(int index)
    {
        LevelData oldLevel = m_currentLevel;
        m_currentLevel = m_levels[index];

        m_currentMap = Instantiate(m_currentLevel.MapPrefab, Vector3.zero, Quaternion.identity);

        EventAggregator.SendMessage(new LevelChangedEvent()
        {
            NewData = m_currentLevel,
            OldData = oldLevel
        });
    }
}
