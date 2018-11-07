using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventAggregator;

public class SpawnPoint : MonoBehaviour, IListener<GameStateChangedEvent>
{
    [SerializeField]
    Enemy[] m_enemyPrefabs;

    [SerializeField]
    float m_spawnRate = 10; // Seconds

    float m_lastSpawn = 0;

    List<Enemy> m_spawned = new List<Enemy>();

    void Awake()
    {
        EventAggregator.Register<GameStateChangedEvent>(this);
    }

    void Start()
    {
        m_lastSpawn = Time.time;

        Spawn();
    }

    void Update()
    {
        if(Time.time > m_lastSpawn + m_spawnRate)
        {
            m_lastSpawn = Time.time;
            Spawn();
        }
    }

    void OnEnable()
    {
        SetActive(true);
    }

    void OnDisable()
    {
        SetActive(false);
    }

    void SetActive(bool active)
    {
        foreach (Enemy enemy in m_spawned)
        {
            if (enemy)
            {
                enemy.enabled = false;
            }
        }
    }

    void OnDestroy()
    {
        foreach(Enemy enemy in m_spawned)
        {
            if (enemy)
            {
                Destroy(enemy.gameObject);
            }
        }
        m_spawned.Clear();

        EventAggregator.UnRegister<GameStateChangedEvent>(this);
    }

    void Spawn()
    {
        Enemy enemy = Instantiate(m_enemyPrefabs[0], transform.position, transform.rotation);
        enemy.Died += OnEnemyDied;

        m_spawned.Add(enemy);
    }

    void OnEnemyDied(Enemy enemy)
    {
        if(m_spawned.Contains(enemy))
        {
            Destroy(enemy.gameObject);
            m_spawned.Remove(enemy);
        }
    }

    public void Handle(GameStateChangedEvent message)
    {
        enabled = message.NewData == GameState.Playing;
    }


}
