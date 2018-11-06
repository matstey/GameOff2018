using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    LevelMap m_mapPrefab;

    public LevelMap MapPrefab { get { return m_mapPrefab; } }

    [SerializeField]
    float m_startHealth = 100;

    public float StartHealth{ get { return m_startHealth; } }

    public Vector2 PlayerStartPosition { get { return m_mapPrefab.PlayerStartPosition; } }
}
