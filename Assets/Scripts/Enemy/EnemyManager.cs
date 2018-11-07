using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventAggregator;

public class EnemyManager : MonoBehaviour, IListener<LevelChangedEvent>
{
    LevelData m_currentLevel;
    LevelMap m_currentMap;

    void Awake()
    {
        EventAggregator.Register<LevelChangedEvent>(this);
    }

    public void Handle(LevelChangedEvent message)
    {
        m_currentLevel = message.NewData;
        m_currentMap = message.Map;
    }
}
