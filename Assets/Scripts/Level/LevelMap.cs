using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
    [SerializeField]
    Transform m_playerStartPosition;

    public Vector2 PlayerStartPosition { get { return m_playerStartPosition.position; } }
}
